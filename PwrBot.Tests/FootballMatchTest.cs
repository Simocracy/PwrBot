// <copyright file="FootballMatchTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simocracy.PwrBot;

namespace Simocracy.PwrBot.Tests
{
	/// <summary>Diese Klasse enthält parametrisierte Komponententests für FootballMatch.</summary>
	[PexClass(typeof(FootballMatch))]
	[PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
	[PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
	[TestClass]
	public partial class FootballMatchTest
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
	}
}
