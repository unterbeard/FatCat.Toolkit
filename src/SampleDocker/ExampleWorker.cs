using FatCat.Fakes;

namespace SampleDocker;

public interface IExampleWorker
{
	string GetMessage();
}

public class ExampleWorker : IExampleWorker
{
	public string GetMessage() => $"This is the message | {Faker.RandomInt()}";
}