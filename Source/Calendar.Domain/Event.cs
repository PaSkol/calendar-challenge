﻿using System;
using System.Collections.Generic;

namespace Calendar.Domain
{
	public class Event
	{
		public Event(User user)
		{
			User = user;
		}

		public string Title { get; set; }
		public string Description { get; set; }
		public string Location { get; set; }
		public Interval Interval { get; set; }
		public bool UserIsBusy { get; set; }
		public Visibility Visibility { get; set; }
		public TimeZone TimeZone { get; set; }
		public User User { get; set; }
		public Repetition Repetition { get; set; }
		public IList<Alert> Alerts { get; set; }
		//TODO: implementation of common behaviors for splitted events
	}
}
