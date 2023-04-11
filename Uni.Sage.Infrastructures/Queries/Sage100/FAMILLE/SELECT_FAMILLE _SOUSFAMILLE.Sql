SELECT distinct 
		ff.FA_CodeFamille  CodeFamille,
		ff.FA_Intitule	   Famille,
		ISNULL(ffc.FA_CodeFamille,ff.FA_CodeFamille) CodeCentrale,
		ISNULL(ffc.FA_Intitule,ff.FA_Intitule)	   FamilleCentrale,
		CASE
			WHEN ff.FA_Type = 0 THEN   'Detail' 
			WHEN ff.FA_Type = 2 THEN   'Central' 
                        END TypeDescription
FROM
		F_FAMILLE ff
	inner join F_ARTICLE art on ff.FA_CodeFamille = art.FA_CodeFamille and art.AR_Sommeil = 0
	LEFT OUTER JOIN
		F_FAMILLE ffc
			ON ff.FA_Central = ffc.FA_CodeFamille
			WHERE  ff.FA_Type <> 1