using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetWikiBot;

namespace Simocracy.PwrBot
{
	[DebuggerDisplay("Home={HomeTeam}, Away={AwayTeam}, Result={Result}")]
	public class FootballMatch
	{
		/// <summary>
		/// Team, dessen Statistik erstellt wird, Basierend auf FLAGGENKÜRZEL
		/// </summary>
		public static string[] MainTeam { get; set; } = { "UNS" };

		/// <summary>
		/// Liste der Nachfolgerstaaten, FLAGGENKÜRZEL, Schema Vorgänger -> Aktueller Staat (bzw. Nachfolger)
		/// </summary>
		public static Dictionary<string, string> Successor { get; set; } = new Dictionary<string, string>();

		private string _Tournament;
		public string Tournament
		{
			get { return _Tournament; }
			private set
			{
				var s = value.Trim();
				_Tournament = (s.Length < 3) ? s.Replace("-", String.Empty) : s;
			}
		}

		public DateTime Date { get; private set; }

		private string _City;
		/// <summary>
		/// Inkl. Flaggenkürzel
		/// </summary>
		public string City
		{
			get { return _City; }
			set { _City = value.Trim(); }
		}

		private string _Stadium;
		public string Stadium
		{
			get { return _Stadium; }
			private set { _Stadium = value.Trim(); }
		}

		private string _HomeTeam;
		/// <summary>
		/// Inkl. Flaggenkürzel
		/// </summary>
		public string HomeTeam
		{
			get { return _HomeTeam; }
			private set { _HomeTeam = value.Trim().Replace("'", ""); }
		}

		private string _AwayTeam;
		/// <summary>
		/// Inkl. Flaggenkürzel
		/// </summary>
		public string AwayTeam
		{
			get { return _AwayTeam; }
			private set { _AwayTeam = value.Trim().Replace("'", ""); }
		}

		/// <summary>
		/// Gegnerteam für Sortierung nach Gegner
		/// </summary>
		public string OpponentTeam
		{
			get
			{
				foreach(var s in MainTeam)
				{
					if(AwayTeam.Contains(s))
						return HomeTeam;
				}
				return AwayTeam;
			}
		}

		/// <summary>
		/// -1 wenn kein Spielergebnis
		/// </summary>
		public int ResultHome { get; private set; }

		/// <summary>
		/// -1 wenn kein Spielergebnis
		/// </summary>
		public int ResultAway { get; private set; }

		/// <summary>
		/// Vollständiges Spielergebnis
		/// </summary>
		public string Result
		{
			get
			{
				if(ResultHome < 0 || ResultAway < 0)
					return "X";
				else
					return ResultHome + ":" + ResultAway;
			}
		}

		public int Specators { get; private set; }
		public bool IsSOldOut { get; private set; }

		/// <summary>
		/// Inkl. Flaggenkürzel
		/// </summary>
		private string _Referee;
		public string Referee
		{
			get { return _Referee; }
			private set { _Referee = value.Trim(); }
		}

		/// <summary>
		/// Quellcode im Wiki
		/// </summary>
		public string SourceCode { get; private set; }

		public FootballMatch(Match match)
			: this(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value, match.Groups[5].Value, match.Groups[6].Value, match.Groups[7].Value, match.Groups[8].Value, match.Groups[9].Value, match.Groups[10].Value, match.Value)
		{ }

		public FootballMatch(string tournament, string date, string city, string stadium, string homeTeam, string awayTeam, string result, string spectators, string soldOut, string referee, string source)
		{
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
		}

		private void SetDate(string date)
		{
			DateTime dt = DateTime.MinValue;
			if(!String.IsNullOrEmpty(date))
			{
				var exactDatePattern = @"((\d{1,2})\.)?((\d{1,2})\.)?(\d{2,4})";
				var exactDateMatch = Regex.Match(date, exactDatePattern);
				if(exactDateMatch.Success)
				{
					var matchStr = exactDateMatch.Value;
					if(exactDateMatch.Groups[5].Length < 4)
						matchStr = matchStr.Insert(matchStr.Length - 2, "20");
					if(exactDateMatch.Groups[1].Success)
						DateTime.TryParse(matchStr, out dt);
					else
						dt = new DateTime(Int32.Parse(matchStr), 1, 1);
				}
			}
			Date = dt;
		}

		private void SetResults(string result)
		{
			var resPattern = @"(\d+):(\d+)";
			var resMatch = Regex.Match(result, resPattern);
			if(resMatch.Success)
			{
				ResultHome = Int32.Parse(resMatch.Groups[1].Value);
				ResultAway = Int32.Parse(resMatch.Groups[2].Value);
			}
			else
			{
				ResultHome = -1;
				ResultAway = -1;
			}
		}

		private void SetSpectators(string spectators)
		{
			int specs = 0;
			Int32.TryParse(spectators, NumberStyles.Number, CultureInfo.CurrentCulture, out specs);
			Specators = specs;
		}

		private void SetIsSoldOut(string soldOut)
		{
			IsSOldOut = !String.IsNullOrEmpty(soldOut);
		}

		public string GetLowLinesWikiCode()
		{
			var regexMatch = Regex.Match(SourceCode, @"\|(.*[^\|\r\n]*)\s*[\r\n|\|]\|\s*([^\|\r\n]*)?\s*[\r\n|\|]\|\s*(\{\{.+\}\}[^\|\r\n]*)[\r\n\|]\|\s*([^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}(\{\{.+\}\})[^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}(\{\{.+\}\})[^\|\r\n]*)?\s*[\r\n\|]\|\s*[^\|]*\|\s*'''((\d+):(\d+))?([^']*)'''(\s*(<br>)?\s*([^\|\r\n]*))?\s*[\r\n\|]\|\s*([\d\.]*)(\s*\(a\))?\s*[\r\n\|]\|\s*(\{\{.*\}\}[^\|\r\n]*)?");
			var tournament = regexMatch.Groups[1].Value.Trim();
			var date = regexMatch.Groups[2].Value.Trim();
			var city = regexMatch.Groups[3].Value.Trim();
			var stadium = regexMatch.Groups[4].Value.Trim();
			var referee = regexMatch.Groups[18].Value.Trim();
			
			var specs = regexMatch.Groups[16].Value.Trim();
			if(!String.IsNullOrEmpty(regexMatch.Groups[17].Value))
				specs += " " + regexMatch.Groups[17].Value.Trim();

			int resultHome = -1;
			int resultAway = -1;
			var hasRes = Int32.TryParse(regexMatch.Groups[10].Value, out resultHome);
			if(hasRes)
				hasRes = Int32.TryParse(regexMatch.Groups[11].Value, out resultAway);
			var ResComment = regexMatch.Groups[12].Value.Trim();
			var halfRes = regexMatch.Groups[13].Value.TrimEnd();
			string result = "'''";
			if(hasRes)
			{
				result += resultHome + ":" + resultAway;
				if(!String.IsNullOrEmpty(ResComment))
					result += " " + ResComment;
				result += "'''" + halfRes;
			}
			else
			{
				result += ResComment + "'''";
			}

			string homeTeam = regexMatch.Groups[6].Value.Replace("'", "").Trim();
			if(homeTeam.Contains("?"))
			{
				homeTeam = regexMatch.Groups[5].Value.Replace("'", "").Trim();
			}
			else if(!homeTeam.Contains("#"))
			{
				homeTeam = homeTeam.Insert(homeTeam.Length - 2, "|#");
			}
			if(resultHome > resultAway)
				homeTeam = "'''" + homeTeam + "'''";

			string awayTeam = regexMatch.Groups[8].Value.Replace("'", "").Trim();
			if(awayTeam.Contains("?"))
			{
				awayTeam = regexMatch.Groups[7].Value.Replace("'", "").Trim();
			}
			else if(!awayTeam.Contains("#"))
			{
				awayTeam = awayTeam.Insert(awayTeam.Length - 2, "|#");
			}
			if(resultAway > resultHome)
				awayTeam = "'''" + awayTeam + "'''";

			return String.Format("| {0}\n| {1} || {2} || {3}\n| {4} || {5}\n|style=\"text-align:center;\"| {6}\n| {7} || {8}\n",
				tournament, // Turnier
				date, // Datum
				city, // Ort
				stadium, // Stadion
				homeTeam, // Heim
				awayTeam, // Aus
				result, // Ergebnis
				specs, // Zuschauer
				referee); // Schiri
		}

		/// <summary>
		/// Parsed die Matches
		/// </summary>
		/// <param name="matches">Matches</param>
		/// <returns>Liste mit <see cref="FootballMatch"/></returns>
		public static List<FootballMatch> GetMatchList(MatchCollection matches)
		{
			var matchesList = new List<FootballMatch>(matches.Count);
			foreach(Match match in matches)
				matchesList.Add(new FootballMatch(match));
			return matchesList;
		}

		/// <summary>
		/// Analysiert die Statistik und gibt diese nach Gegnern gruppiert und alphabetisch sortiert zurück
		/// </summary>
		/// <param name="matches">Spiele</param>
		/// <returns>Statistik</returns>
		public static SortedDictionary<string, FootballStatElement> SortForOpponents(IEnumerable<FootballMatch> matches)
		{
			var sdic = new SortedDictionary<string, FootballStatElement>();
			var flagTempRegex = new Regex(@"\{\{([^\|\}]*)(\|([^\}\|]*))?(\|([^\}\|]*))?\}\}");

			foreach(var match in matches)
			{
				var flagMatch = flagTempRegex.Match(match.OpponentTeam);

				var flag = flagMatch.Groups[1].Value;
				if(String.IsNullOrEmpty(flag))
					continue;

				int pxValue;
				string name;
				if(flagMatch.Groups[3].Success && !flagMatch.Groups[3].Value.Contains("=") && !Int32.TryParse(flagMatch.Groups[3].Value, out pxValue))
				{
					name = flagMatch.Groups[3].Value;
				}
				else if(flagMatch.Groups[5].Success && !flagMatch.Groups[5].Value.Contains("=") && !Int32.TryParse(flagMatch.Groups[5].Value, out pxValue))
				{
					name = flagMatch.Groups[5].Value;
				}
				else
				{
					name = flagTempRegex.Replace(match.OpponentTeam, String.Empty).Trim();
				}

				var isNoneFlag = flag.Contains("?");
				var isDummyName = name.Contains("#");
				flag = Simocracy.GetFlag(isNoneFlag ? name : flag);
				name = Simocracy.GetStateName(isDummyName ? flag : name);

				if(!sdic.ContainsKey(name))
				{
					var fse = new FootballStatElement()
					{
						Name = name,
						Flag = String.Format("{{{{{0}|#}}}}", flag)
					};
					sdic.Add(name, fse);
				}
				sdic[name].AddMatch(match);
			}

			return sdic;
		}

		/// <summary>
		/// Ausgabe der Bilanz von <see cref="MainTeam"/> nach Gegnern
		/// </summary>
		/// <param name="opponents">Bilanz als <see cref="SortedDictionary{string, FootballStatElement}"/></param>
		/// <returns>Bilanz als Wikicode</returns>
		public static string GetOpponentTableCode(SortedDictionary<string, FootballStatElement> opponents)
		{
			var text = "=== Nach Gegner ===\n" +
				"<html><style>\n" +
				".s {\n" +
				"    background:#CCFFCC;\n" +
				"}\n" +
				".u {\n" +
				"    background:#FFFFCC;\n" +
				"}\n" +
				".n {\n" +
				"    background:#FFCCCC;\n" +
				"}</style></html>\n" +
				"{| class=\"wikitable sortable\" style=\"text-align:center;\"\n" +
				"|-\n" +
				"! Mannschaft\n" +
				"! <abbr title=\"Spiele\">Sp.</abbr>\n" +
				"! <abbr title=\"Siege\">S</abbr>\n" +
				"! <abbr title=\"Unentschieden\">U</abbr>\n" +
				"! <abbr title=\"Niederlagen\">N</abbr>\n" +
				"! <abbr title=\"Tore\">T</abbr>\n" +
				"! <abbr title=\"Gegentore\">GT</abbr>\n" +
				"! <abbr title=\"Tordifferenz\">TD</abbr>\n" +
				"! <abbr title=\"Punkte\">P</abbr>";

			foreach(var opp in opponents)
			{
				if(!(MainTeam.Contains("RIV") && opp.Value.Flag.Contains("RIV")))
					text = String.Format("{0}\n{1}", text, opp.Value.GetOpponentWikicode());
			}

			text = String.Format("{0}\n{1}", text, "|}");

			return text;
		}

		public static SortedDictionary<int, FootballStatElement> SortForYears(IEnumerable<FootballMatch> matches)
		{
			var sdic = new SortedDictionary<int, FootballStatElement>();

			foreach(var match in matches)
			{
				if(match.Date != DateTime.MinValue)
				{
					if(!sdic.ContainsKey(match.Date.Year))
					{
						var fse = new FootballStatElement()
						{
							Year = match.Date.Year
						};
						sdic.Add(match.Date.Year, fse);
					}
					sdic[match.Date.Year].AddMatch(match);
				}
			}

			return sdic;
		}

		public static string GetYearTableCode(SortedDictionary<int, FootballStatElement> years)
		{
			var text = "=== Nach Jahr ===\n" +
				"{| class=\"wikitable sortable\" style=\"text-align:center;\"\n" +
				"|-\n" +
				"! Jahr\n" +
				"! <abbr title=\"Spiele\">Sp.</abbr>\n" +
				"! <abbr title=\"Siege\">S</abbr>\n" +
				"! <abbr title=\"Unentschieden\">U</abbr>\n" +
				"! <abbr title=\"Niederlagen\">N</abbr>\n" +
				"! <abbr title=\"Tore\">T</abbr>\n" +
				"! <abbr title=\"Gegentore\">GT</abbr>\n" +
				"! <abbr title=\"Tordifferenz\">TD</abbr>\n" +
				"! <abbr title=\"Punkte\">P</abbr>";

			int allWon = 0;
			int allDrawn = 0;
			int allLose = 0;
			int allGoalsFor = 0;
			int allGoalsAgainst = 0;

			foreach(var year in years)
			{
				allWon += year.Value.Win;
				allDrawn += year.Value.Drawn;
				allLose += year.Value.Lose;
				allGoalsFor += year.Value.GoalsFor;
				allGoalsAgainst += year.Value.GoalsAgainst;

				text = String.Format("{0}\n{1}", text, year.Value.GetYearWikicode());
			}

			var played = allWon + allDrawn + allLose;
			var allGoalDiff = (allGoalsFor - allGoalsAgainst).ToString("+0;-0;+0");
			var points = allWon * 3 + allDrawn;
			text = String.Format("{0}\n|-\n! Ges. || {1} || {2} || {3} || {4} || {5} || {6} || {7} || {8}\n|}}",
				text, played, allWon, allDrawn, allLose, allGoalsFor, allGoalsAgainst, allGoalDiff, points);

			return text;
		}

		/// <summary>
		/// Analysiert und wertet die Statistik einer Fußballnationalmannschaft aus
		/// </summary>
		/// <param name="articleName">Artikel mit der Statistik</param>
		/// <param name="mainTeams">Mainteams, auf die ausgewertet werden soll</param>
		public static void AnalyseFootballStats(string articleName, params string[] mainTeams)
		{
			//var articleName = "Statistik der UNAS-Fußballnationalmannschaft der Herren";
			//FootballMatch.MainTeam = new String[] { "UNS", "VSB", "AME", "CDO", "RIV" };
			FootballMatch.MainTeam = mainTeams;
			/*
			 * Gruppen:
			 * 1: Turnier
			 * 2: Datum
			 * 3: Ort
			 * 4: Stadion
			 * 5: Heimteam
			 * 6: Gastteam
			 * 7: Endergebnis (120 min)
			 * 8: Zuschauer
			 * 9: Wenn nicht leer: Ausverkauft
			 * 10: Schiedsrichter
			 */
			var matchRegexPat = @"\|(.*[^\|\r\n]*)\s*[\r\n|\|]\|\s*([^\|\r\n]*)?\s*[\r\n|\|]\|\s*(\{\{.+\}\}[^\|\r\n]*)[\r\n\|]\|\s*([^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}\{\{.+\}\}[^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}\{\{.+\}\}[^\|\r\n]*)?\s*[\r\n\|]\|\s*[^\|]*\|\s*'''([^']*)'''[^\|\r\n]*?\s*[\r\n\|]\|\s*([\d\.]*)(\s*\(a\))?\s*[\r\n\|]\|\s*(\{\{.*\}\}[^\|\r\n]*)?";

			Console.WriteLine("Lade Artikel");
			Page p = new Page(articleName);
			p.Load();

			var sectionMatches = Regex.Matches(p.text, @"(^={1,6}(.*)?={1,6}\s*?$)", RegexOptions.Multiline);
			int opponentSection = 0;
			int yearSection = 0;
			for(int i = 0; i < sectionMatches.Count; i++)
			{
				if(sectionMatches[i].Value.Contains("Nach Gegner"))
					opponentSection = i + 1;
				else if(sectionMatches[i].Value.Contains("Nach Jahr"))
					yearSection = i + 1;
			}

			var matchesRegex = Regex.Matches(p.text, matchRegexPat);
			var footballMatchList = FootballMatch.GetMatchList(matchesRegex);
			Console.WriteLine(footballMatchList.Count + " Matches found");

			//foreach(var match in footballMatchList)
			//{
			//	var newText = match.GetLowLinesWikiCode();
			//	p.text = p.text.Replace(match.SourceCode, newText);
			//}

			//p.Save();

			if(opponentSection != 0)
			{
				var opponents = FootballMatch.SortForOpponents(footballMatchList);
				Console.WriteLine(opponents.Count + " Opponents found");
				foreach(var o in opponents)
					Console.WriteLine("{0}: {1} Matches", o.Key, o.Value.Played);

				var sectionText = FootballMatch.GetOpponentTableCode(opponents);

				var editStr = String.Format("action=edit&format=xml&bot=1&title={0}&section={1}&text={2}&summary={3}&token={4}",
					Bot.UrlEncode(articleName),
					Bot.UrlEncode(opponentSection.ToString()),
					Bot.UrlEncode(sectionText),
					Bot.UrlEncode("Aktualisierung der Gegnerstatistiken"),
					Bot.UrlEncode(PwrBot.Site.tokens["csrftoken"]));
				var result = PwrBot.Site.PostDataAndGetResult(PwrBot.Site.apiPath, editStr);
				Console.WriteLine("Opponent Result:");
				Console.WriteLine(result);
			}

			if(yearSection != 0)
			{
				var years = FootballMatch.SortForYears(footballMatchList);
				Console.WriteLine(years.Count + " Years found");
				foreach(var o in years)
					Console.WriteLine("{0}: {1} Matches", o.Key, o.Value.Played);

				var sectionText = FootballMatch.GetYearTableCode(years);

				var editStr = String.Format("action=edit&format=xml&bot=1&title={0}&section={1}&text={2}&summary={3}&token={4}",
					Bot.UrlEncode(articleName),
					Bot.UrlEncode(yearSection.ToString()),
					Bot.UrlEncode(sectionText),
					Bot.UrlEncode("Aktualisierung der Jahresstatistiken"),
					Bot.UrlEncode(PwrBot.Site.tokens["csrftoken"]));
				var result = PwrBot.Site.PostDataAndGetResult(PwrBot.Site.apiPath, editStr);
				Console.WriteLine("Year Result:");
				Console.WriteLine(result);
			}
		}
	}

	/// <summary>
	/// Gesamtstatistik für einen Gegner
	/// </summary>
	[DebuggerDisplay("Name={Name}, Flag={Flag}, Matches={Matches.Count}, WDL={Win}-{Drawn}-{Lose}, Goals={GoalsFor}-{GoalsAgainst}")]
	public class FootballStatElement
	{
		/// <summary>
		/// Staatsname
		/// </summary>
		public string Name { get; set; } = String.Empty;
		public string Flag { get; set; } = String.Empty;
		/// <summary>
		/// Staatsname
		/// </summary>
		public int Year { get; set; } = 0;
		/// <summary>
		/// Matchliste
		/// </summary>
		public List<FootballMatch> Matches { get; set; } = new List<FootballMatch>();
		/// <summary>
		/// Kommentar am Anfang
		/// </summary>
		public string PreComment { get; set; } = String.Empty;
		/// <summary>
		/// Kommentar am Ende
		/// </summary>
		public string PostComment { get; set; } = String.Empty;

		public int Played { get { return Win + Drawn + Lose; } }
		public int Win { get; private set; } = 0;
		public int Drawn { get; private set; } = 0;
		public int Lose { get; private set; } = 0;
		public int GoalsFor { get; private set; } = 0;
		public int GoalsAgainst { get; private set; } = 0;
		public int GoalDiff { get { return GoalsFor - GoalsAgainst; } }
		public int Points { get { return Win * 3 + Drawn; } }
		public int Balance
		{
			get
			{
				if(Win > Lose)
					return 1;
				else if(Win < Lose)
					return -1;
				else
					return 0;
			}
		}

		public void AddMatch(FootballMatch match)
		{
			Matches.Add(match);
			if(match.HomeTeam == match.OpponentTeam)
			{
				if(match.ResultHome < match.ResultAway)
					Win++;
				else if(match.ResultHome > match.ResultAway)
					Lose++;
				else
					Drawn++;

				GoalsFor += match.ResultAway;
				GoalsAgainst += match.ResultHome;
			}
			else
			{
				if(match.ResultHome > match.ResultAway)
					Win++;
				else if(match.ResultHome < match.ResultAway)
					Lose++;
				else
					Drawn++;

				GoalsFor += match.ResultHome;
				GoalsAgainst += match.ResultAway;
			}
		}

		public string GetOpponentWikicode()
		{
			return String.Format("|- class=\"{0}\"\n| style=\"text-align:left;\" | {1}\n| {2} || {3} || {4} || {5} || {6} || {7} || {8} || {9}",
				(Balance > 0) ? "s" : (Balance < 0) ? "n" : "u",
				Flag, Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst,
				GoalDiff.ToString("+0;-0;+0"), Points);
		}

		public string GetYearWikicode()
		{
			return String.Format("|-\n| '''{0}''' || {1} || {2} || {3} || {4} || {5} || {6} || {7} || {8}",
				Year, Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst,
				GoalDiff.ToString("+0;-0;+0"), Points);
		}
	}
}
