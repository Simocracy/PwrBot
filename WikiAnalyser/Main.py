import sys
import os
sys.path.append(os.path.abspath("../simocraPy/"))

from simocracy import wiki
from footballMatchAnalyzer import footballMatch

def main():
	articleName = "Statistik der UNAS-Fußballnationalmannschaft der Herren"
	mainTeams = ["UNS", "UANS", "VSB", "AME", "CDO", "RIV"]

	wiki.login()
	footballMatch.analyseFootballStats(articleName, mainTeams)

if __name__ =='__main__':
	main()