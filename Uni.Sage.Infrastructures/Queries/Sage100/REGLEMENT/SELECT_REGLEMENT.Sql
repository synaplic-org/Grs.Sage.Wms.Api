select RG_No Id,rg_date [Date],F_CREGLEMENT.CT_NumPayeur [CodeClient],F_COMPTET.CT_Intitule [Client],RG_Reference Reference

,N_Reglement Mode,RG_Montant Montant


from F_CREGLEMENT inner join F_COMPTET on ct_num=F_CREGLEMENT.CT_NumPayeur 
