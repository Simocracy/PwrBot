import sys
import os
import configparser
sys.path.append(os.path.abspath("../simocraPy/"))

from simocracy import wiki
from footballMatchAnalyzer import footballMatch

def main():
	config = configparser.ConfigParser()
	config.read("pwrbot.cfg")

	# read wiki login
	wikiSect = "WIKI"
	userOpt = "username"
	pwOpt = "password"
	if not config.has_option(wikiSect, userOpt) and not config.has_option(wikiSect, pwOpt):
		print("No wiki login data found. Using simocraPy credentials.")
		wiki.login()
	else:
		username = config[wikiSect][userOpt]
		password = config[wikiSect][pwOpt]
		wiki.login(username, password)

	# read football stats settings
	statSect = "FOOTBALLSTATS"
	articleOpt = "article"
	mainteamOpt = "teamflags"
	if config.has_section(statSect):
		optLength = len(config.items(statSect))
		if optLength % 2 != 0:
			print("Wrong option count in section" + statSect)
			return
		for i in range(0, optLength//2):
			art = config[statSect][articleOpt+str(i)]
			team = config[statSect][mainteamOpt+str(i)].split(',')

			print("Search for " + str(team) + " in article " + art)
			footballMatch.analyseFootballStats(art, team)

	"""
	articleName = "Statistik der UNAS-Fußballnationalmannschaft der Herren"
	mainTeams = ["UNS", "UANS", "VSB", "AME", "CDO", "RIV"]

	footballMatch.analyseFootballStats(articleName, mainTeams)
	"""

if __name__ =='__main__':
	main()