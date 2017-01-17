class FootballStatElement:
	"""Gesamtstatistik für einen Gegner"""

	name = ""
	flag = ""
	year = 0
	matches = []
	preComment = "";
	postComment = "";
	
	@property
	def played(self):
		return self.win + self.drawn + self.lose

	win = 0
	drawn = 0
	lose = 0
	goalsFor = 0
	goalsAgainst = 0

	@property
	def goalsDiff(self):
		return self.goalsFor - self.goalsAgainst

	@property
	def points(self):
		return self.win * 3 + self.drawn

	@property
	def balance(self):
		return self.win - self.lose

	@property
	def colorCode(self):
		return "CCFFCC" if self.balance > 0 else "FFCCCC" if self.balance < 0 else "FFFFCC"
		
	@property
	def opponentWikicode(self):
		return "|- style=\"background:#{0};\"\n| style=\"text-align:left;\" | {1}\n| {2} || {3} || {4} || {5} || {6} || {7} || {8:+d} || {9}".format(
			self.colorCode, self.flag, self.played, self.win, self.drawn, self.lose, self.goalsFor, self.goalsAgainst, self.goalsDiff, self.points)

	@property
	def yearWikicode(self):
		return "|-\n| '''{0}''' || {1} || {2} || {3} || {4} || {5} || {6} || {7:+d} || {8}".format(
			self.year, self.played, self.win, self.drawn, self.lose, self.goalsFor, self.goalsAgainst, self.goalsDiff, self.points)

	def __init__(self, *args, **kwargs):
		"""
		Erstellt neues FootballStatElement
		year (int): Jahr
		name (String): Staatsname
		flag (String): Flaggenkürzel
		"""
		if len(args) == 1:
			self.initFromYear(args[0])
		elif len(args) == 2:
			self.initFromOpponent(args[0], args[1])

	def initFromYear(self, year):
		self.year = year

	def initFromOpponent(self, name, flag):
		self.name = name
		self.flag = str.format("{{{{{0}|#}}}}", flag)
		self.matches = []

	def addMatch(self, match):
		if match.result != "X":
			self.matches.append(match)
			if match.homeTeam == match.opponentTeam:
				if match.resultHome < match.resultAway:
					self.win += 1
				elif match.resultHome > match.resultAway:
					self.lose += 1
				else:
					self.drawn += 1
				self.goalsFor += match.resultAway;
				self.goalsAgainst += match.resultHome
			else:
				if match.resultHome > match.resultAway:
					self.win += 1
				elif match.resultHome < match.resultAway:
					self.lose += 1
				else:
					self.drawn += 1
				self.goalsFor += match.resultHome;
				self.goalsAgainst += match.resultAway