using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetWikiBot;

namespace Simocracy.PwrBot
{
	class Program : Bot
	{
		static Site site = PwrBot.GetSite();

		static void Main(string[] args)
		{
			//ChangeMedirienNamespace();
			//ChangeUNSFlaggeHistorisch();
			AnalyseStripesStats();

			Console.WriteLine("Fertig!");
			Console.Read();
		}

		/// <summary>
		/// Analysiert und wertet die Statistik der UNAS-Fußballnationalmannschaft aus
		/// </summary>
		static void AnalyseStripesStats()
		{
			var articleName = "Statistik der UNAS-Fußballnationalmannschaft der Herren";
			FootballMatch.MainTeam = new String[] { "UNS", "UNAS", "VSB", "AME", "CDO", "RIV" };
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
			var matchRegexPat = @"\|(.*[^\|\r\n]*)\s*[\r\n|\|]\|\s*([^\|\r\n]*)?\s*[\r\n|\|]\|\s*(\{\{.+\}\}[^\|\r\n]*)?[\r\n\|]\|\s*([^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}\{\{.+\}\}[^\|\r\n]*)?\s*[\r\n\|]\|\s*('{0,3}\{\{.+\}\}[^\|\r\n]*)?\s*[\r\n\|]\|\s*[^\|]*\|\s*'''([^']*)'''[^\|\r\n]*?\s*[\r\n\|]\|\s*([\d\.]*)(\s*\(a\))?\s*[\r\n\|]\|\s*(\{\{.*\}\}[^\|\r\n]*)?";

			Console.WriteLine("Lade Artikel");
			Page p = new Page(articleName);
			p.Load();

			var matchesRegex = Regex.Matches(p.text, matchRegexPat);
			var matches = new List<FootballMatch>(matchesRegex.Count);
			foreach(Match match in matchesRegex)
				matches.Add(new FootballMatch(match));

			Console.WriteLine(matches.Count + " Matches found");

			//var opponents = matches.GroupBy(x => x.OpponentTeam).Select(g => new { g.Key }).ToList();
			var opponents = matches.GroupBy(u => u.OpponentTeam)
									  .Select(grp => new { OpponentTeam = grp.Key, Matches = grp.ToList() })
									  .ToList();
			Console.WriteLine(opponents.Count() + " Opponents found");
			foreach(var o in opponents)
				Console.WriteLine("{0}: {1} Matches", o.OpponentTeam, o.Matches.Count);
		}

		/// <summary>
		/// Passt die historischen UNAS-Flaggen an die neuen Vorlagenfunktionen an
		/// </summary>
		static void ChangeUNSFlaggeHistorisch()
		{
			var tempUNS = "Vorlage:USRV";
			var tempUNSRegex = @"\{\{(USRV)(\|[^\}]*)?(\|J=\d*)?\}\}";

			Console.WriteLine("Ändere Einbindung historischer Rivera-Flaggen");

			var pl = new PageList(site);
			//pl.FillFromLinksToPage(tempUNS);
			pl.FillFromTransclusionsOfPage(tempUNS);
			Console.WriteLine("Seitenanzahl: " + pl.Count());

			foreach(var p in pl.pages)
			{
				p.Load();
				var tempMatch = Regex.Match(p.text, tempUNSRegex);

				if(tempMatch.Success)
				{
					if(!tempMatch.Groups[3].Success && !tempMatch.Groups[2].Value.Contains("|J"))
					{
						var pageTitleRegex = @".*(20\d\d)";
						var pageTitleMatch = Regex.Match(p.title, pageTitleRegex);
						if(pageTitleMatch.Success)
						{
							var year = Int32.Parse(pageTitleMatch.Groups[1].Value);
							if(year >= 2028)
							{
								p.text = Regex.Replace(p.text, tempUNSRegex, "{{$1$2|J=2028}}");
								p.Save("Anpassung historischer Rivera-Flaggen", true);
								Console.WriteLine(p.title + " automatisch angepasst.");
								continue;
							}
							else if(year >= 2053)
							{
								p.text = Regex.Replace(p.text, tempUNSRegex, "{{$1$2|J=2053}}");
								p.Save("Anpassung historischer Rivera-Flaggen", true);
								Console.WriteLine(p.title + " automatisch angepasst.");
								continue;
							}
						}

						//if(!p.title.Contains("Eisbären-Pokal")) // Zum schnellen Weiterführen von bereits begonnen Aktionen
						//	continue;

						Console.WriteLine("Seite: " + p.title + ", o = öffnen, 1 = 2028, 2 = 2053");
						var input = Console.ReadKey();
						if(input.KeyChar == 'o')
						{
							System.Diagnostics.Process.Start(site.address + "/" + p.title);
							Console.Write(" ");
							input = Console.ReadKey();
						}
						Console.WriteLine();

						if(input.KeyChar == '1')
						{
							p.text = Regex.Replace(p.text, tempUNSRegex, "{{$1$2|J=2041}}");
							p.Save("Anpassung historischer Rivera-Flaggen", true);
						}
						else if(input.KeyChar == '2')
						{
							p.text = Regex.Replace(p.text, tempUNSRegex, "{{$1$2|J=2053}}");
							p.Save("Anpassung historischer Rivera-Flaggen", true);

						}
					}
					else
						Console.WriteLine(p.title + " bereits bearbeitet.");
				}
				else
					Console.WriteLine(p.title + " enthählt keine Vorlage.");
			}
		}

		/// <summary>
		/// Verschiebt die Kategorie KR Medirien zu Almoravidien
		/// </summary>
		static void ChangeMedirienNamespace()
		{
			var oldCat = "Kategorie:Königreich Medirien";
			var newCat = "Kategorie:Almoravidien";
			Console.WriteLine("Ändere Kategorien von " + oldCat + " zu " + newCat);

			PageList pl = new PageList(site);
			pl.FillAllFromCategory(oldCat);
			Console.WriteLine("Seitenanzahl: " + pl.Count());
			foreach(var p in pl.pages)
			{
				Console.WriteLine("Seite: " + p.title);
				p.Load();
				p.RemoveFromCategory(oldCat);
				p.AddToCategory(newCat);
				p.Save();
				//p.text.Replace(oldCat, newCat);
			}
		}
	}
}
