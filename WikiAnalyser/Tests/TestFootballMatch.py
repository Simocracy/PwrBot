import unittest

from datetime import datetime

from footballMatchAnalyzer.footballMatch import FootballMatch

class Test_FootballMatch(unittest.TestCase):
	def test_SetDateNumbers1(self):
		datestr = "04.04.44"
		defStr = ""
		match = FootballMatch(defStr, datestr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr)
		self.assertEquals(datetime.strptime("04.04.2044", "%d.%m.%Y"), match.date)

	def test_SetDateMonth1(self):
		datestr = "04.44"
		defStr = ""
		match = FootballMatch(defStr, datestr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr)
		self.assertEquals(datetime.strptime("04.2044", "%m.%Y"), match.date)
		
	def test_SetDateYear1(self):
		datestr = "44"
		defStr = ""
		match = FootballMatch(defStr, datestr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr)
		self.assertEquals(datetime.strptime("2044", "%Y"), match.date)

	def test_TestGrouping1(self):
		l = [FootballMatch("", "", "", "", "{{UNS}} UNAS", "{{GRA|#}}", "1:1", "", "", "", ""),
			FootballMatch("", "", "", "", "{{GRA}} Grafenberg", "{{UNS|#}}", "1:1", "", "", "", ""),
			FootballMatch("", "", "", "", "{{?}} New Halma Islands", "{{UNS}} UNAS", "1:1", "", "", "", "")]
		grp = FootballMatch.groupByOpponents(l)
		self.assertEquals(2, grp["Grafenberg"].played)
		self.assertEquals(1, grp["Neuseeland"].played)

if __name__ == '__main__':
	unittest.main()
