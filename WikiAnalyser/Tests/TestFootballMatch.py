import unittest

from datetime import datetime

from FootballMatchAnalyser.FootballMatch import FootballMatch

class Test_FootballMatch(unittest.TestCase):
	def test_SetDateNumbers1(self):
		datestr = "04.04.44"
		defStr = ""
		match = FootballMatch(defStr, datestr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr)
		self.assertEquals(datetime.strptime("04.04.2044", "%d.%m.%Y"), match.Date)

	def test_SetDateMonth1(self):
		datestr = "04.44"
		defStr = ""
		match = FootballMatch(defStr, datestr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr)
		self.assertEquals(datetime.strptime("04.2044", "%m.%Y"), match.Date)
		
	def test_SetDateYear1(self):
		datestr = "44"
		defStr = ""
		match = FootballMatch(defStr, datestr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr, defStr)
		self.assertEquals(datetime.strptime("2044", "%Y"), match.Date)

if __name__ == '__main__':
	unittest.main()
