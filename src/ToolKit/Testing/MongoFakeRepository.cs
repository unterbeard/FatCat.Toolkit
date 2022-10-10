using System.Linq.Expressions;
using FakeItEasy;
using FatCat.Fakes;
using FatCat.Toolkit.Data.Mongo;
using FluentAssertions;
using MongoDB.Bson;

namespace FatCat.Toolkit.Testing;

public class MongoFakeRepository<T> : IMongoRepository<T> where T : MongoObject
{
	private readonly IMongoRepository<T> repository;

	public EasyCapture<T> CreatedCapture { get; } = new();

	public T CreatedItem { get; set; } = null!;

	public List<T> CreatedList { get; set; } = null!;

	public string DatabaseName => repository.DatabaseName;

	public T DeletedItem { get; set; } = null!;

	public List<T> DeletedList { get; set; } = null!;

	public EasyCapture<Expression<Func<T, bool>>> FilterCapture { get; private set; } = null!;

	public T Item { get; set; } = null!;

	public string ItemId { get; set; } = null!;

	public List<T> Items { get; set; } = null!;

	public EasyCapture<T> UpdatedCapture { get; } = new();

	public T UpdatedItem { get; set; } = null!;

	public List<T> UpdatedList { get; set; } = null!;

	public MongoFakeRepository()
	{
		repository = A.Fake<IMongoRepository<T>>();

		SetUpGet();
		SetUpCreate();
		SetUpUpdate();
		SetUpDelete();
		SetUpGetByFilter();
	}

	public async Task<T> Create(T item) => await repository.Create(item);

	public async Task<List<T>> Create(List<T> items) => await repository.Create(items);

	public async Task<T> Delete(T item) => await repository.Delete(item);

	public async Task<List<T>> Delete(List<T> items) => await repository.Delete(items);

	public async Task<List<T>> GetAll() => await repository.GetAll();

	public async Task<List<T>> GetAllByFilter(Expression<Func<T, bool>> filter) => await repository.GetAllByFilter(filter);

	public async Task<T?> GetByFilter(Expression<Func<T, bool>> filter) => await repository.GetByFilter(filter);

	public async Task<T?> GetById(string id) => await repository.GetById(id);

	public async Task<T?> GetById(ObjectId id) => await repository.GetById(id);

	public async Task<T?> GetFirst() => await repository.GetFirst();

	public async Task<T> GetFirstOrCreate() => await repository.GetFirstOrCreate();

	public async Task<T> Update(T item) => await repository.Update(item);

	public async Task<List<T>> Update(List<T> items) => await repository.Update(items);

	public void VerifyCreate(T expectedItem)
	{
		A.CallTo(() => repository.Create(A<T>._))
		.MustHaveHappened();

		CreatedCapture.Value
					.Should()
					.Be(expectedItem);
	}

	public void VerifyCreate()
	{
		A.CallTo(() => repository.Create(A<T>._))
		.MustHaveHappened();
	}

	public void VerifyDidNotCreate()
	{
		A.CallTo(() => repository.Create(A<T>._))
		.MustNotHaveHappened();
	}

	public void VerifyDidNotGetAll()
	{
		A.CallTo(() => repository.GetAll())
		.MustNotHaveHappened();
	}

	public void VerifyDidNotGetByFilter()
	{
		FilterCapture.Value
					.Should()
					.BeNull();
	}

	public void VerifyDidNotGetById()
	{
		A.CallTo(() => repository.GetById(A<string>._))
		.MustHaveHappened();
	}

	public void VerifyDidNotUpdate()
	{
		A.CallTo(() => repository.Update(A<T>._))
		.MustNotHaveHappened();
	}

	public void VerifyGetAll()
	{
		A.CallTo(() => repository.GetAll())
		.MustHaveHappened();
	}

	public void VerifyGetByFilterByItemFalse(T item)
	{
		FilterCapture.Value
					.Should()
					.NotBeNull();

		var compliedExpression = FilterCapture.Value.Compile();

		compliedExpression(item)
			.Should()
			.BeFalse();
	}

	public void VerifyGetByFilterByItemTrue(T item)
	{
		FilterCapture.Value
					.Should()
					.NotBeNull();

		var compliedExpression = FilterCapture.Value.Compile();

		compliedExpression(item)
			.Should()
			.BeTrue();
	}

	public void VerifyGetById()
	{
		A.CallTo(() => repository.GetById(ItemId))
		.MustHaveHappened();
	}

	public void VerifyGetFirst()
	{
		A.CallTo(() => repository.GetFirst())
		.MustHaveHappened();
	}

	public void VerifyNotGetFirst()
	{
		A.CallTo(() => repository.GetFirst())
		.MustNotHaveHappened();
	}

	public void VerifyUpdate(T expectedData)
	{
		A.CallTo(() => repository.Update(expectedData))
		.MustHaveHappened();
	}

	private void SetUpCreate()
	{
		CreatedItem = Faker.Create<T>();
		CreatedList = Faker.Create<List<T>>();

		A.CallTo(() => repository.Create(CreatedCapture))
		.ReturnsLazily(() => CreatedItem);

		A.CallTo(() => repository.Create(A<List<T>>._))
		.ReturnsLazily(() => CreatedList);
	}

	private void SetUpDelete()
	{
		DeletedItem = Faker.Create<T>();
		DeletedList = Faker.Create<List<T>>();

		A.CallTo(() => repository.Delete(A<T>._))
		.ReturnsLazily(() => DeletedItem);

		A.CallTo(() => repository.Delete(A<List<T>>._))
		.ReturnsLazily(() => DeletedList);
	}

	private void SetUpGet()
	{
		ItemId = Faker.RandomString();
		Items = Faker.Create<List<T>>();
		Item = Faker.Create<T>();

		A.CallTo(() => repository.GetById(A<string>._))
		.ReturnsLazily(() => Item);

		A.CallTo(() => repository.GetFirst())
		.ReturnsLazily(() => Item);

		A.CallTo(() => repository.GetAll())
		.ReturnsLazily(() => Items);

		A.CallTo(() => repository.GetAllByFilter(A<Expression<Func<T, bool>>>._))
		.ReturnsLazily(() => Items);
	}

	private void SetUpGetByFilter()
	{
		FilterCapture = new EasyCapture<Expression<Func<T, bool>>>();

		A.CallTo(() => repository.GetByFilter(FilterCapture))
		.ReturnsLazily(() => Item);
	}

	private void SetUpUpdate()
	{
		UpdatedItem = Faker.Create<T>();
		UpdatedList = Faker.Create<List<T>>();

		A.CallTo(() => repository.Update(UpdatedCapture))
		.ReturnsLazily(() => UpdatedItem);

		A.CallTo(() => repository.Update(A<List<T>>._))
		.ReturnsLazily(() => UpdatedList);
	}
}