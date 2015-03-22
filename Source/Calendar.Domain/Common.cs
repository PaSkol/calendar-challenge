using System;

namespace Calendar.Domain
{
	/// <summary>
	/// Units for number of repetitions and for duration before event for alerts
	/// </summary>
	public enum TimeUnit
	{
		Minute = 1,
		Hour = 2,
		Day = 3,
		Week = 4,
		Month = 5,
		Year = 6
	}

	/// <summary>
	/// Available alert action
	/// </summary>
	public enum AlertAction
	{
		SendEmail,
		Popup
	}
}
