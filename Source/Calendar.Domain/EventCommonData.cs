using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
	public class EventCommonData
	{
		/// <summary>
		/// one and only common property corresponding to Google calendar
		/// </summary>
		public ConsoleColor PresentationColor { get; set; }
		/// <summary>
		/// Only for demonstration of common data
		/// </summary>
		public string CommonTextData { get; set; }
		/// <summary>
		/// Only for demonstration of common data
		/// </summary>
		public DateTime CommonDateData { get; set; }
	}
}
