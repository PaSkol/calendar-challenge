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
			var calendarDayOfWeek = DateTime.Now.DayOfWeek;//Repetition.SystemDayOfWeekToCalendarDayOfWeek(DateTime.Now.DayOfWeek);
			//Act
			var repetition = new Repetition(TimeUnit.Day, 1);
			//Assert
			Assert.AreEqual(1, repetition.OnCertainDaysOfWeek.Count(), "more than one default day of week");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == calendarDayOfWeek), "default day of week differs from current day of week");
		}

	}
}
