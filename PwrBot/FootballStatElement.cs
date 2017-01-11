using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Simocracy.PwrBot
{


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

		public int Played => Win + Drawn + Lose;
		public int Win { get; private set; }
		public int Drawn { get; private set; }
		public int Lose { get; private set; }
		public int GoalsFor { get; private set; }
		public int GoalsAgainst { get; private set; }
		public int GoalDiff => GoalsFor - GoalsAgainst;
		public int Points => Win * 3 + Drawn;

		public int Balance => Win - Lose;

		public void AddMatch(FootballMatch match)
		{
			if(match.Result != "X")
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
		}

		public string GetOpponentWikicode()
		{
			return String.Format("|- class=\"{0}\"\n| style=\"text-align:left;\" | {1}\n| {2} || {3} || {4} || {5} || {6} || {7} || {8:+0;-0;+0} || {9}",
				(Balance > 0) ? "s" : (Balance < 0) ? "n" : "u",
				Flag, Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst, GoalDiff, Points);
		}

		public string GetYearWikicode()
		{
			return String.Format("|-\n| '''{0}''' || {1} || {2} || {3} || {4} || {5} || {6} || {7:+0;-0;+0} || {8}",
				(Year > 1930) ? Year.ToString() : "N/A", Played, Win, Drawn, Lose, GoalsFor, GoalsAgainst, GoalDiff, Points);
		}
	}
}
