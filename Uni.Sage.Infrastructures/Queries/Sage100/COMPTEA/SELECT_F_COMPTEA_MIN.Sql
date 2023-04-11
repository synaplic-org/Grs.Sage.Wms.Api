SELECT fc.N_Analytique,pa.A_Intitule ,fc.CA_Num Code,fc.CA_Type,fc.CA_Intitule Intitule FROM 
P_ANALYTIQUE pa INNER JOIN F_COMPTEA fc	 ON fc.N_Analytique	 = pa.cbIndice