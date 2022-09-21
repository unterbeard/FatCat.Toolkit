using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using LiteDB;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class DeleteTests : RequireCollectionLiteDbRepositoryTests<LiteDbTestObject>
{
	private readonly EasyCapture<BsonValue> deleteCapture;

	public DeleteTests()
	{
		deleteCapture = new EasyCapture<BsonValue>();

		A.CallTo(() => collection.Delete(deleteCapture))
		.Returns(true);
	}

	[Fact]
	public async Task DeleteInTheCollection()
	{
		await RunTest();

		A.CallTo(() => collection.Delete(A<BsonValue>._))
		.MustHaveHappened();

		deleteCapture.Value.AsInt32
					.Should()
					.Be(testItem.Id);
	}

	[Fact]
	public void ReturnDeletedItem()
	{
		RunTest()
			.Should()
			.Be(testItem);
	}

	protected override async Task<LiteDbTestObject> RunTest() => await repository.Delete(testItem);
}