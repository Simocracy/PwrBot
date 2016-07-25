using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
			get {
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

		/// <summary>
		/// Gruppiert eine Aufzählung mit <see cref="FootballMatch"/>-Instanzen nach den Gegnern des <see cref="MainTeam"/>
		/// </summary>
		/// <param name="matches">Aufzählung</param>
		/// <returns>Nach Gegnern gruppierte Liste</returns>
		public static Dictionary<string, List<FootballMatch>> SortForOpponents(IEnumerable<FootballMatch> matches)
		{
			var dic = new Dictionary<string, List<FootballMatch>>();
			var flagTempRegex = new Regex(@"\{\{([^\|\}]*)(\|([^\}\|]*))?(\|([^\}\|]*))?\}\}");

			foreach(var match in matches)
			{
				var flag = flagTempRegex.Match(match.OpponentTeam).Groups[1].Value;
				if(String.IsNullOrEmpty(flag))
					continue;
				else if(flag.Contains("?"))
				{
					var name = flagTempRegex.Replace(match.OpponentTeam, String.Empty).Trim();
					flag = Simocracy.GetFlag(name);
				}
				else
				{
					flag = Simocracy.GetFlag(flag);
				}

				if(dic.ContainsKey(flag))
					dic[flag].Add(match);
				else
				{
					var l = new List<FootballMatch>();
					l.Add(match);
					dic.Add(flag, l);
				}
			}

			return dic;
		}


		public static Dictionary<string, List<FootballMatch>> SortForOpponents(MatchCollection matches)
		{
			var matchesList = new List<FootballMatch>(matches.Count);
			foreach(Match match in matches)
				matchesList.Add(new FootballMatch(match));
			return SortForOpponents(matchesList);
		}

		/// <summary>
		/// Analysiert die Statistik und gibt diese nach Gegnern gruppiert und alphabetisch sortiert zurück
		/// </summary>
		/// <param name="matches">Spiele</param>
		/// <returns>Statistik</returns>
		public static SortedDictionary<string, FootballStatElement> AnalyseStats(MatchCollection regexMatches)
		{
			var sdic = new SortedDictionary<string, FootballStatElement>();
			var flagTempRegex = new Regex(@"\{\{([^\|\}]*)(\|([^\}\|]*))?(\|([^\}\|]*))?\}\}");

			foreach(Match regexMatch in regexMatches)
			{
				var match = new FootballMatch(regexMatch);
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
						Stats = new FootballOpponentStats()
						{
							Flag = String.Format("{{{{{0}|#}}}}", flag)
						}
					};
					sdic.Add(name, fse);
				}
				sdic[name].AddMatch(match);
			}

			return sdic;
		}
	}

	/// <summary>
	/// Gesamtstatistik für einen Gegner
	/// </summary>
	[DebuggerDisplay("Name={Name}, Matches={Matches.Count}")]
	public class FootballStatElement
	{
		/// <summary>
		/// Staatsname
		/// </summary>
		public string Name { get; set; } = String.Empty;
		/// <summary>
		/// Gesamtstatistik
		/// </summary>
		public FootballOpponentStats Stats { get; set; } = new FootballOpponentStats();
		/// <summary>
		/// Matchliste
		/// </summary>
		public List<FootballMatch> Matches { get; set; } = new List<FootballMatch>();
		/// <summary>
		/// Kommentar
		/// </summary>
		public string Comment { get; set; } = String.Empty;

		/// <summary>
		/// Fügt das <see cref="FootballMatch"/> hinzu und aktualisiert <see cref="Stats"/>
		/// </summary>
		/// <param name="match">Spiel</param>
		public void AddMatch(FootballMatch match)
		{
			Matches.Add(match);
			Stats.AddMatch(match);
		}
	}

	/// <summary>
	/// Statistik für Fußballspiele
	/// </summary>
	[DebuggerDisplay("Flag={Flag}, WDL={Win}-{Drawn}-{Lose}, Goals={GoalsFor}-{GoalsAgainst}")]
	public class FootballOpponentStats
	{
		public string Flag { get; set; }
		public int Played { get { return Win + Drawn + Lose; } }
		public int Win { get; set; } = 0;
		public int Drawn { get; set; } = 0;
		public int Lose { get; set; } = 0;
		public int GoalsFor { get; set; } = 0;
		public int GoalsAgainst { get; set; } = 0;
		public int GoalDiff { get { return GoalsFor - GoalsAgainst; } }
		public int Points { get { return Win * 3 + Drawn; } }

		public void AddMatch(FootballMatch match)
		{
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
	}

	enum ERegexMatchGroup
	{
		/// <summary>
		/// Kompletter Regex-Match
		/// </summary>
		FullMatch = 0,
		/// <summary>
		/// Muss nicht überall angegeben sein
		/// </summary>
		Trounament = 1,
		/// <summary>
		/// Kann ohne Jahresangabe sein
		/// </summary>
		Date = 2,
		/// <summary>
		/// Inkl. Flaggenkürzel
		/// </summary>
		City = 3,
		Stadium = 4,
		/// <summary>
		/// Siegermannschaft fett geschrieben
		/// </summary>
		HomeTeam = 5,
		/// <summary>
		/// Siegermannschaft fett geschrieben
		/// </summary>
		AwayTeam = 6,
		/// <summary>
		/// Ergebnis nach 120 Minuten
		/// </summary>
		Result = 7,
		Spectators = 8,
		/// <summary>
		/// Wenn nicht leer: Ausverkauft
		/// </summary>
		SoldOut = 9,
		/// <summary>
		/// Inkl. Flaggenkürzel
		/// </summary>
		Referee = 10
	}
}
