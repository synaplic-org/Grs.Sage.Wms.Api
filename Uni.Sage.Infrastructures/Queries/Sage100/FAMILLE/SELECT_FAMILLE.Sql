﻿select distinct FA_CodeFamille as CodeFamille,FA_Intitule as Intitule,FA_Type as Type,FA_Central as FamilleCentrale,
			CASE
			WHEN FA_Type = 0 THEN   'Detail' 
			WHEN FA_Type = 2 THEN   'Central' 
                        END TypeDescription
			from F_FAMILLE
			where
			FA_Type <> 1