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
		return Win + Drawn + Lose

	Win = 0
	Drawn = 0
	Lose = 0
	GoalsFor = 0
	GoalsAgainst = 0
	GoalsDIff = 0

	@property
	def GoalsDiff(self):
		return GoalsFor - GoalsAgainst

	@property
	def Points(self):
		return Win * 3 + Drawn

	@property
	def Balance(self):
		if Win > Lose:
			return 1
		elif Win < Lose:
			return -1
		else:
			return 0

	def AddMatch(self, match):
		if match.Result != "X":
			Matches.append(match)
			if match.HomeTeam == match.OpponentTeam:
				if match.ResultHome < match.ResultAway:
					+Win
				elif match.ResultHome > match.ResultAway:
					+Lose
				else:
					+Drawn
				GoalsFor += match.ResultAway;
				GoalsAgainst += match.ResultHome
			else:
				if match.ResultHome > match.ResultAway:
					+Win
				elif match.ResultHome < match.ResultAway:
					+Lose
				else:
					+Drawn
				GoalsFor += match.ResultHome;
				GoalsAgainst += match.ResultAway

	def GetOpponentWikicode(self):
		return "|- class=\"{0}\"\n| style=\"text-align:left;\" | {1}\n| {2} || {3} || {4} || {5} || {6} || {7} || {8:+0;-0;+0} || {9}".format(
			"s" if Balance > 0 else "n" if Balance < 0 else "u",
			Flag, Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst, GoalDiff, Points)

	def GetYearWikicode(self):
		return "|-\n| '''{0}''' || {1} || {2} || {3} || {4} || {5} || {6} || {7:+0;-0;+0} || {8}".format(
			Year if Year > 1930 else "N/A",
			Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst, GoalDiff, Points)