
"""
Convert Simocracy Flags
Note: Methods automatic converted from C# code
"""

"""
Gibt das Flaggenkürzel des aktuellen Staates bzw. Nachfolgers zurück, die nicht (de)fusioniert wurden bzw. der Nachfolger eindeutig ist.
Flag:  Kürzel oder Name des historischen Staates
Returns: Kürzel des aktuellen Staates
"""
def GetFlag(flag):
	if flag == "ABR" or flag == "BOS" or flag == "DRB":
		return "BSC"
	elif flag == "ADE":
		return "ORA"
	elif flag == "AQU":
		return "SNL"
	elif flag == "AZO" or flag == "FNZ" or flag == "NHI" or flag == "FRP" or flag == "FNS" or flag == "OKA" or flag == "NZ" or flag == "New Halma Islands" or flag == "Neu Halmanesien" or flag == "Neu-Halmanesien" or flag == "Pacifica":
		return "NZL"
	elif flag == "ASR":
		return "BTZ"
	elif flag == "SUD":
		return "PATA"
	elif flag == "HYL":
		return "HYA"
	elif flag == "LAG":
		return "RLQ"
	elif flag == "SEV":
		return "GRSI"
	elif flag == "STO" or flag == "NGT" or flag == "MEY" or flag == "BOU":
		return "KLY"
	elif flag == "Vannekar":
		return "GRA"
	elif flag == "SHI" or flag == "HIK":
		return "KNN"
	elif flag == "UIP" or flag == "RPA" or flag == "Janinisches Reich":
		return "RPP"
	elif flag == "VIR" or flag == "4VIR":
		return "TOR"
	elif flag == "RNL" or flag == "AGM":
		return "NLL"
	elif flag == "RUQ" or flag == "SOW":
		return "KSW"
	elif flag == "KUR":
		return "CSVR"
	elif flag == "RCF" or flag == "NFRC":
		return "FRC"
	elif flag == "BRU":
		return "FVS"
	elif flag == "AST":
		return "MAS"
	elif flag == "BOL1":
		return "BOL"
	elif flag == "TRU" or flag == "MAC1":
		return "MAC"
	elif flag == "EMM":
		return "EMA"
	elif flag == "HJH" or flag == "SKV":
		return "NDL"
	elif flag == "KOR":
		return "SPA"
	elif flag == "Sijut":
		return "GOA"
	elif flag == "SIM" or flag == "Nuestra Senora" or flag == "Simultanien":
		return "NUS"
	elif flag == "Azoren":
		return "KBAZ"
	else:
		return flag

"""
Gibt den Staatsnamen des angegebenen Kürzels oder historischen Staates zurück
flag: Historisches Kürzel oder Name
return: Aktueller Name
"""
def GetStateName(flag):
	if flag == "ADRM":
		return "Ostmedirien"
	elif flag == "ABR" or flag == "BOS" or flag == "BSC" or flag == "DRB" or flag == "Åbro" or flag == "Abro":
		return "Boscoulis"
	elif flag == "AKM":
		return "Almoravidien"
	elif flag == "ADE" or flag == "ORA" or flag == "AQ":
		return "Oranienbund"
	elif flag == "AKS":
		return "Aksai"
	elif flag == "AQU" or flag == "SNL" or flag == "SNL/ALT" or flag == "Aquilon":
		return "Neusimmanien"
	elif flag == "AMI":
		return "Aminier"
	elif flag == "ANT":
		return "Antares"
	elif flag == "AZO" or flag == "FNZ" or flag == "NHI" or flag == "FRP" or flag == "FNS" or flag == "OKA" or flag == "NZL" or flag == "NZ" or flag == "New Halma Islands" or flag == "Neu Halmanesien" or flag == "Neu-Halmanesien" or flag == "Pacifica":
		return "Neuseeland"
	elif flag == "ASR" or flag == "BTZ" or flag == "Astraliana Royalem":
		return "Batazion"
	elif flag == "HEB":
		return "Hebridan"
	elif flag == "COA" or flag == "UAK":
		return "Australien"
	elif flag == "AZM":
		return "Azmodan"
	elif flag == "SUD" or flag == "PATA" or flag == "Sudamêrica" or flag == "Sudamerica":
		return "Patagonien"
	elif flag == "HYL" or flag == "HYA":
		return "Hylalien"
	elif flag == "LAG" or flag == "RL" or flag == "RLQ":
		return "Lago"
	elif flag == "SEV" or flag == "GRSI" or flag == "UKSI":
		return "Sevi Island"
	elif flag == "EDO":
		return "Eldorado"
	elif flag == "STO" or flag == "NGT" or flag == "MEY" or flag == "KLY" or flag == "BOU" or flag == "Meyham" or flag == "Nagato" or flag == "Stormic":
		return "Kelyne"
	elif flag == "GRA" or flag == "Vannekar":
		return "Grafenberg"
	elif flag == "BRI":
		return "Barnien"
	elif flag == "BRI-AN":
		return "Anglia"
	elif flag == "SHI" or flag == "HIK" or flag == "KNN" or flag == "NVC" or flag == "Shigoni" or flag == "Hikari":
		return "Kanon"
	elif flag == "UIP" or flag == "RPA" or flag == "RPP" or flag == "Janinisches Reich":
		return "Papua"
	elif flag == "VIR" or flag == "4VIR" or flag == "TOR" or flag == "Virenien":
		return "Toro"
	elif flag == "RNL" or flag == "AGM" or flag == "NLL" or flag == "Aggermond":
		return "Neulettland"
	elif flag == "RUQ" or flag == "KSW" or flag == "SOW" or flag == "Ruquia":
		return "Sowekien"
	elif flag == "KUR" or flag == "CSVR" or flag == "Kurland":
		return "Caltanien"
	elif flag == "KPR":
		return "Preußen"
	elif flag == "KRM":
		return "Medirien"
	elif flag == "RCH" or flag == "NFRC" or flag == "FRC":
		return "Chryseum"
	elif flag == "BRU" or flag == "FVS" or flag == "Brûmiasta" or flag == "Brumiasta":
		return "Polyessia"
	elif flag == "AST" or flag == "MAS":
		return "Astana"
	elif flag == "BOL1" or flag == "BOL":
		return "Bolivarien"
	elif flag == "TRU" or flag == "MAC1" or flag == "MAC" or flag == "Trujilo":
		return "Macronien"
	elif flag == "EMM" or flag == "EMA":
		return "Emmeria"
	elif flag == "FL" or flag == "FLU":
		return "Flugghingen"
	elif flag == "HJH" or flag == "SKV" or flag == "NDL" or flag == "Jotunheim" or flag == "Skørnvar" or flag == "Skörnvar":
		return "Nordurland"
	elif flag == "KOR" or flag == "SPA":
		return "Spartan"
	elif flag == "GOA" or flag == "Sijut":
		return "Goatanien"
	elif flag == "SSH":
		return "Singa Shang"
	elif flag == "SHIK" or flag == "NSI":
		return "Shikanojima"
	elif flag == "NUS" or flag == "SIM" or flag == "Nuestra Senora":
		return "Simultanien"
	elif flag == "RBS" or flag == "Südburg.":
		return "Südburgund"
	elif flag == "URS" or flag == "Arancazuelaz":
		return "URS"
	elif flag == "IRBU" or flag == "KBAB":
		return "Alm. Brumiasta"
	elif flag == "MAMA":
		return "Mamba Mamba"
	elif flag == "MAU":
		return "Mauritanien"
	elif flag == "MEB":
		return "Mitteleuropa"
	elif flag == "MEX":
		return "Mexicali"
	elif flag == "MIR":
		return "Mirabella"
	elif flag == "PKY":
		return "Kyiv"
	elif flag == "RIV":
		return "Rivero"
	elif flag == "UDV" or flag == "VRD":
		return "Damas"
	elif flag == "UIB":
		return "Balearen"
	elif flag == "USGB":
		return "Grimbergen"
	elif flag == "USRV":
		return "Rivera"
	elif flag == "WLJ":
		return "Welanja"
	elif flag == "YJB":
		return "Yojahbalo"
	elif flag == "ZUM" or flag == "ZR":
		return "Zumanien"
	elif flag == "KBAZ":
		return "Azoren"
	else:
		return flag