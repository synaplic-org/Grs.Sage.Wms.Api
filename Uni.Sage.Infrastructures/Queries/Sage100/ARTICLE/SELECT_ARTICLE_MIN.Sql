SELECT ISNULL(FMC.FA_Intitule,'Inconnu') AS Brand ,
AR_Ref as ErpID ,
AR_Design As Name,
FM.FA_CodeFamille As FamilyID , 
AR_Sommeil as IsEnabled,
AR_SuiviStock as StockType 
--unt.U_Intitule Brand
FROM F_FAMILLE  FM 
INNER JOIN	 dbo.F_ARTICLE ON F_ARTICLE.FA_CodeFamille = FM.FA_CodeFamille
LEFT OUTER JOIN F_FAMILLE FMC 
        ON FM.FA_Central =FMC.FA_CodeFamille
INNER JOIN	 dbo.P_UNITE  unt ON unt.cbIndice = F_ARTICLE.AR_UniteVen
