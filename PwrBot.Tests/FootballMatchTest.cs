using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simocracy.PwrBot;

namespace PwrBot.Tests
{
	[TestClass]
	public class FootballMatchTest
	{

		[TestMethod]
		public void TestSetDateNumbers1()
		{
			var datesStr = "04.04.44";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 4, 4), fm.Date);
		}

		[TestMethod]
		public void TestSetDateNumbers2()
		{
			var datesStr = "04.04.2044";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 4, 4), fm.Date);
		}

		[TestMethod]
		public void TestSetDateMonth1()
		{
			var datesStr = "04.44";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 4, 1), fm.Date);
		}

		[TestMethod]
		public void TestSetDateMonth2()
		{
			var datesStr = "04.2044";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 4, 1), fm.Date);
		}

		[TestMethod]
		public void TestSetDateYear1()
		{
			var datesStr = "44";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 1, 1), fm.Date);
		}

		[TestMethod]
		public void TestSetDateYear2()
		{
			var datesStr = "2044";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 1, 1), fm.Date);
		}

		[TestMethod]
		public void TestGrouping1()
		{
			var l = new List<FootballMatch>
			{
				new FootballMatch(String.Empty, String.Empty, String.Empty, String.Empty, "{{UNS}} UNAS", "{{GRA|#}}", "1:1",
					String.Empty, String.Empty, String.Empty, String.Empty),
				new FootballMatch(String.Empty, String.Empty, String.Empty, String.Empty, "{{GRA}} Grafenberg", "{{UNS|#}}",
					"1:1", String.Empty, String.Empty, String.Empty, String.Empty),
				new FootballMatch(String.Empty, String.Empty, String.Empty, String.Empty, "{{?}} New Halma Islands", "{{UNS}} UNAS",
					"1:1", String.Empty, String.Empty, String.Empty, String.Empty)
			};
			var grp = FootballMatch.SortForOpponents(l);

			Assert.AreEqual(grp["Grafenberg"].Played, 2);
			Assert.AreEqual(grp["Neuseeland"].Played, 1);
		}
	}
}
