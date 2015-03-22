using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Domain
{
	public class User : CalendarEntity
	{
		public User(string name)
		{
			Name = name;
		}

		public string Name { get; protected set; }
		public string Email { get; set; }
	}
}
