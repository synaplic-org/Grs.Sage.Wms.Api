select distinct art.AR_Ref as Reference,isnull(ds.PrixVente,art.AR_PrixVen) Prix ,
AR_Design as Designation,
ff.FA_CodeFamille  CodeFamille,
ff.FA_Intitule	   Famille,
ISNULL(ffc.FA_CodeFamille,ff.FA_CodeFamille) CodeCentrale,
ISNULL(ffc.FA_Intitule,ff.FA_Intitule)	   FamilleCentrale,
isnull(artc.CT_Num,'') as CodeClient,
AR_Sommeil as EnSommiel,
AR_Photo as Picture,
AR_SuiviStock as SuiviStock ,
AC_PrixTTC  Type,
AC_Remise as Remise 

 from F_ARTICLE as art 
 left outer join F_ARTCLIENT artc  on art.AR_Ref = artc.AR_Ref
 left outer join P_CATTARIF cat on cat.cbIndice = artc.AC_Categorie
 left outer join F_COMPTET cpt on cpt.CT_Num = artc.CT_Num
 left outer JOIN F_FAMILLE ff ON ff.FA_CodeFamille =art.FA_CodeFamille
 LEFT OUTER JOIN F_FAMILLE ffc ON ff.FA_Central = ffc.FA_CodeFamille

 cross apply 
     (select top 1  clt.CT_Num,clt.N_CatTarif,trf.AR_Ref ,
trf.AC_PrixVen PrixVente,
trf.AC_Remise RemiseVente,
trf.AC_TypeRem TypeRemiseVente

from 
F_COMPTET clt
left outer join F_ARTCLIENT trf on trf.AR_Ref = art.AR_Ref 
and (clt.N_CatTarif = trf.AC_Categorie or clt.CT_Num = trf.CT_Num)   
order by AC_Categorie) as ds
where ff.FA_Type = 0 and AR_Sommeil = 0 and (ffc.FA_CodeFamille =@famille or ff.FA_CodeFamille=@famille )