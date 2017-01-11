class FootballStatElement:
	"""Gesamtstatistik für einen Gegner"""

	Name = ""
	Flag = ""
	Year = 0
	Matches = []
	PreComment = "";
	PostComment = "";
	
	@property
	def Played(self):
		return self.Win + self.Drawn + self.Lose

	Win = 0
	Drawn = 0
	Lose = 0
	GoalsFor = 0
	GoalsAgainst = 0
	GoalsDIff = 0

	@property
	def GoalsDiff(self):
		return self.GoalsFor - self.GoalsAgainst

	@property
	def Points(self):
		return self.Win * 3 + self.Drawn

	@property
	def Balance(self):
		return self.Win - self.Lose

	def AddMatch(self, match):
		if match.Result != "X":
			self.Matches.append(match)
			if match.HomeTeam == match.OpponentTeam:
				if match.ResultHome < match.ResultAway:
					self.Win += 1
				elif match.ResultHome > match.ResultAway:
					self.Lose += 1
				else:
					self.Drawn += 1
				self.GoalsFor += match.ResultAway;
				self.GoalsAgainst += match.ResultHome
			else:
				if match.ResultHome > match.ResultAway:
					self.Win += 1
				elif match.ResultHome < match.ResultAway:
					self.Lose += 1
				else:
					self.Drawn += 1
				self.GoalsFor += match.ResultHome;
				self.GoalsAgainst += match.ResultAway

	def __init__(self, name, flag):
		"""
		Erstellt neues FootballStatElement
		name (String): Staatsname
		flag (String): Flaggenkürzel
		"""
		self.Name = name
		self.Flag = str.format("{{{{{0}|#}}}}", flag)
		self.Matches = []

	def GetOpponentWikicode(self):
		return "|- class=\"{0}\"\n| style=\"text-align:left;\" | {1}\n| {2} || {3} || {4} || {5} || {6} || {7} || {8:+0;-0;+0} || {9}".format(
			"s" if Balance > 0 else "n" if Balance < 0 else "u",
			Flag, Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst, GoalDiff, Points)

	def GetYearWikicode(self):
		return "|-\n| '''{0}''' || {1} || {2} || {3} || {4} || {5} || {6} || {7:+0;-0;+0} || {8}".format(
			Year if Year > 1930 else "N/A",
			Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst, GoalDiff, Points)