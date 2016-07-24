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
		public static string SetToSuccessorFlagNoFusion(string flag)
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
				case "FNS":
				case "OKA":
					return "NZL";
				default:
					return flag;
			}
		}
	}
}
