 select AR_Ref ProductId,LS_NoSerie StockId,LS_Peremption DatePeremption 
 from F_LOTSERIE where DL_NoOut = 0 
 and ar_ref=  @Reference