﻿using System;

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

	/// <summary>
	/// Visibility of event for other users
	/// </summary>
	public enum Visibility
	{
		Default,
		Private,
		Public
	}

	/// <summary>
	/// Describes how to interpret (eq. for present) slice of interval (see graphic apperance of events in Google calendar)
	/// </summary>
	[Flags]
	public enum IntervalSliceKind : byte
	{
		Complete =  0x00, // |##|
		FromPast = 0x01, // <##|
		ToFuture = 0x02, // |##>
		Between = 0x03, // <##>
	}

	public enum ExactDayOfWeekInMonth : sbyte
	{
		None = 0,
		First = 1,
		Second = 2,
		Third = 3,
		Fourth = 4,
		Last = -1
	}

	public class DayAndMonth
	{
		public DayAndMonth(int day, int month = 0)
		{
			Day = (byte) day;
			Month = (byte) month;
		}

		public byte Day { get; set; }
		public byte Month { get; set; }
	}
}
