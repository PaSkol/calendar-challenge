using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
	public class Event
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public DateTime Start { get; set; }
		public DateTime Stop { get; set; }
		public bool FullDay { get; set; }
		public bool UserIsBusy { get; set; }
		public TimeZone TimeZone { get; set; }
		public User User { get; set; }
		public Repetition Repetition { get; set; }
		public IList<Alert> Alerts { get; set; }
	}
}
