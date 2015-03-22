using System;
using System.Collections.Generic;

namespace Calendar.Domain
{
	public class Repetition : CalendarEntity
	{
		public Repetition(TimeUnit periodUnit, int period, IEnumerable<DayOfWeek> daysOfWeek = null)
			: this(daysOfWeek)
		{
			PeriodUnit = periodUnit;
			Period = period;
		}

		private Repetition(IEnumerable<DayOfWeek> daysOfWeek)
		{
			EncodedDaysOfWeek = EncodeDaysOfWeek(daysOfWeek ?? new List<DayOfWeek>() { DateTime.Now.DayOfWeek });
		}

		public int Period { get; set; }
		public TimeUnit PeriodUnit { get; set; }
		protected byte EncodedDaysOfWeek { get; set; }
		public DateTime? ContinueToDate { get; set; }
		public int ContinueFixedNumberOfTimes { get; set; }
		public bool ContinueIndefinitely { get; set; }

		public IEnumerable<DayOfWeek> OnCertainDaysOfWeek
		{
			get { return DecodeDaysOfWeek(EncodedDaysOfWeek); }
			set { EncodeDaysOfWeek(value); }
		}

		private static byte EncodeDaysOfWeek(IEnumerable<DayOfWeek> daysOfWeek)
		{
			var result = 0;
			foreach (var dayOfWeek in daysOfWeek)
				result |= 1 << (byte)dayOfWeek;
			return (byte)result;
		}

		private static IEnumerable<DayOfWeek> DecodeDaysOfWeek(byte encodedDaysOfWeek)
		{
			var result = new List<DayOfWeek>();
			for (var i = (int)DayOfWeek.Sunday; i <= (int)DayOfWeek.Saturday; i++)
				if ((encodedDaysOfWeek & (1 << i)) > 0)
					result.Add((DayOfWeek) i);
			return result;
		}
	}
}
