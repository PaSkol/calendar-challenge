using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
	public class Alert
	{
		public Event Event { get; set; }
		public int RaiseBefore { get; set; }
		public TimeUnit TimeUnit { get; set; }
		public AlertAction Action { get; set; }
	}
}
