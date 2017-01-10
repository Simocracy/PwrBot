from datetime import datetime

from FootballMatchAnalyser.FootballStatElement import FootballStatElement

class FootbalMatch:
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
	@property.setter
	def Tournament(self, value):
		s = value.strip()
		self._Tournament = s.replace("-", "") if (len(s) < 3) else s

	Date = ""

	_City = ""
	@property
	def City(self):
		return self._City
	@property.setter
	def City(self, value):
		self._City = value.strip()

	_Stadium = ""
	@property
	def Stadium(self):
		return self._Stadium
	@property.setter
	def Stadium(self, value):
		self._Stadium = value.strip()

	_HomeTeam = ""
	@property
	def HomeTeam(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._HomeTeam
	@property.setter
	def HomeTeam(self, value):
		self._HomeTeam = value.strip().replace("'", "")

	_AwayTeam = ""
	@property
	def AwayTeam(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._AwayTeam
	@property.setter
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
	@property.setter
	def Referee(self, value):
		self._Referee = value.strip().replace("'", "")

	"""
	Quellcode im Wiki
	"""
	SourceCode = ""

	def __init__(self, tournament, date, city, stadium, homeTeam, awayTeam, result, spectators, soldOut, referee, source):
			Tournament = tournament;
			SetDate(date);
			City = city;
			Stadium = stadium;
			HomeTeam = homeTeam;
			AwayTeam = awayTeam;
			SetResults(result);
			SetSpectators(spectators);
			SetIsSoldOut(soldOut);
			Referee = referee;
			SourceCode = source;

	def SetDate(self, date):
		if len(date.strip()) > 0:
			Date = datetime.strptime(date, '%b %d %Y %I:%M%p')

	def SetResults(self, result):
		return 0

	def SetSpectators(self, spectators):
		try:
			sp = spectators.replace(".","").replace("'","").replace(" ","")
			Spectators = int(sp)
		except:
			Spectators = 0

	def SetIsSoldOut(self, soldOut):
		IsSoldOut = len(soldOut.strip()) > 0