using System;
using System.Collections.Generic;

namespace Calendar.Domain
{
	public class Repetition : CalendarEntity
	{
		public Repetition(TimeUnit periodUnit, int period, IEnumerable<CalendarDayOfWeek> daysOfWeek = null)
			: this(daysOfWeek)
		{
			PeriodUnit = periodUnit;
			Period = period;
		}

		private Repetition(IEnumerable<CalendarDayOfWeek> daysOfWeek)
		{
			EncodedDaysOfWeek = EncodeDaysOfWeek(daysOfWeek ?? new List<CalendarDayOfWeek>() { SystemDayOfWeekToCalendarDayOfWeek(DateTime.Now.DayOfWeek) });
		}

		public int Period { get; set; }
		public TimeUnit PeriodUnit { get; set; }
		protected byte EncodedDaysOfWeek { get; set; }
		public DateTime? ContinueToDate { get; set; }
		public int ContinueFixedNumberOfTimes { get; set; }
		public bool ContinueIndefinitely { get; set; }

		public IEnumerable<CalendarDayOfWeek> OnCertainDaysOfWeek
		{
			get { return DecodeDaysOfWeek(EncodedDaysOfWeek); }
			set { EncodeDaysOfWeek(value); }
		}

		private static byte EncodeDaysOfWeek(IEnumerable<CalendarDayOfWeek> daysOfWeek)
		{
			var result = 0;
			foreach (var dayOfWeek in daysOfWeek)
				result |= (byte)dayOfWeek;
			return (byte)result;
		}

		private static IEnumerable<CalendarDayOfWeek> DecodeDaysOfWeek(byte encodedDaysOfWeek)
		{
			var result = new List<CalendarDayOfWeek>();
			foreach (var dayOfWeek in Enum.GetValues(typeof(CalendarDayOfWeek)))
				if (((byte)dayOfWeek & encodedDaysOfWeek) > 0)
					result.Add((CalendarDayOfWeek)dayOfWeek);
			return result;
		}

		public static CalendarDayOfWeek SystemDayOfWeekToCalendarDayOfWeek(DayOfWeek dayOfWeek)
		{
			var calendarDaysOfWeek = Enum.GetValues(typeof (CalendarDayOfWeek));
			return (CalendarDayOfWeek)calendarDaysOfWeek.GetValue((int)dayOfWeek);
		}
	}
}
