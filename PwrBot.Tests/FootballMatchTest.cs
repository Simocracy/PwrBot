﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simocracy.PwrBot;

namespace PwrBot.Tests
{
	[TestClass]
	public class FootballMatchTest
	{
		[TestMethod]
		public void TestSetDateNumbers()
		{
			var datesStr = "04.04.44";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 4, 4), fm.Date);
		}

		[TestMethod]
		public void TestSetDateMonth()
		{
			var datesStr = "04.44";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 4, 1), fm.Date);
		}

		[TestMethod]
		public void TestSetDateYear()
		{
			var datesStr = "44";
			var defStr = String.Empty;
			var fm = new FootballMatch(defStr, datesStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr);
			Assert.AreEqual(new DateTime(2044, 1, 1), fm.Date);
		}
	}
}
