using FakeItEasy;

namespace FatCat.Toolkit.Testing;

public class EasyCapture<T>
{
	public static implicit operator T(EasyCapture<T> capture)
	{
		A<T>.That.Matches(i =>
						{
							capture.CaptureValue(i);

							return true;
						},
						$"Captured parameter {typeof(T).FullName}");

		return default!;
	}

	private readonly List<T> values = new();

	public bool HasValues => values.Any();

	public T Value
	{
		get
		{
			return values.Count switch
			{
				0 => throw new InvalidOperationException("No values have been captured"),
				> 1 => throw new InvalidOperationException("Multiple values were captured, uses Values property instead"),
				_ => values.First()
			};
		}
	}

	public IReadOnlyList<T> Values => values.AsReadOnly();

	private void CaptureValue(T value) => values.Add(value);
}