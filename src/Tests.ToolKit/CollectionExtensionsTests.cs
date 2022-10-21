﻿using FatCat.Toolkit;
using FatCat.Toolkit.Extensions;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit;

public class CollectionExtensionsTests
{
	[Fact]
	public void ComparableTestObjectWillBeTrueRegardlessOfOrder()
	{
		var firstList = new List<ComparableTestItem>
						{
							new()
							{
								Number = 1,
								AName = "One",
							},
							new()
							{
								Number = 2,
								AName = "Two"
							}
						};

		var secondList = new List<ComparableTestItem>
						{
							new()
							{
								Number = 2,
								AName = "Two"
							},
							new()
							{
								Number = 1,
								AName = "One",
							}
						};

		firstList.ListsAreEqual(secondList)
				.Should()
				.BeTrue();
	}

	[Fact]
	public void ComplicatedListsCanBeEqual()
	{
		var firstList = new List<TestItem>
						{
							new()
							{
								Number = 1,
								AName = "One",
							},
							new()
							{
								Number = 2,
								AName = "Two"
							}
						};

		var secondList = new List<TestItem>
						{
							new()
							{
								Number = 1,
								AName = "One",
							},
							new()
							{
								Number = 2,
								AName = "Two"
							}
						};

		firstList.ListsAreEqual(secondList)
				.Should()
				.BeTrue();
	}

	[Fact]
	public void ListWithDifferentElementsAreNotEqual()
	{
		var firstList = new List<int>
						{
							1,
							3,
							4
						};

		var secondList = new List<int>
						{
							1,
							4,
							5
						};

		firstList.ListsAreEqual(secondList)
				.Should()
				.BeFalse();
	}

	[Fact]
	public void TwoListAreEqual()
	{
		var firstList = new List<int>
						{
							1,
							3,
							4
						};

		var secondList = new List<int>
						{
							1,
							4,
							3
						};

		firstList.ListsAreEqual(secondList)
				.Should()
				.BeTrue();
	}

	private class ComparableTestItem : TestItem, IComparable<ComparableTestItem>
	{
		public int CompareTo(ComparableTestItem other) => Number.CompareTo(other.Number);
	}

	private class TestItem : EqualObject
	{
		public string AName { get; set; }

		public int Number { get; set; }
	}
}