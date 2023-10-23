

select Reference,Article Designation,Sum(stockReel) stockReel,Sum(StockPreparer) StockPreparer,Sum(stockDispo) stockDispo,Sum(Qtecommande) Qtecommande,Sum(QteReserve) QteReserve,Sum(StockATerme) StockATerme,Depot,DE_No from (select Reference,article,A.qte stockReel, isnull( PrepaVente.Qte,0)    StockPreparer,
isnull(a.qte-isnull(PrepaVente.Qte,0),0) stockDispo,isnull(Commande.qte,0) Qtecommande,
isnull(Reserve.Qte ,0)QteReserve, isnull(((A.qte-isnull(PrepaVente.Qte,0))+Commande.Qte) -isnull(Reserve.Qte,0),0) StockATerme,
Depot,a.DE_no,N_lot





--stock preparer = preparation vente
--stock dispo=reeel-preparer vente
--stock commande ¨Preparation ACHAT+BC ACHAT
--stock reserve= bc vente
--stock a terme=dispo+commande-reserve



from (

select stk.AR_Ref Reference,stk.DE_No,LS_Peremption,LS_NoSerie N_lot
,DE_Intitule Depot

,AR_Stat01  Laboratoire,AR_Design Article

      ,sum(qte) qte
      from (
      SELECT
                        F_DOCLIGNE.DE_No,F_DOCLIGNE.DO_Date,F_DOCLIGNE.DL_No,F_DOCLIGNE.DL_No DLNo,
                        F_DOCLIGNE.cbAR_Ref,F_DOCLIGNE.AR_Ref,
                  F_DOCLIGNE.AG_No1 AG_No1, F_DOCLIGNE.AG_No2 AG_No2,
                        ' ' LS_NoSerie,
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
                  FROM  F_DOCLIGNE INNER JOIN F_ARTICLE fArt ON (fArt.cbAR_Ref = F_DOCLIGNE.cbAR_Ref)



                  
                  

                  WHERE
                              (F_DOCLIGNE.DO_Type<5 OR F_DOCLIGNE.DO_Type>5)
                              AND ISNULL(NULLIF(DL_DateBL,{d '1753-01-01'}),DO_Date) <= GETDATE()
                                    AND   (fArt.AR_SuiviStock>=2 AND fArt.AR_SuiviStock<=4)

 
                              AND F_DOCLIGNE.DL_MvtStock > 0
                              
                                    
                              

 UNION all
                  SELECT
                        F_DOCLIGNE.DE_No,F_DOCLIGNE.DO_Date,F_DOCLIGNE.DL_No,fLot.DLNo,
                        F_DOCLIGNE.cbAR_Ref,F_DOCLIGNE.AR_Ref,
      
      
      
      
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

                              FROM  F_DOCLIGNE INNER JOIN F_ARTICLE fArt ON (fArt.cbAR_Ref = F_DOCLIGNE.cbAR_Ref AND fArt.AR_SuiviStock in (1,5) )
                              --LEFT OUTER JOIN
                              
                              inner join (      SELECT DL_NoIn DLNo,DL_NoIn DL_No, LS_Peremption, LS_Fabrication, LS_NoSerie FROM F_LOTSERIE WHERE DL_NoOut = 0
                                                      UNION
                                                      SELECT DL_NoIn DLNo,DL_NoOut DL_No, LS_Peremption, LS_Fabrication, LS_NoSerie FROM F_LOTSERIE WHERE DL_NoOut> 0
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
                                                                                                             ELSE 0     END END) Qte,
                                                                                                SUM(DL_CMUP *(CASE WHEN DL_MvtStock = 1 OR DL_MvtStock = 3 THEN ABS(DL_Qte) ELSE 1 END)
                                                                                                      * CASE WHEN DL_MvtStock = 3 THEN -1 ELSE CASE WHEN DL_MvtStock = 1 THEN
                                                                                                                                                                              CASE WHEN DO_Type=27 AND DL_Qte<0 THEN -1
                                                                                                                                                                   ELSE 1 END
                                                                                                            ELSE CASE WHEN DL_MvtStock = 4 THEN CASE WHEN DO_Domaine = 1 AND (DL_TypePL = 2 OR DL_TypePL=3) THEN 1
                                                                                                                                                                  ELSE -1 END
                                                                                                                   ELSE 1     END   END   END) CMUP
                                                            FROM F_DOCLIGNE ligne INNER JOIN F_ARTICLE ON (ligne.cbAR_Ref = F_ARTICLE.cbAR_Ref)
                                                            WHERE (AR_SuiviStock = 1 OR AR_SuiviStock = 5) AND DL_MvtStock > 0
                                                                  AND ISNULL(NULLIF(DL_DateBL,{d '1753-01-01'}),DO_Date) <= GETDATE()
                                                                  GROUP BY ligne.cbAR_Ref) Calcul
                                                            ON (F_DOCLIGNE.cbAR_Ref = Calcul.cbAR_Ref)


                              
                              

                              WHERE (F_DOCLIGNE.DO_Type<5 OR F_DOCLIGNE.DO_Type>5)
                                          AND ISNULL(NULLIF(DL_DateBL,{d '1753-01-01'}),DO_Date) <= GETDATE()

 
                              AND (F_DOCLIGNE.DL_MvtStock > 0)
                              
                              
                  ) stk
                  inner join f_article on stk.AR_Ref=f_article.AR_Ref
                  inner join f_depot on f_depot.de_no=stk.de_no
                  --where
                  --stk.DE_No <> 1
            

                  group by stk.ar_ref
                  ,stk.DE_No,DE_Intitule,AR_Design,AR_Stat01  
                  ,LS_Peremption,
      LS_NoSerie
      
      --,DLNo
      having sum(qte) >=  0
      ) a
      left outer join (select sum(DL_Qte) Qte,Ar_ref ,DE_No from F_DOCLIGNE where DO_Type=2   group by ar_ref ,DE_No)PrepaVente ON A.Reference=PrepaVente.AR_Ref and a.DE_No=PrepaVente.DE_No

      left outer join (select sum(DL_Qte) Qte,Ar_ref ,DE_No from F_DOCLIGNE where DO_Type in (11,12)   group by ar_ref ,DE_No)Commande ON A.Reference=Commande.AR_Ref and a.DE_No=Commande.DE_No

      left outer join (select sum(DL_Qte) Qte,Ar_ref ,DE_No from F_DOCLIGNE where DO_Type=1   group by ar_ref ,DE_No)Reserve ON A.Reference=Reserve.AR_Ref and a.DE_No=Reserve.DE_No

) Stock
--where Reference  ='LINGOR18'
group by Reference,Article,Depot,DE_No
