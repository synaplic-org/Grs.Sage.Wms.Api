--select F_ARTICLE.AR_Ref Reference,F_ARTICLE.AR_Design Designation,AS_QteSto QuantiteReel,AS_MontSto Montant,arts.DE_NO CodeDepot,dpt.DE_Intitule Depot  from F_ARTICLE 
--inner  join F_ARTSTOCK arts on arts.AR_Ref = F_ARTICLE.ar_ref
--inner join  F_DEPOT dpt on dpt.de_no = arts.DE_No


select f_artstock.AR_Ref ErpId,art.AR_Design Name  ,AS_QteSto QuantiteReel,f_artstock.DE_No WareHouseID ,dpt.DE_Intitule WareHouseName from f_artstock 
inner join F_ARTICLE art on art.AR_Ref = F_ARTSTOCK.AR_Ref
inner join f_depot dpt on dpt.DE_No = f_artstock.DE_No

--where f_artstock.AR_Ref = @ErpId and f_artstock.DE_No = @WareHouseID