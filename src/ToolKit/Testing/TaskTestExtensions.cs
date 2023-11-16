﻿using FluentAssertions;
using FluentAssertions.Primitives;

namespace FatCat.Toolkit.Testing;

public static class TaskTestExtensions
{
	public static TaskTestAssertions<T> Should<T>(this Task<T> task) => new(task);
}

public class TaskTestAssertions<T> : ReferenceTypeAssertions<Task<T>, TaskTestAssertions<T>>
{
	protected override string Identifier
	{
		get => "Task Test Assertions";
	}

	public TaskTestAssertions(Task<T> subject)
		: base(subject) { }

	public TaskTestAssertions<T> Be(T expectedValue)
	{
		Subject.Result.Should().Be(expectedValue);

		return this;
	}

	public TaskTestAssertions<T> BeEquivalentTo(T expectedValue)
	{
		Subject.Result.Should().BeEquivalentTo(expectedValue);

		return this;
	}

	public TaskTestAssertions<T> BeFalse()
	{
		Subject.Result.Should().Be(false);

		return this;
	}

	public TaskTestAssertions<T> BeTrue()
	{
		Subject.Result.Should().Be(true);

		return this;
	}
}
