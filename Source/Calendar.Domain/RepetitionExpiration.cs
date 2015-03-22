using System;

namespace Calendar.Domain
{
	public class RepetitionExpiration
	{
		public RepetitionExpiration()
		{
			ContinueIndefinitely();	
		}

		public DateTime? OnDate { get; protected set; }
		public int AfterFixedNumberOfTimes { get; protected set; }
		public bool Never { get; protected set; }

		public void ContinueToDate(DateTime date)
		{
			Never = false;
			OnDate = date;
		}

		public void ContinueFixedNumberOfTimes(int fixedNumberOfTimes)
		{
			Never = false;
			AfterFixedNumberOfTimes = fixedNumberOfTimes;
		}

		public void ContinueIndefinitely()
		{
			Never = true;
		}

	}
}
