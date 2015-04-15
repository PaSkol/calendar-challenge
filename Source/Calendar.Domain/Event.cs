using System;
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
		/// <summary>
		/// Interval of time when event occurs
		/// </summary>
		public Interval Occurs { get; set; }
		public bool UserIsBusy { get; set; }
		public Visibility Visibility { get; set; }
		public TimeZone TimeZone { get; set; }
		public User User { get; set; }
		public Repetition Repetition { get; set; }
		public bool SingleOccurrence { get { return Repetition == null; }}
		public EventCommonData CommonData { get; set; }
		public IList<Alert> Alerts { get; set; }
		//TODO: implementation of common behaviors for splitted events
	}
}
