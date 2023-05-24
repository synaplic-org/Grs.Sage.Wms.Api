select distinct FA_CodeFamille as CodeErp,FA_Intitule as Intitule,FA_Type as Type,FA_Central as FamilyCentral,
			CASE
			WHEN FA_Type = 0 THEN   'Detail' 
			WHEN FA_Type = 2 THEN   'Central' 
                        END TypeDescription
			from F_FAMILLE
			where
			FA_Type <> 1