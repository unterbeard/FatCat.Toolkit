using System.Linq.Expressions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Testing;

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
}