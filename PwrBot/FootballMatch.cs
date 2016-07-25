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
	class FootballMatch
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
					flag = String.Format("{{{{{0}|#}}}}", Simocracy.SearchCurrentFlag(name));
				}
				else
				{
					flag = String.Format("{{{{{0}|#}}}}", Simocracy.ReplaceOldFlag(flag));
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
