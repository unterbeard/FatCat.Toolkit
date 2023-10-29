#nullable enable
using FatCat.Fakes;
using MongoDB.Bson;

namespace FatCat.Toolkit;

public interface IGenerator
{
	IEnumerable<byte> Bytes(long length);

	IEnumerable<byte> BytesFast(long length);

	bool IsValidObjectId(string idToTest);

	Guid NewGuid();

	string NewId();

	ObjectId NewObjectId();

	int NextRandom(int? minNumber = null, int? maxNumber = null);

	int RandomNumber(int? minNumber = null, int? maxNumber = null);

	string RandomString(string? prefix = null, int? length = null);
}

public class Generator : IGenerator
{
	public IEnumerable<byte> Bytes(long length)
	{
		var bytes = new List<byte>();

		for (var i = 0; i < length; i++)
		{
			bytes.Add((byte)Faker.RandomInt(0, 255));
		}

		return bytes;
	}

	public IEnumerable<byte> BytesFast(long length)
	{
		
		
		return Bytes(length);
	}

	public bool IsValidObjectId(string idToTest) => ObjectId.TryParse(idToTest, out _);

	public Guid NewGuid() => Guid.NewGuid();

	public string NewId() => NewObjectId().ToString();

	public ObjectId NewObjectId() => ObjectId.GenerateNewId();

	public int NextRandom(int? minNumber = null, int? maxNumber = null) => Faker.RandomInt(minNumber, maxNumber);

	public int RandomNumber(int? minNumber = null, int? maxNumber = null) => Faker.RandomInt(minNumber, maxNumber);

	public string RandomString(string? prefix = null, int? length = null) => Faker.RandomString(prefix, length);
}
