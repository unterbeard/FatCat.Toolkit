using FatCat.Toolkit.Data.Lite;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Data.Lite.LiteDbRepositorySpecs;

public abstract class RequireCollectionLiteDbRepositoryTests<T> : LiteDbRepositoryTests
{
	[Fact]
	public void IfCollectionIsNullThrowConnectionException()
	{
		repository.Collection = null;

		var testAction = () => RunTest().Wait();

		testAction
			.Should()
			.Throw<LiteDbCollectionException>();
	}

	protected abstract Task<T> RunTest();
}