using System;

namespace Calendar.Domain
{
	/// <summary>
	/// Days of week encoded in one byte as separated bits combination (eq. monday and tuesday give 0x06)
	/// </summary>
	[Flags]
	public enum CalendarDayOfWeek : byte
	{
		Sunday = 0x01,
		Monday = 0x02,
		Tuesday = 0x04,
		Wednesday = 0x08,
		Thursday = 0x10,
		Friday = 0x20,
		Saturday = 0x40,
	}

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
