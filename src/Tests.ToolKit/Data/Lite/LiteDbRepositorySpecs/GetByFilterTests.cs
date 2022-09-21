using FluentAssertions;
using FakeItEasy;
using FatCat.Fakes;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public class GetByFilterTests : RequireCollectionLiteDbRepositoryTests<LiteDbTestObject>
{
	private int numberToFind;

	public GetByFilterTests() { numberToFind = Faker.RandomInt(); }

	protected override Task<LiteDbTestObject> RunTest() => repository.GetByFilter(i => i.SomeNumber == numberToFind);
}