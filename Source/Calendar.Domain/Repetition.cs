using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendar.Domain
{
	public class Repetition : CalendarEntity
	{
		public Repetition(TimeUnit periodUnit, int period, IEnumerable<DayOfWeek> daysOfWeek = null)
			: this(daysOfWeek)
		{
			Expiration = new RepetitionExpiration();
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
		public RepetitionExpiration Expiration { get; private set; }

		public IEnumerable<DayOfWeek> OnCertainDaysOfWeek
		{
			get { return DecodeDaysOfWeek(EncodedDaysOfWeek); }
			set { EncodeDaysOfWeek(value); }
		}

		public DateTime? CalculateExpirationDate(DateTime startDate)
		{
			if (Expiration.Never)
				return null;
			return Expiration.AfterFixedNumberOfTimes > 0
				? CalculateDateFromNumberOfTimes(startDate, Expiration.AfterFixedNumberOfTimes)
				: Expiration.OnDate;
		}

		private DateTime CalculateDateFromNumberOfTimes(DateTime startDate, int numberOfTimes)
		{
			//TODO: include other options (daily, monthly, yearly)
			var result = 0;
			switch (PeriodUnit)
			{
				case TimeUnit.Day:
					break;
				case TimeUnit.Week:
					var timesOnWeek = OnCertainDaysOfWeek.Count();
					result = (int)Math.Floor((double) numberOfTimes/timesOnWeek)*7*Period + numberOfTimes%timesOnWeek;
					break;
				case TimeUnit.Month:
					break;
				case TimeUnit.Year:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return startDate.Date.Add(new TimeSpan(result, 0, 0, 0));
		}

		public void ContinueToDate(DateTime date)
		{
			Expiration.ContinueToDate(date);
		}

		public void ContinueFixedNumberOfTimes(int fixedNumberOfTimes)
		{
			Expiration.ContinueFixedNumberOfTimes(fixedNumberOfTimes);
		}

		public void ContinueIndefinitely()
		{
			Expiration.ContinueIndefinitely();
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
