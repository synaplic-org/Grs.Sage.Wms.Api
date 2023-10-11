select 

Article.DE_No,
fDep.DE_Intitule Depot,
fArt.AR_Ref Reference,
fArt.AR_Design Article,
famcent,
fArt.FA_CodeFamille FA_CodeFamille,f_famille.FA_Intitule famille,

Article.LS_NoSerie N_lot,
Article.LS_Peremption,
Article.LS_Fabrication,
sum(Article.Qte) qte
--,Article.CMUP cmup_global,
--Article.CMUP/Article.Qte  CMUP,
--Article.PR

FROM
(

SELECT
sousReqSelLigne.DE_No,
'''' IntituleTri, '''' IntituleTri2,
sousReqSelLigne.cbAR_Ref,
sousReqSelLigne.AG_No1 AG_No1, sousReqSelLigne.AG_No2 AG_No2,
fgam1.EG_Enumere Enumere1 ,fgam2.EG_Enumere Enumere2,
CASE WHEN sousReqSelLigne.AG_No1 <> fArtEnumRef.AG_No1 OR sousReqSelLigne.AG_No2 <> fArtEnumRef.AG_No2 THEN '''' ELSE AE_Ref END AE_Ref,
    LS_NoSerie,LS_Peremption,LS_Fabrication,

SUM(Qte) Qte,
SUM(CMUP*Sens) CMUP,
SUM(PR*Sens) PR,
SUM(PRUnitaire * Sens) PRUnitaire,
SUM(Qte2) Qte2
FROM
(
SELECT
F_DOCLIGNE.DE_No,
F_DOCLIGNE.cbAR_Ref,
F_DOCLIGNE.AG_No1 AG_No1, F_DOCLIGNE.AG_No2 AG_No2,
'''' LS_NoSerie,
{d '1753-01-01'} LS_Peremption,
{d '1753-01-01'} LS_Fabrication,
( CASE WHEN DL_MvtStock = 3 THEN
CASE WHEN DO_Type = 14 THEN
-F_DOCLIGNE.DL_Qte
ELSE CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR F_DOCLIGNE.DL_Qte<0 THEN
F_DOCLIGNE.DL_Qte
ELSE
-F_DOCLIGNE.DL_Qte
END
END
END

ELSE
CASE WHEN DL_MvtStock = 1 THEN
CASE WHEN DO_Type = 27 OR DO_Type = 4 THEN
F_DOCLIGNE.DL_Qte
ELSE CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE CASE WHEN DL_TypePL>0 OR (F_DOCLIGNE.DL_Qte<0 AND DO_Domaine<>2) THEN
-F_DOCLIGNE.DL_Qte
 ELSE
F_DOCLIGNE.DL_Qte
 END
   END
END
ELSE
0
END
END) Qte,
(CASE WHEN DL_MvtStock = 3 OR DL_MvtStock = 1 THEN
ROUND(CAST(DL_CMUP * ABS(F_DOCLIGNE.DL_Qte) as numeric(24,6)),2)
ELSE
DL_CMUP
END) CMUP,


 
(CASE WHEN DL_MvtStock = 3 OR DL_MvtStock = 1 THEN
ROUND(CAST(DL_CMUP * ABS(F_DOCLIGNE.DL_Qte) as numeric(24,6)),2)
ELSE
DL_CMUP
END
)
         PR,
(CASE WHEN DL_MvtStock = 3 OR DL_MvtStock = 1 THEN
ROUND(CAST(DL_CMUP * ABS(F_DOCLIGNE.DL_Qte) as numeric(24,6)),2)
ELSE
DL_CMUP
END
) PRUnitaire,
ABS(F_DOCLIGNE.DL_Qte) Qte2,


(CASE WHEN DL_MvtStock = 3 THEN
-1
ELSE
CASE WHEN DL_MvtStock = 1 THEN
CASE WHEN DO_Type=27 AND F_DOCLIGNE.DL_Qte<0 THEN
-1
ELSE
1
END
ELSE
CASE WHEN DL_MvtStock = 4 THEN
CASE WHEN DO_Domaine = 1 AND (DL_TypePL = 2 OR DL_TypePL=3) THEN
1
ELSE
-1
END
ELSE
1
END
END
END) Sens
FROM F_DOCLIGNE INNER JOIN F_ARTICLE fArt ON (fArt.cbAR_Ref = F_DOCLIGNE.cbAR_Ref)




WHERE
(F_DOCLIGNE.DO_Type<5 OR F_DOCLIGNE.DO_Type>5)
AND ISNULL(NULLIF(DL_DateBL,{d '1753-01-01'}),DO_Date) <=  getdate()
AND (fArt.AR_SuiviStock>=2 AND fArt.AR_SuiviStock<=4)

 
AND F_DOCLIGNE.DL_MvtStock > 0

 UNION all
SELECT
F_DOCLIGNE.DE_No,
F_DOCLIGNE.cbAR_Ref,
F_DOCLIGNE.AG_No1,
F_DOCLIGNE.AG_No2,
ISNULL(fLot.LS_NoSerie,''),
ISNULL(fLot.LS_Peremption,{d '1753-01-01'}),
ISNULL(fLot.LS_Fabrication,{d '1753-01-01'}),
 ( CASE WHEN DL_MvtStock = 3 THEN
CASE WHEN DO_Type = 14 THEN
-F_DOCLIGNE.DL_Qte
ELSE CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR F_DOCLIGNE.DL_Qte<0 THEN
F_DOCLIGNE.DL_Qte
ELSE
-F_DOCLIGNE.DL_Qte
END
END
END
 ELSE
CASE WHEN DL_MvtStock = 1 THEN
CASE WHEN DO_Type = 27 OR DO_Type = 4 THEN
F_DOCLIGNE.DL_Qte
ELSE CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE CASE WHEN DL_TypePL>0 OR (F_DOCLIGNE.DL_Qte<0 AND DO_Domaine<>2) THEN
-F_DOCLIGNE.DL_Qte
 ELSE
F_DOCLIGNE.DL_Qte
 END
   END
END
ELSE
0
END
END) Qte,
(CASE WHEN DL_MvtStock = 3 OR DL_MvtStock = 1 THEN
ROUND(CAST(DL_CMUP * ABS(F_DOCLIGNE.DL_Qte) as numeric(24,6)),2)
ELSE
DL_CMUP
END) CMUP,



((CASE WHEN DL_MvtStock = 3 THEN
CASE WHEN DO_Type = 14 THEN
-F_DOCLIGNE.DL_Qte
ELSE
CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR F_DOCLIGNE.DL_Qte<0 THEN
F_DOCLIGNE.DL_Qte
ELSE
-F_DOCLIGNE.DL_Qte
END
END
END
ELSE
CASE WHEN DL_MvtStock = 1 THEN
CASE WHEN DO_Type = 27 OR DO_Type = 4 THEN
F_DOCLIGNE.DL_Qte
ELSE
CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR (F_DOCLIGNE.DL_Qte<0 AND DO_Domaine<>2) THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
END
END
ELSE
1
END
END) * (CASE WHEN Calcul.Qte <> 0 THEN  Calcul.CMUP/Calcul.Qte ELSE Calcul.CMUP END))



PR,



((CASE WHEN DL_MvtStock = 3 THEN
CASE WHEN DO_Type = 14 THEN
-F_DOCLIGNE.DL_Qte
ELSE
CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR F_DOCLIGNE.DL_Qte<0 THEN
F_DOCLIGNE.DL_Qte
ELSE
-F_DOCLIGNE.DL_Qte
END
END
END
ELSE
CASE WHEN DL_MvtStock = 1 THEN
CASE WHEN DO_Type = 27 OR DO_Type = 4 THEN
F_DOCLIGNE.DL_Qte
ELSE
CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR (F_DOCLIGNE.DL_Qte<0 AND DO_Domaine<>2) THEN
-F_DOCLIGNE.DL_Qte
ELSE
F_DOCLIGNE.DL_Qte
END
END
END
ELSE
1
END
END) * (CASE WHEN Calcul.Qte <> 0 THEN  Calcul.CMUP/Calcul.Qte ELSE Calcul.CMUP END))



PRUnitaire,
CASE WHEN DL_MvtStock = 3 OR DL_MvtStock = 1 THEN
ABS(F_DOCLIGNE.DL_Qte)
ELSE 1 END Qte2,
1 Sens

FROM F_DOCLIGNE INNER JOIN F_ARTICLE fArt ON (fArt.cbAR_Ref = F_DOCLIGNE.cbAR_Ref AND fArt.AR_SuiviStock in (1,5) )
--LEFT OUTER JOIN
inner join ( SELECT DL_NoIn DL_No, LS_Peremption, LS_Fabrication, LS_NoSerie FROM F_LOTSERIE WHERE DL_NoOut = 0
UNION
SELECT DL_NoOut DL_No, LS_Peremption, LS_Fabrication, LS_NoSerie FROM F_LOTSERIE WHERE DL_NoOut> 0
) fLot ON (F_DOCLIGNE.DL_No = fLot.DL_No)
LEFT OUTER JOIN (SELECT ligne.cbAR_Ref,SUM(CASE WHEN DL_MvtStock = 3 THEN CASE WHEN DO_Type=14 THEN
-DL_Qte
ELSE
CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN -DL_Qte
ELSE DL_Qte
END
ELSE
CASE WHEN DL_TypePL>0 OR DL_Qte<0 THEN
DL_Qte
ELSE -DL_Qte END
END
END
ELSE CASE WHEN DL_MvtStock = 1 THEN CASE WHEN DO_Type = 27 OR DO_Type = 4 THEN
DL_Qte
ELSE CASE WHEN DL_TypePL = 4 THEN
CASE WHEN DO_Domaine = 0 THEN -DL_Qte
ELSE DL_Qte END
ELSE
CASE WHEN DL_TypePL>0 OR (DL_Qte<0 AND DO_Domaine<>2) THEN
-DL_Qte ELSE DL_Qte END
END
END
ELSE 0 END END) Qte,
SUM(DL_CMUP *(CASE WHEN DL_MvtStock = 1 OR DL_MvtStock = 3 THEN ABS(DL_Qte) ELSE 1 END)
* CASE WHEN DL_MvtStock = 3 THEN -1 ELSE CASE WHEN DL_MvtStock = 1 THEN
CASE WHEN DO_Type=27 AND DL_Qte<0 THEN -1
ELSE 1 END
ELSE CASE WHEN DL_MvtStock = 4 THEN CASE WHEN DO_Domaine = 1 AND (DL_TypePL = 2 OR DL_TypePL=3) THEN 1
ELSE -1 END
ELSE 1 END END END) CMUP
FROM F_DOCLIGNE ligne INNER JOIN F_ARTICLE ON (ligne.cbAR_Ref = F_ARTICLE.cbAR_Ref)
WHERE (AR_SuiviStock = 1 OR AR_SuiviStock = 5) AND DL_MvtStock > 0
AND ISNULL(NULLIF(DL_DateBL,{d '1753-01-01'}),DO_Date) <=  getdate()
GROUP BY ligne.cbAR_Ref) Calcul
ON (F_DOCLIGNE.cbAR_Ref = Calcul.cbAR_Ref)



WHERE (F_DOCLIGNE.DO_Type<5 OR F_DOCLIGNE.DO_Type>5)
AND ISNULL(NULLIF(DL_DateBL,{d '1753-01-01'}),DO_Date) <=  getdate()

 
AND (F_DOCLIGNE.DL_MvtStock > 0)

)

sousReqSelLigne
LEFT OUTER JOIN F_ARTGAMME fgam1 ON (sousReqSelLigne.AG_No1=fgam1.AG_No AND fgam1.AG_Type = 0)
LEFT OUTER JOIN F_ARTGAMME fgam2 ON (sousReqSelLigne.AG_No2 = fgam2.AG_No AND fgam2.AG_Type = 1)
LEFT OUTER JOIN F_ARTENUMREF fArtEnumRef ON (sousReqSelLigne.AG_No1 = fArtEnumRef.AG_No1 AND sousReqSelLigne.AG_No2 = fArtEnumRef.AG_No2 AND sousReqSelLigne.cbAR_Ref = fArtEnumRef.cbAR_Ref)


GROUP BY
sousReqSelLigne.DE_No,
sousReqSelLigne.cbAR_Ref,
sousReqSelLigne.AG_No1,
sousReqSelLigne.AG_No2
,fgam1.EG_Enumere,
fgam2.EG_Enumere,
CASE WHEN sousReqSelLigne.AG_No1 <> fArtEnumRef.AG_No1 OR sousReqSelLigne.AG_No2 <> fArtEnumRef.AG_No2 THEN '''' ELSE AE_Ref END
,LS_NoSerie,
LS_Peremption,
LS_Fabrication
HAVING  SUM(Qte) > 0    
) Article INNER JOIN F_ARTICLE fArt ON (Article.cbAR_Ref = fArt.cbAR_Ref)
INNER JOIN F_famille on fArt.FA_CodeFamille=f_famille.FA_CodeFamille
left join ( select fa_codefamille,fa_intitule famcent from f_famille where fa_type=2) facent
 on facent.FA_CodeFamille=F_FAMILLE.FA_Central
INNER JOIN F_DEPOT fDep ON (Article.DE_No = fDep.DE_No)
where fArt.AR_Sommeil = 0
--and Article.DE_No in (1,2)
and AR_Ref = @Reference

group by Article.DE_No,
fDep.DE_Intitule ,
fArt.AR_Ref ,
fArt.AR_Design  ,
famcent,
fArt.FA_CodeFamille ,f_famille.FA_Intitule ,
Article.LS_NoSerie ,
Article.LS_Peremption,
Article.LS_Fabrication


