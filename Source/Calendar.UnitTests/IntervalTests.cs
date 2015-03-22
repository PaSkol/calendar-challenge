using System;
using Calendar.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calendar.UnitTests
{
	[TestClass]
	public class IntervalTests
	{
		[TestMethod]
		public void IntervalSlice_BetweenIntervalSliceKind_CorrectCalculation()
		{
			CommonTest(PrepareFrom(+3), PrepareTo(-3), IntervalSliceKind.Between);
		}

		[TestMethod]
		public void IntervalSlice_CompleteIntervalSliceKind_CorrectCalculation()
		{
			CommonTest(PrepareFrom(-3), PrepareTo(+3), IntervalSliceKind.Complete);
		}

		[TestMethod]
		public void IntervalSlice_FromPastIntervalSliceKind_CorrectCalculation()
		{
			CommonTest(PrepareFrom(+3), PrepareTo(+3), IntervalSliceKind.FromPast);
		}

		[TestMethod]
		public void IntervalSlice_ToFutureIntervalSliceKind_CorrectCalculation()
		{
			CommonTest(PrepareFrom(-3), PrepareTo(-3), IntervalSliceKind.ToFuture);
		}

		[TestMethod]
		public void IntervalSlice_IsBeyondInterval()
		{
			CommonTest(new DateTime(2011, 01, 10), new DateTime(2011, 03, 10), IntervalSliceKind.Complete, 0);
		}

		private static void CommonTest(DateTime fromSlice, DateTime toSlice, IntervalSliceKind kind, int fullDays = -1)
		{
			//Arrange
			var interval = PrepareInterval();
			//Act
			var slice = new IntervalSlice(fromSlice, toSlice, interval);
			//Assert
			Assert.AreEqual(fullDays < 0 ? CalculateFullDays(fromSlice, toSlice, interval) : fullDays, slice.FullDays);
			Assert.AreEqual(kind, slice.Kind);
		}

		private static int CalculateFullDays(DateTime fromSlice, DateTime toSlice, Interval interval)
		{
			if (fromSlice < interval.From)
				fromSlice = interval.From;
			if (toSlice > interval.To)
				toSlice = interval.To;
			return toSlice.Date.Subtract(fromSlice.Date).Days + 1;
		}

		private static Interval PrepareInterval()
		{
			return new Interval(PrepareFrom(), PrepareTo());
		}

		private static DateTime PrepareFrom(int dayOffset = 0)
		{
			return new DateTime(2011, 10, 10 + dayOffset);
		}

		private static DateTime PrepareTo(int dayOffset = 0)
		{
			return new DateTime(2011, 11, 11 + dayOffset);
		}
	}
}
