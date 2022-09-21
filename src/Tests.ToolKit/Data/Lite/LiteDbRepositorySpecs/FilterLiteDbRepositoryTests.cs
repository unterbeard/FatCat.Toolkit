using System.Linq.Expressions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;
using FluentAssertions;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public abstract class FilterLiteDbRepositoryTests<T> : RequireCollectionLiteDbRepositoryTests<T>
{
	protected readonly EasyCapture<Expression<Func<LiteDbTestObject, bool>>> filterCapture;
	protected readonly int numberToFind;

	protected abstract List<LiteDbTestObject> ItemsToReturn { get; }

	protected FilterLiteDbRepositoryTests()
	{
		numberToFind = Faker.RandomInt();

		filterCapture = new EasyCapture<Expression<Func<LiteDbTestObject, bool>>>();

		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.Returns(ItemsToReturn);
	}

	protected void SetUpFindWithEmptyCollection()
	{
		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.Returns(new List<LiteDbTestObject>());
	}

	protected void VerifyFilterCallOnCollectionMade()
	{
		A.CallTo(() => collection.Find(filterCapture, A<int>._, A<int>._))
		.MustHaveHappened();

		var expression = filterCapture.Value.Compile();

		var filterItem = new LiteDbTestObject { SomeNumber = numberToFind };

		expression(filterItem)
			.Should()
			.BeTrue();

		expression(new LiteDbTestObject { SomeNumber = numberToFind - 1 })
			.Should()
			.BeFalse();
	}
}