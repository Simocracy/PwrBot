using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simocracy.PwrBot
{
	class Simocracy
	{

		/// <summary>
		/// Gibt das Flaggenkürzel des aktuellen Staates bzw. Nachfolgers zurück, die nicht (de)fusioniert wurden bzw. der Nachfolger eindeutig ist.
		/// </summary>
		/// <param name="flag">Kürzel des historischen Staates</param>
		/// <returns>Kürzel des aktuellen Staates</returns>
		public static string ReplaceOldFlag(string flag)
		{
			switch(flag)
			{
				case "ABR":
				case "BOS":
					return "BSC";
				case "ADE":
					return "ORA";
				case "AQU":
					return "SNL";
				case "AZO":
				case "FNZ":
				case "FRP":
				case "FNS":
				case "OKA":
					return "NZL";
				case "ASR":
					return "BTZ";
				case "SUD":
					return "PATA";
				case "HYL":
					return "HYA";
				case "LAG":
					return "RLQ";
				case "SEV":
					return "GRSI";
				case "STO":
				case "NGT":
				case "MEY":
					return "KLY";
				case "SHI":
				case "HIK":
					return "KNN";
				case "UIP":
				case "RPA":
					return "RPP";
				case "VIR":
					return "TOR";
				case "RNL":
				case "AGM":
					return "NLL";
				case "RUQ":
					return "KSW";
				case "KUR":
					return "CSVR";
				case "RCF":
				case "NFRC":
					return "FRC";
				case "BRU":
					return "FVS";
				case "AST":
					return "MAS";
				case "BOL1":
					return "BOL";
				case "TRU":
				case "MAC1":
					return "MAC";
				case "EMM":
					return "EMA";
				case "HJH":
				case "SKV":
					return "NDL";
				case "KOR":
					return "SPA";
				default:
					return flag;
			}
		}

		/// <summary>
		/// Gibt das Flaggenkürzel des aktuellen Staates bzw. Nachfolgers zurück, die nicht (de)fusioniert wurden bzw. der Nachfolger eindeutig ist.
		/// </summary>
		/// <param name="name">Name des historischen Staates</param>
		/// <returns>Name des aktuellen Staates</returns>
		public static string SearchCurrentFlag(string name)
		{
			switch(name)
			{
				case "New Halma Islands":
				case "Neu Halmanesien":
					return "NZL";
				case "Vannekar":
					return "GRA";
				case "Janinisches Reich":
					return "RPP";
				case "Sijut":
					return "GOA";
				case "Singa Shang":
					return "GOA";
				default:
					return "?";
			}
		}
	}
}
