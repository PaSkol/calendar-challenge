using System;
using System.Collections.Generic;
using Calendar.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calendar.UnitTests
{
	[TestClass]
	public class DomainTests
	{
		[TestMethod]
		public void TestForQuickTemporaryNeeds()
		{
			//Arrange
			var date = new DateTime(2015, 03, 23);
			//Act
			var newDate = date.Add(new TimeSpan(367, 0, 0, 0));
			//Assert
			Assert.AreEqual(new DateTime(2016, 03, 24), newDate);
		}
	}
}
