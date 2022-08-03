using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using MongoDB.Driver;
using Tests.Fog.Common.Data;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class GetFirstOrCreateTests : DataRepositoryTests
{
	private readonly EasyCapture<ExpressionFilterDefinition<TestingDataObject>> expressionCapture;
	private readonly TestingDataObject firstItem;
	private EasyCapture<TestingDataObject> insertCapture = null!;

	public GetFirstOrCreateTests()
	{
		SetUpInsertCapture();

		firstItem = Faker.Create<TestingDataObject>();

		expressionCapture = new EasyCapture<ExpressionFilterDefinition<TestingDataObject>>();

		SetUpToReturnItem();
	}

	[Fact]
	public async Task GetFirstItem()
	{
		await repository.GetFirstOrCreate();

		A.CallTo(() => collection.FindAsync<TestingDataObject>(A<ExpressionFilterDefinition<TestingDataObject>>._!, default, default))
		.MustHaveHappened();

		var filter = expressionCapture.Value.Expression.Compile();

		filter(Faker.Create<TestingDataObject>())
			.Should()
			.BeTrue();
	}

	[Fact]
	public void IfFirstItemIsNotNullReturnItem()
	{
		SetUpToReturnItem();

		repository.GetFirstOrCreate()
				.Should()
				.Be(firstItem);

		A.CallTo(() => collection.InsertOneAsync(A<TestingDataObject>._, default, default))
		.MustNotHaveHappened();
	}

	[Fact]
	public async Task IfFirstItemIsNullCreateIt()
	{
		SetUpToNotReturnAnItem();

		await repository.GetFirstOrCreate();

		A.CallTo(() => collection.InsertOneAsync(A<TestingDataObject>._, default, default))
		.MustHaveHappened();

		var insertedItem = insertCapture.Value;

		insertedItem
			.Should()
			.NotBeNull();

		insertedItem
			.Id
			.Should()
			.NotBeNull();

		insertedItem.Name
					.Should()
					.BeNullOrEmpty();

		insertedItem.SomeDate
					.Should()
					.Be(default);
	}

	[Fact]
	public void IfFirstItemIsNullReturnCreatedItem()
	{
		SetUpToNotReturnAnItem();

		repository.GetFirstOrCreate()
				.Should()
				.Be(insertCapture.Value);
	}

	private void SetUpInsertCapture()
	{
		insertCapture = new EasyCapture<TestingDataObject>();

		A.CallTo(() => collection.InsertOneAsync(insertCapture, default, default))
		.Returns(Task.CompletedTask);
	}

	private void SetUpToNotReturnAnItem()
	{
		A.CallTo(() => collection.FindAsync<TestingDataObject>(expressionCapture, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(new List<TestingDataObject>()));
	}

	private void SetUpToReturnItem()
	{
		A.CallTo(() => collection.FindAsync<TestingDataObject>(expressionCapture, default, default))
		.Returns(new TestingAsyncCursor<TestingDataObject>(new List<TestingDataObject> { firstItem }));
	}
}