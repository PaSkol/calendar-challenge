using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calendar.Domain;

namespace Calendar.UnitTests
{
	[TestClass]
	public class RepetitionTests
	{
		[TestMethod]
		public void Repetition_DaysOfWeek_Are_Stored_Correctly()
		{
			//Arrange
			var daysOfWeek = new List<DayOfWeek> {DayOfWeek.Sunday, DayOfWeek.Monday};
			//Act
			var repetition = new Repetition(TimeUnit.Day, 1, daysOfWeek);
			//Assert
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Sunday), "excpected Sunday");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Monday), "excpected Monday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Tuesday), "unexcpected Tuesday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Wednesday), "unexcpected Wednesday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Thursday), "unexcpected Thursday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Friday), "unexcpected Friday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Saturday), "unexcpected Saturday");
		}

		[TestMethod]
		public void Repetition_Default_DayOfWeek_Is_Set()
		{
			//Arrange
			var dayOfWeek = DateTime.Now.DayOfWeek;
			//Act
			var repetition = new Repetition(TimeUnit.Day, 1);
			//Assert
			Assert.AreEqual(1, repetition.OnCertainDaysOfWeek.Count(), "more than one default day of week");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == dayOfWeek), "default day of week differs from current day of week");
		}

		[TestMethod]
		public void Repetition_CalculateExpirationDate_ForInfinityRepetition()
		{
			//Arrange
			var repetition = new Repetition(TimeUnit.Week, SpanInWeeks, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
			//Act
			var date = repetition.CalculateExpirationDate(StartDate);
			//Assert
			Assert.IsNull(date);
		}

		[TestMethod]
		public void Repetition_CalculateExpirationDate_ForRepetitionExpiredOnFixedDate()
		{
			//Arrange
			var expectedDate = DateTime.Now.Date.Add(new TimeSpan(3, 0, 0, 0));
			var repetition = new Repetition(TimeUnit.Week, SpanInWeeks, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
			repetition.ContinueToDate(expectedDate);
			//Act
			var date = repetition.CalculateExpirationDate(StartDate);
			//Assert
			Assert.IsNotNull(date);
			Assert.AreEqual(expectedDate, date.Value);
		}

		[TestMethod]
		public void Repetition_CalculateExpirationDate_ForFixedNumberOfTimesRepetition()
		{
			//Arrange
			const int howManyTimes = 11;
			var repetition = new Repetition(TimeUnit.Week, SpanInWeeks, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
			repetition.ContinueFixedNumberOfTimes(howManyTimes);
			//Act
			var date = repetition.CalculateExpirationDate(StartDate);
			//Assert
			Assert.IsNotNull(date);
			Assert.AreEqual(ExpirationDate, date.Value);
		}

		const int SpanInWeeks = 4;

		private static DateTime StartDate
		{
			get { return new DateTime(2015, 03, 22); }
		}

		private static DateTime ExpirationDate
		{
			get { return new DateTime(2015, 08, 10); }
		}
	}
}
