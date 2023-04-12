using FatCat.Toolkit;

namespace SampleDocker;

public class WeatherForecast : EqualObject
{
	public DateTime Date { get; set; }

	public string MetaData { get; set; }

	public string Name { get; set; } = "This should now show up.  Because I told it too at 8:13 pm";

	public string SecondMetaData { get; set; }

	public string SomeMessage { get; set; }

	public string Summary { get; set; }

	public int TemperatureC { get; set; }

	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}