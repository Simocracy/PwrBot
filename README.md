Quellcode des PostWriter-Bot, welcher ab und an von Gobo7793 verwendet wird um verschiedene arbeiten zu erledigen.

Hinweis: Quellcode ist nicht immer ausreichend Kommentiert!

## PwrBot
Um den Bot Nutzen zu können, muss zunächst die Quelldatei `PwrBot\PwrBot.cs` angelegt werden. Die Datei hat folgenden Inhalt:
```
using DotNetWikiBot;

namespace Simocracy.PwrBot
{
	public class PwrBot
	{
		public static Site GetSite()
		{
			return new Site("https://simocracy.de/", "BENUTZERNAME", "PASSWORT");
		}
	}
}
```
`BENUTZERNAME` und `PASSWORT` müssen natürlich entsprechend angepasst werden.

Im Unterverzeichnis 'DotNetWikiBot' liegen die Quelldateien sowie die Informationen über die gleichnamige MediaWiki-API-Implementierung. Der DotNetWikiBot ist unter GPL 2.0 lizenziert.
