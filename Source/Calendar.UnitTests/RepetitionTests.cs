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
		public void Repetition_ForWeekPeriod_DaysOfWeek_Are_Stored_Correctly()
		{
			//Arrange
			var daysOfWeek = new List<DayOfWeek> {DayOfWeek.Sunday, DayOfWeek.Monday};
			//Act
			var repetition = new Repetition(StartDate, 1, daysOfWeek);
			//Assert
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Sunday), "expected Sunday");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Monday), "expected Monday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Tuesday), "unexpected Tuesday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Wednesday), "unexpected Wednesday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Thursday), "unexpected Thursday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Friday), "unexpected Friday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == DayOfWeek.Saturday), "unexpected Saturday");
		}

		[TestMethod]
		public void Repetition_ForWeekPeriod_Default_DayOfWeek_Is_Set()
		{
			//Arrange
			var dayOfWeek = DateTime.Now.DayOfWeek;
			//Act
			var repetition = new Repetition(StartDate, 1, null);
			//Assert
			Assert.AreEqual(1, repetition.OnCertainDaysOfWeek.Count(), "more than one default day of week");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == dayOfWeek), "default day of week differs from current day of week");
		}

		[TestMethod]
		public void Repetition_ForWeekPeriod_CalculateExpirationDate_ForInfinityRepetition()
		{
			//Arrange
			var repetition = new Repetition(StartDate, RepetitionInWeeks, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
			//Act
			var date = repetition.CalculateExpirationDate();
			//Assert
			Assert.IsNull(date);
		}

		[TestMethod]
		public void Repetition_ForWeekPeriod_CalculateExpirationDate_ForRepetitionExpiredOnFixedDate()
		{
			//Arrange
			var expectedDate = DateTime.Now.Date.AddDays(3);
			var repetition = new Repetition(StartDate, RepetitionInWeeks, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
			repetition.ContinueToDate(expectedDate);
			//Act
			var date = repetition.CalculateExpirationDate();
			//Assert
			Assert.IsNotNull(date);
			Assert.AreEqual(expectedDate, date.Value);
		}

		[TestMethod]
		public void Repetition_ForWeekPeriod_CalculateExpirationDate_ForFixedNumberOfTimesRepetition()
		{
			//Arrange
			const int howManyTimes = 11;
			var repetition = new Repetition(StartDate, RepetitionInWeeks, new List<DayOfWeek> { DayOfWeek.Sunday, DayOfWeek.Monday });
			repetition.ContinueFixedNumberOfTimes(howManyTimes);
			//Act
			var date = repetition.CalculateExpirationDate();
			//Assert
			Assert.IsNotNull(date);
			Assert.AreEqual(new DateTime(2015, 08, 10), date.Value);
		}

		[TestMethod]
		public void Repetition_ForYearPeriod_CalculateExpirationDate_ForFixedNumberOfTimesRepetition()
		{
			//Arrange
			const int howManyTimes = 3;
			const int repetitionInYears = 2;
			var startDate = StartDate;
			var repetition = new Repetition(startDate, repetitionInYears, startDate);
			repetition.ContinueFixedNumberOfTimes(howManyTimes);
			//Act
			var date = repetition.CalculateExpirationDate();
			//Assert
			Assert.IsNotNull(date);
			Assert.AreEqual(startDate.AddYears(howManyTimes * repetitionInYears), date.Value);
		}

		[TestMethod]
		public void Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek()
		{
			//Arrange
			const int repetitionInMonths = 1;
			var startDate = new DateTime(2015, 03, 01);
			
			//Act
			var firstSunday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 0), true);
			var secondSunday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 1), true);
			var thirdSunday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 2), true);
			var fourthSunday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 3), true);
			var lastSunday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 4), true);

			var firstSaturday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 0 + 6), true);
			var secondSaturday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 1 + 6), true);
			var thirdSaturday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 2 + 6), true);
			var fourthSaturday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 3 + 6), true);
			var lastSaturday = new Repetition(startDate, repetitionInMonths, startDate.AddDays(7 * 4 + 6), true);

			//Assert
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(firstSunday, DayOfWeek.Sunday, ExactDayOfWeekInMonth.First);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(secondSunday, DayOfWeek.Sunday, ExactDayOfWeekInMonth.Second);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(thirdSunday, DayOfWeek.Sunday, ExactDayOfWeekInMonth.Third);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(fourthSunday, DayOfWeek.Sunday, ExactDayOfWeekInMonth.Fourth);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(lastSunday, DayOfWeek.Sunday, ExactDayOfWeekInMonth.Last);

			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(firstSaturday, DayOfWeek.Saturday, ExactDayOfWeekInMonth.First);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(secondSaturday, DayOfWeek.Saturday, ExactDayOfWeekInMonth.Second);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(thirdSaturday, DayOfWeek.Saturday, ExactDayOfWeekInMonth.Third);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(fourthSaturday, DayOfWeek.Saturday, ExactDayOfWeekInMonth.Last);
			Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(lastSaturday, DayOfWeek.Saturday, ExactDayOfWeekInMonth.First);
		}

		private static void Repetition_ForMonthPeriod_Correct_TransformToDayOfWeek_Assert(Repetition repetition, DayOfWeek dayOfWeek, ExactDayOfWeekInMonth exactDayOfWeekInMonth)
		{
			Assert.AreEqual(exactDayOfWeekInMonth, repetition.ExactDayOfWeekInMonth, "expected " + exactDayOfWeekInMonth.ToString().ToLower() + " " + dayOfWeek + " of month");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == dayOfWeek), "expected " + dayOfWeek);
		}

		//[TestMethod]
		//public void Repetition_ForMonthPeriod_CalculateExpirationDate_ForFixedNumberOfTimesRepetition()
		//{
		//}

		const int RepetitionInWeeks = 4;

		private static DateTime StartDate
		{
			get { return new DateTime(2015, 03, 22); }
		}

	}
}
