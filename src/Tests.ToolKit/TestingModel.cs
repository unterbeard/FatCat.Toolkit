using FatCat.Toolkit;

namespace Tests.FatCat.Toolkit;

public class TestingModel : EqualObject
{
	public string Description { get; set; }

	public string Name { get; set; }

	public List<SubModel> SubModels { get; set; } = new();

	public Guid TheId { get; set; }
}

public class SubModel : EqualObject
{
	public string Name { get; set; }

	public Guid SubId { get; set; }
}