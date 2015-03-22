using System;

namespace Calendar.Domain
{
	public class Interval
	{
		public Interval(DateTime from, DateTime to)
		{
			From = from;
			To = to;
		}

		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public int FullDays { get { return To.Date.Subtract(From.Date).Days + 1; } }

	}

	/// <summary>
	/// Responsibles for calculating in exact data range length of slice of interval in full days and gives type of that slice
	/// </summary>
	public class IntervalSlice
	{
		public IntervalSlice(DateTime sliceFrom, DateTime sliceTo, Interval interval)
		{
			Kind = IntervalSliceKind.Complete;
			FullDays = 0;
			// slice is beyond interval
			if (interval.From.Date > sliceTo || interval.To.Date < sliceFrom)
				return;
			var from = interval.From;
			var to = interval.To;
			if (interval.To.Date > sliceTo)
			{
				Kind |= IntervalSliceKind.ToFuture; // |=>
				to = sliceTo;
			}
			if (interval.From.Date < sliceFrom)
			{
				Kind |= IntervalSliceKind.FromPast; // <=|
				from = sliceFrom;
			}
			FullDays = to.Date.Subtract(from.Date).Days + 1;
		}

		public int FullDays { get; private set; }
		public IntervalSliceKind Kind { get; private set; }
	}
}
