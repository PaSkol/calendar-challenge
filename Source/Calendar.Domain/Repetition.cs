using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Calendar.Domain
{
	public class Repetition : CalendarEntity
	{
		protected Repetition(TimeUnit periodUnit, DateTime firstOccurrence, int period, IEnumerable<DayOfWeek> daysOfWeek)
		{
			FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
			FirstOccurrence = firstOccurrence;
			PeriodUnit = periodUnit;
			Period = period;
			Expiration = new RepetitionExpiration();
			EncodedDaysOfWeek = (daysOfWeek == null ? (byte)0 : EncodeDaysOfWeek(daysOfWeek));
		}

		/// <summary>
		/// Constructor for month repetition
		/// </summary>
		/// <param name="firstOccurrence">first occurence of repetition (and first occurence of event)</param>
		/// <param name="period">repeat after than number of months</param>
		/// <param name="when">uses day of date when event has to be repated or uses as base for calculation of exact day of exact week in month (depends on <see cref="transformToDayOfWeek"/>)</param>
		/// <param name="transformToDayOfWeek">calculate <see cref="when"/> parameter to exact day of exact week in month</param>
		public Repetition(DateTime firstOccurrence, int period, DateTime when, bool transformToDayOfWeek)
			: this(TimeUnit.Month, firstOccurrence, period, transformToDayOfWeek ? new List<DayOfWeek>() { when.DayOfWeek } : null)
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
		/// <param name="firstOccurrence">first occurence of repetition (and first occurence of event)</param>
		/// <param name="period">repeat after than number of years</param>
		/// <param name="when">uses day and month of date when event has to be repated</param>
		public Repetition(DateTime firstOccurrence, int period, DateTime when)
			: this(TimeUnit.Year, firstOccurrence, period, null)
		{
			ExactDayAndOptionalMonth = new DayAndMonth(when.Day, when.Month);
		}

		/// <summary>
		/// Constructor for week repetition
		/// </summary>
		/// <param name="firstOccurrence">first occurence of repetition (and first occurence of event)</param>
		/// <param name="period">repeat after than number of weeks</param>
		/// <param name="daysOfWeek">days of week, when event has to occure</param>
		public Repetition(DateTime firstOccurrence, int period, IEnumerable<DayOfWeek> daysOfWeek)
			: this(TimeUnit.Week, firstOccurrence, period, daysOfWeek ?? new List<DayOfWeek>() { DateTime.Now.DayOfWeek })
		{
			ExactDayOfWeekInMonth = ExactDayOfWeekInMonth.None;
		}

		public DayOfWeek FirstDayOfWeek { get; set; }
		public DateTime FirstOccurrence { get; private set; }
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
		protected DayAndMonth ExactDayAndOptionalMonth { get; set; }
		public RepetitionExpiration Expiration { get; private set; }

		public IEnumerable<DayOfWeek> OnCertainDaysOfWeek
		{
			get { return DecodeDaysOfWeek(EncodedDaysOfWeek); }
			set { EncodeDaysOfWeek(value); }
		}

		public DateTime? CalculateExpirationDate()
		{
			if (Expiration.Never)
				return null;
			return Expiration.AfterFixedNumberOfTimes > 0
				? CalculateStopDateFromNumberOfTimes(FirstOccurrence, Expiration.AfterFixedNumberOfTimes)
				: Expiration.OnDate;
		}

		private int CalculateMaxDayNumberFromDaysOfWeekInWeekRange()
		{
			// 1 2 3 4 5 6 7
			//     3
			return 0;
		}

		//TODO: practically stop date can be calculate once - in constructor, we only need pass start date there and here only return stored result of calculation
		private DateTime CalculateStopDateFromNumberOfTimes(DateTime startDate, int numberOfTimes)
		{
			//TODO: include other options (daily, monthly, yearly)
			var date = startDate;
			switch (PeriodUnit)
			{
				case TimeUnit.Day:
					date = date.AddDays(Period*numberOfTimes);
					break;
				case TimeUnit.Week:
					var timesOnWeek = OnCertainDaysOfWeek.Count();
					var weeks = (int)Math.Floor((double)numberOfTimes / timesOnWeek) * Period;
					var days = weeks*7 + numberOfTimes%timesOnWeek;
					//TODO: don't forget that last day of event is exact day of week - that the stop date is
					//TODO: also don't forget, that sunday can be begin of week, so it can bee before of rest days of week, and one of these will be last day of event
					return startDate.Date.AddDays(days);
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
