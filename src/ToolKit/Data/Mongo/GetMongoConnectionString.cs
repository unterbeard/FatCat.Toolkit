#nullable enable
namespace FatCat.Toolkit.Data.Mongo;

public interface IGetMongoConnectionString
{
	string? Get();
}

public class GetMongoConnectionString : IGetMongoConnectionString
{
	public string? Get() => null;
}