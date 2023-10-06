using FatCat.Fakes;

namespace SampleDocker;

public interface ISecondInjectedThing
{
	int GetSomeNumber();
}

public class SecondInjectedThing : ISecondInjectedThing
{
	public int GetSomeNumber()
	{
		return Faker.RandomInt();
	}
}
