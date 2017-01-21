using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DotNetWikiBot;

namespace Simocracy.PwrBot
{
	class PwrBot : Bot
	{
		private static Site _Site;
		public static Site Site => _Site ?? (_Site = new Site("https://simocracy.de", PwrBotLoginData.Username, PwrBotLoginData.Password));

		static void Main(string[] args)
		{

			Console.WriteLine("Start");

			try
			{
				FootballMatch.AnalyseFootballStats("Statistik der UNAS-Fußballnationalmannschaft der Herren",
					new String[] {"UNS", "VSB", "AME", "CDO", "RIV"});

				//ChangeMedirienNamespace();
				//ChangeUNSFlaggeHistorisch();
				//ReplaceOldFlagTemplates();
				//RemoveCategories();
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
				Trace.WriteLine(e);
			}

			Console.WriteLine("Fertig!");
			Console.ReadKey();
		}

		/// <summary>
		/// Aktualisiert alle Flaggenvorlagen auf die Vorlage:Flaggenvorlage
		/// </summary>
		static void ReplaceOldFlagTemplates()
		{
			var oldTempRegex = @"\[\[Datei:([^\|]*)\|\{\{\{1\|20\}\}\}px\|?([^\]]*)?\]\]";
			var newTempReplace = @"{{Flaggenvorlage|$1|$2|{{{1|}}}|{{{b|}}}|{{{h|}}}|{{{2|}}}}}";

			Console.WriteLine("Ändere Flaggenvorlagen");
			var pl = new PageList(Site);
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

			var pl = new PageList(Site);
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
							System.Diagnostics.Process.Start(Site.address + "/" + p.title);
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

			PageList pl = new PageList(Site);
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

			PageList pl = new PageList(Site);
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
