using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Calendar.Domain
{
	public class Repetition : CalendarEntity
	{
		/// <summary>
		/// Constructor for month repetition
		/// </summary>
		/// <param name="period">repeat after than number of months</param>
		/// <param name="when">uses day of date when event has to be repated or uses as base for calculation of exact day of exact week in month (depends on <see cref="transformToDayOfWeek"/>)</param>
		/// <param name="transformToDayOfWeek">calculate <see cref="when"/> parameter to exact day of exact week in month</param>
		public Repetition(int period, DateTime when, bool transformToDayOfWeek)
			: this(TimeUnit.Month, period, transformToDayOfWeek ? new List<DayOfWeek>() { when.DayOfWeek } : null)
		{
			if (transformToDayOfWeek)
			{
				var date = new DateTime(when.Year, when.Month, 1);
				date = date.AddMonths(1).Subtract(new TimeSpan(7 + 1, 0, 0, 0));
				if (when > date)
					ExactDayOfWeekInMonth = ExactDayOfWeekInMonth.Last;
				else
					ExactDayOfWeekInMonth = (ExactDayOfWeekInMonth)GetWeekOfMonth(when);
			}
			else
			{
				ExactDayAndOptionalMonth = new DayAndMonth(when.Day);
				ExactDayOfWeekInMonth = ExactDayOfWeekInMonth.None;
			}
		}

		/// <summary>
		/// Constructor for year repetition
		/// </summary>
		/// <param name="period">repeat after than number of years</param>
		/// <param name="when">uses day and month of date when event has to be repated</param>
		public Repetition(int period, DateTime when)
			: this(TimeUnit.Year, period, null)
		{
			ExactDayAndOptionalMonth = new DayAndMonth(when.Day, when.Month);
		}

		/// <summary>
		/// Constructor for week repetition
		/// </summary>
		/// <param name="period">repeat after than number of weeks</param>
		/// <param name="daysOfWeek">days of week, when event has to occure</param>
		public Repetition(int period, IEnumerable<DayOfWeek> daysOfWeek)
			: this(TimeUnit.Week, period, daysOfWeek ?? new List<DayOfWeek>() { DateTime.Now.DayOfWeek })
		{
			ExactDayOfWeekInMonth = ExactDayOfWeekInMonth.None;
		}

		protected Repetition(TimeUnit periodUnit, int period, IEnumerable<DayOfWeek> daysOfWeek)
		{
			PeriodUnit = periodUnit;
			Period = period;
			Expiration = new RepetitionExpiration();
			EncodedDaysOfWeek = (daysOfWeek == null ? (byte)0 : EncodeDaysOfWeek(daysOfWeek));
		}

		public int Period { get; set; }
		public TimeUnit PeriodUnit { get; set; }
		/// <summary>
		/// Required for <see cref="PeriodUnit"/> equal <see cref="TimeUnit.Week"/> or <see cref="TimeUnit.Month"/>
		/// </summary>
		protected byte EncodedDaysOfWeek { get; set; }
		/// <summary>
		/// Required for <see cref="PeriodUnit"/> equal <see cref="TimeUnit.Month"/>
		/// </summary>
		public ExactDayOfWeekInMonth ExactDayOfWeekInMonth { get; set; }
		/// <summary>
		/// Required for <see cref="PeriodUnit"/> equal <see cref="TimeUnit.Month"/> (uses day only) or <see cref="TimeUnit.Year"/> (uses day and month)
		/// </summary>
		public DayAndMonth ExactDayAndOptionalMonth { get; set; }
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
				? CalculateStopDateFromNumberOfTimes(startDate, Expiration.AfterFixedNumberOfTimes)
				: Expiration.OnDate;
		}

		//TODO: practically stop date can be calculate once - in constructor, we only need pass start date there and here only return stored result of calculation
		private DateTime CalculateStopDateFromNumberOfTimes(DateTime startDate, int numberOfTimes)
		{
			//TODO: include other options (daily, monthly, yearly)
			var date = startDate;
			switch (PeriodUnit)
			{
				case TimeUnit.Day:
					break;
				case TimeUnit.Week:
					var timesOnWeek = OnCertainDaysOfWeek.Count();
					var daysFromWeeks = (int)Math.Floor((double) numberOfTimes/timesOnWeek)*7*Period + numberOfTimes%timesOnWeek;
					//TODO: don't forget that last day of event is exact day of week - that the stop date is
					//TODO: also don't forget, that sunday can be begin of week, so it can bee before of rest days of week, and one of these will be last day of event
					return startDate.Date.Add(new TimeSpan(daysFromWeeks, 0, 0, 0));
				case TimeUnit.Month:
					date = date.AddMonths(Period * numberOfTimes);
					if (ExactDayOfWeekInMonth == ExactDayOfWeekInMonth.None)
						date = new DateTime(date.Year, date.Month, ExactDayAndOptionalMonth.Day);
					else
					{
						date = new DateTime(date.Year, date.Month, 1);
						// TODO: complete implementation (maybe in constructor, as suggested above, on begin of method)
					}
					return date;
				case TimeUnit.Year:
					date = date.AddYears(Period*numberOfTimes);
					date = new DateTime(date.Year, ExactDayAndOptionalMonth.Month, ExactDayAndOptionalMonth.Day);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return date;
		}

		private static int GetWeekOfMonth(DateTime time)
		{
			var first = new DateTime(time.Year, time.Month, 1);
			return GetWeekOfYear(time) - GetWeekOfYear(first) + 1;
		}

		private static GregorianCalendar calendar;
		private static int GetWeekOfYear(DateTime time)
		{
			calendar = calendar ?? new GregorianCalendar();
			return calendar.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
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
