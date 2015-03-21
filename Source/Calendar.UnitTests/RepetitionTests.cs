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
			var daysOfWeek = new List<CalendarDayOfWeek> {CalendarDayOfWeek.Sunday, CalendarDayOfWeek.Monday};
			//Act
			var repetition = new Repetition(TimeUnit.Day, 1, daysOfWeek);
			//Assert
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Sunday), "excpected Sunday");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Monday), "excpected Monday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Tuesday), "unexcpected Tuesday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Wednesday), "unexcpected Wednesday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Thursday), "unexcpected Thursday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Friday), "unexcpected Friday");
			Assert.IsFalse(repetition.OnCertainDaysOfWeek.Any(x => x == CalendarDayOfWeek.Saturday), "unexcpected Saturday");
		}

		[TestMethod]
		public void Repetition_Default_DayOfWeek_Is_Set()
		{
			//Arrange
			var calendarDayOfWeek = Repetition.SystemDayOfWeekToCalendarDayOfWeek(DateTime.Now.DayOfWeek);
			//Act
			var repetition = new Repetition(TimeUnit.Day, 1);
			//Assert
			Assert.AreEqual(1, repetition.OnCertainDaysOfWeek.Count(), "more than one default day of week");
			Assert.IsTrue(repetition.OnCertainDaysOfWeek.Any(x => x == calendarDayOfWeek), "default day of week differs from current day of week");
		}

		[TestMethod]
		public void Repetition_StaticMethod_SystemDayOfWeekToCalendarDayOfWeek_Gives_Proper_Result()
		{
			//Arrange
			const CalendarDayOfWeek calendarDayOfWeek = CalendarDayOfWeek.Monday;
			const DayOfWeek dayOfWeek = DayOfWeek.Monday;
			//Act
			var result = Repetition.SystemDayOfWeekToCalendarDayOfWeek(dayOfWeek);
			//Assert
			Assert.AreEqual(calendarDayOfWeek, result);
		}
	}
}
