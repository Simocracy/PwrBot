import re
from datetime import datetime
from collections import OrderedDict

from simocracy import flagConverter
from simocracy import wiki
from footballMatchAnalyzer.footballStatElement import FootballStatElement

class FootballMatch:
	"""
	Fußballspiel
	"""
	
	"""
	Team, dessen Statistik erstellt wird, Basierend auf FLAGGENKÜRZEL
	"""
	mainTeam = ["UNS"]

	"""
	Liste der Nachfolgerstaaten, FLAGGENKÜRZEL, Schema Vorgänger -> Aktueller Staat (bzw. Nachfolger)
	"""
	sucessor = []

	_tournament = ""
	@property
	def tournament(self):
		return self._tournament
	@tournament.setter
	def tournament(self, value):
		s = value.strip()
		self._Tournament = s.replace("-", "") if (len(s) < 3) else s

	date = datetime.min

	_city = ""
	@property
	def city(self):
		return self._city
	@city.setter
	def city(self, value):
		self._city = value.strip()

	_stadium = ""
	@property
	def stadium(self):
		return self._stadium
	@stadium.setter
	def stadium(self, value):
		self._stadium = value.strip()

	_homeTeam = ""
	@property
	def homeTeam(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._homeTeam
	@homeTeam.setter
	def homeTeam(self, value):
		self._homeTeam = value.strip().replace("'", "")

	_awayTeam = ""
	@property
	def awayTeam(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._awayTeam
	@awayTeam.setter
	def awayTeam(self, value):
		self._awayTeam = value.strip().replace("'", "")
		
	@property
	def opponentTeam(self):
		"""
		Gegnerteam für Sortierung nach Gegner
		"""
		for s in self.mainTeam:
			if s in self.awayTeam:
				return self.homeTeam
			return self.awayTeam
		
	"""
	-1 wenn kein Spielergebnis
	"""
	resultHome = ""

	"""
	-1 wenn kein Spielergebnis
	"""
	resultAway = ""

	@property
	def result(self):
		"""
		Vollständiges Spielergebnis
		"""
		if (self.resultHome < 0 or self.resultAway < 0):
			return "X"
		else:
			return str(self.resultHome) + ":" + str(self.resultAway)

	spectators = 0
	isSoldOut = False

	_referee = ""
	@property
	def referee(self):
		"""
		Inkl. Flaggenkürzel
		"""
		return self._referee
	@referee.setter
	def referee(self, value):
		self._referee = value.strip().replace("'", "")

	"""
	Quellcode im Wiki
	"""
	sourceCode = ""

	def __init__(self, tournament, date, city, stadium, homeTeam, awayTeam, result, spectators, soldOut, referee, source):
			self.tournament = tournament;
			self.setDate(date);
			self.city = city;
			self.stadium = stadium;
			self.homeTeam = homeTeam;
			self.awayTeam = awayTeam;
			self.setResults(result);
			self.setSpectators(spectators);
			self.SetIsSoldOut(soldOut);
			self.referee = referee;
			self.sourceCode = source;

	def setDate(self, date):
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
				self.date = parsedDate

		except:
			self.date = datetime.min
		

	def setResults(self, result):
		resPattern = r"(\d+):(\d+)"
		resMatch = re.match(resPattern, result)
		if not resMatch is None:
			self.resultHome = int(resMatch.group(1))
			self.resultAway = int(resMatch.group(2))
		else:
			self.resultHome = -1;
			self.resultAway = -1;

	def setSpectators(self, spectators):
		try:
			sp = spectators.replace(".","").replace("'","").replace(" ","")
			self.spectators = int(sp)
		except:
			self.spectators = 0

	def SetIsSoldOut(self, soldOut):
		self.isSoldOut = len(soldOut.strip()) > 0

def getMatchList(matches):
	"""
	Parsed die Matches in FootballMatch-Objekte
	matches (Match-Collection): Matches
	return: Liste mit FootballMatch
	"""
	matchList = []
	for match in matches:
		matchList.append(match)
	return matchList
	
def groupByOpponents(matches):
	"""
	Analysiert die Statistik und gibt diese nach Gegnern gruppiert und alphabetisch sortiert zurück
	matches (Enumerable<FootballMatch>): Spiele
	return (OrderedDict<string, FootballStatElement>): Statistik
	"""
	dic = {}
	flagTempRegex = re.compile(r"\{\{([^\|\}]*)(\|([^\}\|]*))?(\|([^\}\|]*))?\}\}")
	for match in matches:
		flagMatch = flagTempRegex.match(match.opponentTeam)

		flag = flagMatch.group(1)
		if len(flag.strip()) < 1:
			continue

		# {{GRA|Grafenberg}} -> Grafenberg
		if not flagMatch.group(3) is None and not "=" in flagMatch.group(3) and re.match(r"\d+", flagMatch.group(3)) is None:
			name = flagMatch.group(3)
		# {{GRA|b=20|Grafenberg}} -> Grafenberg
		elif not flagMatch.group(5) is None and not "=" in flagMatch.group(5) and re.match(r"\d+", flagMatch.group(5)) is None:
			name = flagMatch.group(5)
		# {{GRA}} -> "" -> {{GRA}} Grafenberg -> Grafenberg
		else:
			name = flagTempRegex.sub("", match.opponentTeam).strip()

		isNoneFlag = "?" in flag
		isDummyName = "#" in name
		flag = flagConverter.getFlag(name if isNoneFlag else flag)
		name = flagConverter.getStateName(flag if isDummyName else name)

		if not name in dic:
			fse = FootballStatElement(name, flag)
			dic[name] = fse
		dic[name].addMatch(match)

	sdic = OrderedDict(sorted(dic.items()))
	return sdic

def getOpponentTableCode(opponents):
	"""
	Ausgabe der Bilanz von MainTeam nach Gegnern
	opponents: Bilanz als OrderedDict<string, FootballStatElement>
	return: Bilanz als Wikicode

	"""
	text = ("=== Nach Gegner ===\n"
			"<html>"
			"{| class=\"wikitable sortable\" style=\"text-align:center;\"\n"
			"|-\n"
			"! Mannschaft\n"
			"! <abbr title=\"Spiele\">Sp.</abbr>\n"
			"! <abbr title=\"Siege\">S</abbr>\n"
			"! <abbr title=\"Unentschieden\">U</abbr>\n"
			"! <abbr title=\"Niederlagen\">N</abbr>\n"
			"! <abbr title=\"Tore\">T</abbr>\n"
			"! <abbr title=\"Gegentore\">GT</abbr>\n"
			"! <abbr title=\"Tordifferenz\">TD</abbr>\n"
			"! <abbr title=\"Punkte\">P</abbr>")
	for opp in opponents:
		if not opp.Flag in FootballMatch.mainTeam:
			text = str.format("{0}\n{1}", text, opp.opponentWikicode)

	text = str.format("{0}\n|}}\n<sup>Stand: <drechner eing=\"j\" day=\"j\">{1:%Y-%m-%d %H:%M}</drechner></sup>", text, datetime.datetime.now())

	return text

def sortForYears(matches):
	dic = {int:FootballStatElement}
	for match in machtes:
		if not match.date is datetime.min:
			if not match.date.year in dic:
				fse = FootballStatElement(match.date.year)
				dic[match.date.year] = fse
			dic[match.date.year].addMatch(match)

	sdic = OrderedDict(sorted(dic.items()))
	return sdic

def getYearTableCode(years):
	text = ("=== Nach Jahr ===\n"
			"{| class=\"wikitable sortable\" style=\"text-align:center;\"\n"
			"|-\n"
			"! Jahr\n"
			"! <abbr title=\"Spiele\">Sp.</abbr>\n"
			"! <abbr title=\"Siege\">S</abbr>\n"
			"! <abbr title=\"Unentschieden\">U</abbr>\n"
			"! <abbr title=\"Niederlagen\">N</abbr>\n"
			"! <abbr title=\"Tore\">T</abbr>\n"
			"! <abbr title=\"Gegentore\">GT</abbr>\n"
			"! <abbr title=\"Tordifferenz\">TD</abbr>\n"
			"! <abbr title=\"Punkte\">P</abbr>")

	allWon = 0;
	allDrawn = 0;
	allLose = 0;
	allGoalsFor = 0;
	allGoalsAgainst = 0;

	for year in years:
		allWon += year.win
		allDrawn += year.drawn
		allLose += year.lose
		allGoalsFor += year.goalsFor
		allGoalsAgainst += year.goalsAgainst

		text = str.format("{0}|n{1}", text, year.yearWikicode)

	played = allWon + allDrawn + allLose
	allGoalDiff = allGoalsFor - allGoalsAgainst
	points = allWon * 3 + allDrawn

	text = str.format("{0}\n|-\n! Ges. || {1} || {2} || {3} || {4} || {5} || {6} || {7:+0;-0;+0} || {8}\n|}}" +
				"\n<sup>Stand: <drechner eing=\"j\" day=\"j\">{9:%Y-%m-%d %H:%M}</drechner></sup>",
			text, played, allWon, allDrawn, allLose, allGoalsFor, allGoalsAgainst, allGoalDiff, points, datetime.datetime.now())

	return text

def analyseFootballStats(articleName, mainTeams):
	"""
	Analysiert und wertet die Statistik einer Fußballnationalmannschaft aus
	articleName: Artikel mit der Statistik
	mainTeams[]: Mainteams, auf die ausgewertet werden soll
	"""
	MainTeam = mainTeams

	"""
	Gruppen:
	1: Turnier
	2: Datum
	3: Ort
	4: Stadion
	5: Heimteam
	6: Gastteam
	7: Endergebnis (120 min)
	8: Zuschauer
	9: Wenn nicht leer: Ausverkauft
	10: Schiedsrichter
	"""
	matchRegexPat = r"\|(.*[^\|\r\n]*)\s*[\r\n|\|]\|\s*([^\|\r\n]*)?\s*[\r\n|\|]\|\s*(\{\{.+\}\}[^\|\r\n]*)[\r\n\|]\|\s*([^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}\{\{.+\}\}[^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}\{\{.+\}\}[^\|\r\n]*)?\s*[\r\n\|]\|\s*[^\|]*\|\s*'''([^']*)'''[^\|\r\n]*?\s*[\r\n\|]\|\s*([\d\.]*)(\s*\(a\))?\s*[\r\n\|]\|\s*(\{\{.*\}\}[^\|\r\n]*)?"

	print("Lade Artikel " + articleName)
	p = wiki.Article(articleName)
		
	# Abschnitte analysieren
	sectionMatches = re.match(r"(^={1,6}(.*)?={1,6}\s*?$)", p.text, re.RegexFlag.MULTILINE)
		
	counter = 0
	opponentSection = 0
	yearSection = 0
	for sectMatch in sectionMatches:
		counter += 1
		if "Nach Gegner" in sectMatch:
			opponentSection = counter
		elif "Nach Jahr" in sectMatch:
			yearSection = counter

	matchesRegex = re.match(matchRegexPat, p.text)
	footballMatchList = getMatchList(matchesRegex)
	print(len(footballMatchList) + " Spiele gefunden")

	# Gegnerstats
	if opponentSection > 0:
		return

	# Jahresstats
	if yearSection > 0:
		return