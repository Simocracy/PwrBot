using System.Collections.Generic;

namespace Simocracy.PwrBot
{
	class Simocracy
	{
		/// <summary>
		/// Gibt das Flaggenkürzel des aktuellen Staates bzw. Nachfolgers zurück, die nicht (de)fusioniert wurden bzw. der Nachfolger eindeutig ist.
		/// </summary>
		public static Dictionary<string, string> Flags
		{
			get
			{
				return new Dictionary<string, string>
				{
					{"ABR", "BSC"},
					{"BOS", "BSC"},
					{"DRB", "BSC"},
					{"ADE", "ORA"},
					{"AQU", "SNL"},
					{"AZO", "NZL"},
					{"FNZ", "NZL"},
					{"NHI", "NZL"},
					{"FRP", "NZL"},
					{"FNS", "NZL"},
					{"OKA", "NZL"},
					{"NZ", "NZL"},
					{"New Halma Islands", "NZL"},
					{"Neu Halmanesien", "NZL"},
					{"New-Halmanesien", "NZL"},
					{"Pacifica", "NZL"},
					{"ASR", "BTZ"},
					{"SUD", "PATA"},
					{"HYL", "HYA"},
					{"LAG", "RLQ"},
					{"LAG", "RLQ"},
					{"UKSI", "GRSI"},
					{"SEV", "GRSI"},
					{"SR", "KLY"},
					{"STO", "KLY"},
					{"NGT", "KLY"},
					{"BOU", "KLY"},
					{"Vannekar", "GRA"},
					{"SHI", "KNN"},
					{"HIK", "KNN"},
					{"NVC", "KNN"},
					{"UIP", "RPP"},
					{"RPA", "RPP"},
					{"Janinisches Reich", "RPP"},
					{"VIR", "TOR"},
					{"4VIR", "TOR"},
					{"RNL", "NLL"},
					{"AGM", "NLL"},
					{"RUQ", "KSW"},
					{"SOW", "KSW"},
					{"KUR", "CSVR"},
					{"RCF", "FRC"},
					{"NFRC", "FRC"},
					{"RCH", "FRC"},
					{"BRU", "FVS"},
					{"AST", "MAS"},
					{"BOL1", "BOL"},
					{"TRU", "MAC"},
					{"MAC1", "MAC"},
					{"EMM", "EMA"},
					{"HJH", "NDL"},
					{"SKV", "NDL"},
					{"KOR", "SPA"},
					{"Sijut", "GOA"},
					{"GOT", "GOA"},
					{"SIM", "NUS"},
					{"Nuestra Senora", "NUS"},
					{"Simultanien", "NUS"},
					{"Azoren", "KBAZ"},
					{"IBRU", "KBAB"}
				};
			}
		}

		/// <summary>
		/// Gibt das Flaggenkürzel des aktuellen Staates bzw. Nachfolgers zurück, die nicht (de)fusioniert wurden bzw. der Nachfolger eindeutig ist.
		/// </summary>
		/// <param name="flag">Kürzel oder Name des historischen Staates</param>
		/// <returns>Kürzel des aktuellen Staates</returns>
		public static string GetFlag(string flag)
		{
			string current;
			return Flags.TryGetValue(flag, out current) ? current : flag;
		}

		/// <summary>
		/// Gibt den Staatsnamen des angegebenen Kürzels oder historischen Staates zurück
		/// </summary>
		/// <param name="flag">Historisches Kürzel oder Name</param>
		/// <returns>Aktueller Name</returns>
		public static string GetStateName(string flag)
		{
			switch(flag)
			{
				case "ADRM":
					return "Ostmedirien";
				case "ABR":
				case "BOS":
				case "BSC":
				case "DRB":
				case "Åbro":
				case "Abro":
					return "Boscoulis";
				case "AKM":
					return "Almoravidien";
				case "ADE":
				case "ORA":
				case "AQ":
					return "Oranienbund";
				case "AKS":
					return "Aksai";
				case "AQU":
				case "SNL":
				case "SNL/ALT":
				case "Aquilon":
					return "Neusimmanien";
				case "AMI":
					return "Aminier";
				case "ANT":
					return "Antares";
				case "AZO":
				case "FNZ":
				case "NHI":
				case "FRP":
				case "FNS":
				case "OKA":
				case "NZL":
				case "NZ":
				case "New Halma Islands":
				case "Neu Halmanesien":
				case "Neu-Halmanesien":
				case "Pacifica":
					return "Neuseeland";
				case "ASR":
				case "BTZ":
				case "Astraliana Royalem":
					return "Batazion";
				case "HEB":
					return "Hebridan";
				case "COA":
				case "UAK":
					return "Australien";
				case "AZM":
					return "Azmodan";
				case "SUD":
				case "PATA":
				case "Sudamêrica":
				case "Sudamerica":
					return "Patagonien";
				case "HYL":
				case "HYA":
					return "Hylalien";
				case "LAG":
				case "RL":
				case "RLQ":
					return "Lago";
				case "SEV":
				case "GRSI":
				case "UKSI":
					return "Sevi Island";
				case "EDO":
					return "Eldorado";
				case "STO":
				case "NGT":
				case "MEY":
				case "KLY":
				case "BOU":
				case "Meyham":
				case "Nagato":
				case "Stormic":
					return "Kelyne";
				case "GRA":
				case "Vannekar":
					return "Grafenberg";
				case "BRI":
					return "Barnien";
				case "BRI-AN":
					return "Anglia";
				case "SHI":
				case "HIK":
				case "KNN":
				case "NVC":
				case "Shigoni":
				case "Hikari":
					return "Kanon";
				case "UIP":
				case "RPA":
				case "RPP":
				case "Janinisches Reich":
					return "Papua";
				case "VIR":
				case "4VIR":
				case "TOR":
				case "Virenien":
					return "Toro";
				case "RNL":
				case "AGM":
				case "NLL":
				case "Aggermond":
					return "Neulettland";
				case "RUQ":
				case "KSW":
				case "SOW":
				case "Ruquia":
					return "Sowekien";
				case "KUR":
				case "CSVR":
				case "Kurland":
					return "Caltanien";
				case "KPR":
					return "Preußen";
				case "KRM":
					return "Medirien";
				case "RCH":
				case "NFRC":
				case "FRC":
					return "Chryseum";
				case "BRU":
				case "FVS":
				case "Brûmiasta":
				case "Brumiasta":
					return "Polyessia";
				case "AST":
				case "MAS":
					return "Astana";
				case "BOL1":
				case "BOL":
					return "Bolivarien";
				case "TRU":
				case "MAC1":
				case "MAC":
				case "Trujilo":
					return "Macronien";
				case "EMM":
				case "EMA":
					return "Emmeria";
				case "FL":
				case "FLU":
					return "Flugghingen";
				case "HJH":
				case "SKV":
				case "NDL":
				case "Jotunheim":
				case "Skørnvar":
				case "Skörnvar":
					return "Nordurland";
				case "KOR":
				case "SPA":
					return "Spartan";
				case "GOA":
				case "Sijut":
					return "Goatanien";
				case "SSH":
					return "Singa Shang";
				case "SHIK":
				case "NSI":
					return "Shikanojima";
				case "NUS":
				case "SIM":
				case "Nuestra Senora":
					return "Simultanien";
				case "RBS":
				case "Südburg.":
					return "Südburgund";
				case "URS":
				case "Arancazuelaz":
					return "URS";
				case "IRBU":
				case "KBAB":
					return "Alm. Brumiasta";
				case "MAMA":
					return "Mamba Mamba";
				case "MAU":
					return "Mauritanien";
				case "MEB":
					return "Mitteleuropa";
				case "MEX":
					return "Mexicali";
				case "MIR":
					return "Mirabella";
				case "PKY":
					return "Kyiv";
				case "RIV":
					return "Rivero";
				case "UDV":
				case "VRD":
					return "Damas";
				case "UIB":
					return "Balearen";
				case "USGB":
					return "Grimbergen";
				case "USRV":
					return "Rivera";
				case "WLJ":
					return "Welanja";
				case "YJB":
					return "Yojahbalo";
				case "ZUM":
				case "ZR":
					return "Zumanien";
				case "KBAZ":
					return "Azoren";

				default:
					return flag;
			}
		}
	}
}
