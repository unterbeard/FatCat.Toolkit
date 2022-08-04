using FatCat.Fakes;

namespace FatCat.Toolkit;

public interface IGenerator
{
	int NextRandom(int? minNumber = null, int? maxNumber = null);
}

public class Generator : IGenerator
{
	public int NextRandom(int? minNumber = null, int? maxNumber = null) => Faker.RandomInt(minNumber, maxNumber);
}