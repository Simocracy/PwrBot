Quellcode des PostWriter-Bot, welcher ab und an von Gobo7793 verwendet wird um verschiedene arbeiten zu erledigen.

Hinweis: Quellcode ist nicht immer ausreichend Kommentiert!

## PwrBot
Um den Bot Nutzen zu können, muss zunächst die Datei `PwrBot\PwrBotLoginData.cs` angelegt werden. Die Datei hat folgenden Inhalt:
```
namespace Simocracy.PwrBot
{
	internal class PwrBotLoginData
	{
		public static string Username = "BENUTZER";
		public static string Password = "PASSWORT";
	}
}
```
`BENUTZER` und `PASSWORT` sind die Anmeldedaten für den Wiki-Account im [Wikocracy](https://simocracy.de/). Die Datei ist anschließend zu kompilieren, damit sie vom Programm genutzt werden kann.

Im Unterverzeichnis `DotNetWikiBot` liegen die Quelldateien sowie die Informationen über die gleichnamige MediaWiki-API-Implementierung. Die `DotNetWikiBot`-Implementierung der Mediawiki-API ist unter GPL 2.0 lizenziert und auf [Sourceforge](http://dotnetwikibot.sourceforge.net/) zu finden.

Der Bot dient hauptsächlich als Unterstützung für einmalige Arbeiten.



## WikiAnalyzer
Der WikiAnalyzer ist eine in Python konvertierte Version der im `PwrBot` zu findenden Auswertung von Fußballstatistiken für Nationalmannschaften in Simocracy.

#### Installation
Um den WikiAnalyzer lokal nutzen zu können, muss zunächst [simocraPy](https://github.com/Simocracy/simocraPy) heruntergeladen und nach `simocraPy\` kopiert werden. Anschließend muss die Datei `simocraPy\simocracy\credentials.py` mit folgendem Inhalt erstellt werden:
```
username="BENUTZER"
password="PASSWORT"
```
`BENUTZER` und `PASSWORT` sind die Anmeldedaten für den Wiki-Account im [Wikocracy](https://simocracy.de/).

#### Anwendung
Damit die Daten ausgewertet werden können, müssen Abschnitte für Gegnerstatistiken (in der Überschrift muss die Zeichenkette `Gegner` enthalten sein) bzw. für Jahresstatistiken (`Jahr` muss enthalten sein) vorhanden sein. Es wird jeweils der zuletzt vorkommende Abschnitt genutzt.

Die auszuwertenden Spiele müssen in einer Tabelle mit folgendem Schema aufgelistet werden.
```
{|class="wikitable"
|-
! Turnier || Datum || Ort || Stadion
! colspan="2" | Mannschaften
! Ergebnis || Zuschauer || Schiedsrichter
|-
| [[Fußball-Amerikameisterschaft 2052|AM52]]
| 15.06.2052 || {{BOL}} Cali || Estadio Olímpico
| '''{{UNS|#}}''' || {{GRSI|#}}
|style="text-align:center;"| '''3:0''' (3:0)
| 35.000 (a) || {{FRC}} Robert Casson
|-
...
|}
```
- Turnier: Pflicht. Kann mit oder ohne Link realisiert werden.
- Datum: Pflicht. Das Datum muss eines der folgenden Formate entsprechen: `DD.MM.YYYY`, `MM.YYYY` und `YYYY`.
- Ort: Mindestens die Flagge des Landes muss angegeben sein, die Stadt ist optional.
- Stadion: Optional.
- Mannschaften: Pflicht. Angabe der Staaten entweder mithilfe der Flaggenvorlage oder alternativ `{{GRA}} Grafenberg`. `{{?}}` wird Unterstützt, dann ist eine Staatsangabe zwingend nötig.
- Ergebnis: Pflicht. Ausgewertet wird nur Endergebnis, welches mit Wikicode Fett formatiert werden muss. Zwischenstände oder Ergebnisse im Elfmeterschießen sind optional möglich.
- Zuschauer: Optional. Formatierung der Zuschauerzahl ist egal. `(a)` für Ausverkauft ist optional möglich.
- Schiedsrichter: Optional.

Ein Beispiel für die Anwendung des WikiAnalyzer ist die [Statistik der UNAS-Nationalmannschaft](https://simocracy.de/Statistik_der_UNAS-Fu%C3%9Fballnationalmannschaft_der_Herren).
