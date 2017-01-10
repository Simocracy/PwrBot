import re
from datetime import datetime

from FootballMatchAnalyser import FootballStatElement

class FootballMatch:
	"""
	Fußballspiel
	"""


	"""
	Team, dessen Statistik erstellt wird, Basierend auf FLAGGENKÜRZEL
	"""
	MainTeam = ["UNAS"]

	"""
	Liste der Nachfolgerstaaten, FLAGGENKÜRZEL, Schema Vorgänger -> Aktueller Staat (bzw. Nachfolger)
	"""
	Sucessor = []

	_Tournament = ""
	@property
	def Tournament(self):
		return self._Tournament
	@Tournament.setter
	def Tournament(self, value):
		s = value.strip()
		self._Tournament = s.replace("-", "") if (len(s) < 3) else s

	Date = datetime.min

	_City = ""
	@property
	def City(self):
		return self._City
	@City.setter
	def City(self, value):
		self._City = value.strip()

	_Stadium = ""
	@property
	def Stadium(self):
		return self._Stadium
	@Stadium.setter
	def Stadium(self, value):
		self._Stadium = value.strip()

	_HomeTeam = ""
	@property
	def HomeTeam(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._HomeTeam
	@HomeTeam.setter
	def HomeTeam(self, value):
		self._HomeTeam = value.strip().replace("'", "")

	_AwayTeam = ""
	@property
	def AwayTeam(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._AwayTeam
	@AwayTeam.setter
	def AwayTeam(self, value):
		self._AwayTeam = value.strip().replace("'", "")
		
	@property
	def OpponentTeam(self):
		"""
		Gegnerteam für Sortierung nach Gegner
		"""
		for s in MainTeam:
			if s in AwayTeam:
				return HomeTeam
			return AwayTeam
		
	"""
	-1 wenn kein Spielergebnis
	"""
	ResultHome = ""

	"""
	-1 wenn kein Spielergebnis
	"""
	ResultAway = ""

	"""
	Vollständiges Spielergebnis
	"""
	@property
	def Result(self):
		if (ResultHome < 0 or ResultAway < 0):
			return "X"
		else:
			return ResultHome + ":" + ResultAway

	Spectators = 0
	IsSoldOut = False

	_Referee = ""
	@property
	def Referee(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._Referee
	@Referee.setter
	def Referee(self, value):
		self._Referee = value.strip().replace("'", "")

	"""
	Quellcode im Wiki
	"""
	SourceCode = ""

	def __init__(self, tournament, date, city, stadium, homeTeam, awayTeam, result, spectators, soldOut, referee, source):
			self.Tournament = tournament;
			self.SetDate(date);
			self.City = city;
			self.Stadium = stadium;
			self.HomeTeam = homeTeam;
			self.AwayTeam = awayTeam;
			self.SetResults(result);
			self.SetSpectators(spectators);
			self.SetIsSoldOut(soldOut);
			self.Referee = referee;
			self.SourceCode = source;

	def SetDate(self, date):
		try:
			exactDatePattern = r"((\d{1,2})\.)?((\d{1,2})\.)?(\d{2,4})"
			exactDateMatch = re.match(exactDatePattern, date)
			if not exactDateMatch is None:
				matchStr = exactDateMatch.group(0)
				if len(exactDateMatch.group(5)) < 4:
					matchStr = matchStr[:len(matchStr)-2] + "20" + matchStr[len(matchStr)-2:]
				if exactDateMatch.group(2) is None:
					datepstr = "%Y"
				elif exactDateMatch.group(4) is None:
					datepstr = "%m.%Y"
				else:
					datepstr = "%d.%m.%Y"
				parsedDate = datetime.strptime(matchStr, datepstr)
				self.Date = parsedDate

		except:
			self.Date = datetime.min
		

	def SetResults(self, result):
		resPattern = r"(\d+):(\d+)"
		resMatch = re.match(resPattern, result)
		if not resMatch is None:
			self.ResultHome = int(resMatch.group(1))
			self.ResultAway = int(resMatch.group(2))
		else:
			self.ResultHome = -1;
			self.ResultAway = -1;

	def SetSpectators(self, spectators):
		try:
			sp = spectators.replace(".","").replace("'","").replace(" ","")
			self.Spectators = int(sp)
		except:
			self.Spectators = 0

	def SetIsSoldOut(self, soldOut):
		self.IsSoldOut = len(soldOut.strip()) > 0