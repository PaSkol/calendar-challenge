using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
	/// <summary>
	/// Dni tygodnia w konwencji umożliwiającej kodowanie ich na pojedynczym bajcie poprzez złożenie bitów (np. pn i wt dadzą 0x06)
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

	public enum TimeUnit
	{
		Minute = 1,
		Hour = 2,
		Day = 3,
		Week = 4,
		Month = 5,
		Year = 6
	}

	public enum AlertAction
	{
		SendEmail,
		Popup
	}
}
