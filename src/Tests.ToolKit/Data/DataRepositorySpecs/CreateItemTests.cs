using FakeItEasy;
using FatCat.Toolkit.Testing;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.DataRepositorySpecs;

public class CreateItemTests : DataRepositoryTests
{
	[Fact]
	public async Task InsertOneOnTheCollection()
	{
		await repository.Create(item);

		A.CallTo(() => collection.InsertOneAsync(item, default, default))
		.MustHaveHappened();
	}

	[Fact]
	public async Task PlayingWithCapture()
	{
		var easyCapture = new EasyCapture<TestingDataObject>();

		A.CallTo(() => collection.InsertOneAsync(easyCapture, default, default))
		.Returns(Task.CompletedTask);

		await repository.Create(item);

		easyCapture.Value
					.Should()
					.BeEquivalentTo(item);
	}

	[Fact]
	public void ReturnItem()
	{
		repository.Create(item)
				.Should()
				.Be(item);
	}
}