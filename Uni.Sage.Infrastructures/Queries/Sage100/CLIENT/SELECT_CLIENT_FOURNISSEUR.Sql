select 
       client.CT_num ErpID,
       client.CT_Intitule Name,
     client.CT_Type PartyType,
       --client.CG_NumPrinc CollectiveAccount,
       trf.CT_Intitule Tarif,
       client.CT_Contact Contact,
       client.CT_Telephone Telephone,
       client.CT_Pays City,
       client.CT_Ville Country,
       client.CT_Adresse  Adress,
       client.CT_Identifiant Identifiant,
       client.CO_No CollaboratorID,
       client.DE_No DefaultWareHouseErpID,
	     client.CT_Sommeil IsEnabled

from F_COMPTET client
    
    left outer join P_CATTARIF trf
        on client.N_CatTarif = trf.cbIndice
where  CT_Sommeil=0 and CT_Type in (0,1)