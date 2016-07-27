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
			AnalyseFootballStats("Statistik der UNAS-Fußballnationalmannschaft der Herren", new String[] { "UNS", "VSB", "AME", "CDO", "RIV" });
			//ReplaceOldFlagTemplates();
			//RemoveCategories();

			Console.WriteLine("Fertig!");
			Console.ReadKey();
		}

		/// <summary>
		/// Analysiert und wertet die Statistik einer Fußballnationalmannschaft aus
		/// </summary>
		static void AnalyseFootballStats(string articleName, params string[] mainTeams)
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
					UrlEncode(articleName),
					UrlEncode(opponentSection.ToString()),
					UrlEncode(sectionText),
					UrlEncode("Aktualisierung der Gegnerstatistiken"),
					UrlEncode(site.tokens["csrftoken"]));
				var result = site.PostDataAndGetResult(site.apiPath, editStr);
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
					UrlEncode(articleName),
					UrlEncode(yearSection.ToString()),
					UrlEncode(sectionText),
					UrlEncode("Aktualisierung der Jahresstatistiken"),
					UrlEncode(site.tokens["csrftoken"]));
				var result = site.PostDataAndGetResult(site.apiPath, editStr);
				Console.WriteLine("Year Result:");
				Console.WriteLine(result);
			}
		}

		/// <summary>
		/// Aktualisiert alle Flaggenvorlagen auf die Vorlage:Flaggenvorlage
		/// </summary>
		static void ReplaceOldFlagTemplates()
		{
			var oldTempRegex = @"\[\[Datei:([^\|]*)\|\{\{\{1\|20\}\}\}px\|?([^\]]*)?\]\]";
			var newTempReplace = @"{{Flaggenvorlage|$1|$2|{{{1|}}}|{{{b|}}}|{{{h|}}}|{{{2|}}}}}";

			Console.WriteLine("Ändere Flaggenvorlagen");
			var pl = new PageList(site);
			pl.FillAllFromCategory("Kategorie:Flaggenvorlage");
			Console.WriteLine("Vorlagenanzahl: {0}", pl.Count());

			foreach(var p in pl.pages)
			{
				p.Load();
				var regex = new Regex(oldTempRegex);
				var match = regex.Match(p.text);
				if(match.Success)
				{
					Console.WriteLine("Vorlage wird geändert");
					p.text = regex.Replace(p.text, newTempReplace);
					p.Save("Nutzung der Vorlage:Flaggenvorlage", false);
				}
			}
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
		/// Löscht die Kategorien
		/// </summary>
		static void RemoveCategories()
		{
			var oldCat = "Kategorie:Reform";
			Console.WriteLine("Lösche Kategorien von " + oldCat);

			PageList pl = new PageList(site);
			pl.FillAllFromCategory(oldCat);
			Console.WriteLine("Seitenanzahl: " + pl.Count());
			foreach(var p in pl.pages)
			{
				p.Load();
				p.RemoveFromCategory(oldCat);
				p.Save("Löschen der Kategorie:Reform", false);
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
