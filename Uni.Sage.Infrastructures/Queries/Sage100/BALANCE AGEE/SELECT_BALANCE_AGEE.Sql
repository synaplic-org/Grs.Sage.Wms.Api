SELECT fCpteG.CG_Num [NumCompte],fCpteG.CG_Intitule [Intitulé de compte],
	--fCpteG.CG_Saut,fCpteG.N_Nature,
	isnull(fEcgSel.CT_Num,'') [CodeClient],isnull(f_comptet.CT_Intitule,'') client,
	compteSoldeNE,
	comptePeriod1,comptePeriod2,comptePeriod3,comptePeriod4,
	compteSolde
	--,compteRepart
      FROM
      (
            SELECT echSel.cbCG_Num,echSel.CT_Num,
            SUM
            (
                  CASE
                  WHEN ISNULL(echSel.ech,DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date)) < getdate() THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) > 0 AND echSel.N_Nature IN (2,3,6,9) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) < 0 AND echSel.N_Nature IN (1,8) THEN 0
                  ELSE fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) END
            ) compteSoldeNE,
            SUM
            (
                  CASE
                  WHEN ISNULL(echSel.ech,DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date)) < DATEADD(d,-30,getdate()) OR echSel.ech >= getdate() THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) > 0 AND echSel.N_Nature IN (2,3,6,9) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) < 0 AND echSel.N_Nature IN (1,8) THEN 0
                  ELSE fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) END
            ) comptePeriod1,
            SUM
            (
                  CASE
                  WHEN ISNULL(echSel.ech,DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date)) < DATEADD(d,-45,getdate()) OR echSel.ech >= DATEADD(d,-30,getdate()) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) > 0 AND echSel.N_Nature IN (2,3,6,9) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) < 0 AND echSel.N_Nature IN (1,8) THEN 0
                  ELSE fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) END
            ) comptePeriod2,
            SUM
            (
                  CASE
                  WHEN ISNULL(echSel.ech,DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date)) < DATEADD(d,-60,getdate()) OR echSel.ech >= DATEADD(d,-45,getdate()) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) > 0 AND echSel.N_Nature IN (2,3,6,9) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) < 0 AND echSel.N_Nature IN (1,8) THEN 0
                  ELSE fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) END
            ) comptePeriod3,
            SUM
            (
                  CASE
                  WHEN ISNULL(echSel.ech,DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date)) >= DATEADD(d,-60,getdate()) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) > 0 AND echSel.N_Nature IN (2,3,6,9) THEN 0
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) < 0 AND echSel.N_Nature IN (1,8) THEN 0
                  ELSE fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) END
            ) comptePeriod4,
            SUM (
                  fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens)
            ) compteSolde,
            SUM
            (
                  CASE
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) > 0 AND echSel.N_Nature IN (2,3,6,9) THEN ABS(fEcg.EC_Montant)
                  WHEN fEcg.EC_Montant * (1 - 2 * fEcg.EC_Sens) < 0 AND echSel.N_Nature IN (1,8) THEN ABS(fEcg.EC_Montant)
                  ELSE 0
                  END
            ) compteRepart
            FROM
            (
                  SELECT fCpteG2.cbCG_Num,fCpteG2.N_Nature,fEcg.CT_Num,EC_No,
                        CASE WHEN EC_Echeance = '01/01/1753'
                        THEN   DATEADD(d,EC_Jour - 1,JM_Date)
                        ELSE EC_Echeance
                        END ech
                  FROM F_ECRITUREC fEcg
                  INNER JOIN F_COMPTEG fCpteG2
                  ON fCpteG2.CG_Num = fEcg.CG_Num
                  INNER JOIN F_JOURNAUX fCjr
                  ON fEcg.JO_Num = fCjr.JO_Num
                  
                  WHERE fEcg.EC_CType = 0 AND fCpteG2.N_Nature = 1
                  
                  AND (fEcg.EC_Lettre = 0 OR EXISTS (SELECT * FROM F_ECRITUREC fEcg2 WHERE fEcg2.cbCT_Num = fEcg.cbCT_Num
                  AND fEcg2.EC_Lettre = 1 AND DATEADD(d,fEcg2.EC_Jour - 1,fEcg2.JM_Date) > getdate()
                  AND DATEADD(d,fEcg2.EC_Jour - 1,fEcg2.JM_Date) <= DATEFROMPARTS(year(getdate()),12,31)
                  
                  AND fEcg2.cbCG_Num = fEcg.cbCG_Num AND fEcg2.EC_Lettrage = fEcg.EC_Lettrage))
                  
                  and year(fEcg.JM_Date) = year(getdate())
                  
                  
                  --AND DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date) >= '01/01/2022'
                   AND DATEADD(d,fEcg.EC_Jour - 1,fEcg.JM_Date) <= getdate()
                  
                              AND (ISNULL(EC_Norme,0) = 0 OR EC_Norme = 1)
                  
                        AND JO_Type BETWEEN 0 AND 3
            ) echSel
            INNER JOIN F_ECRITUREC fEcg
            ON echSel.EC_No = fEcg.EC_No
            GROUP BY echSel.cbCG_Num,echSel.CT_Num
      ) fEcgSel
      INNER JOIN F_COMPTEG fCpteG
      ON fEcgSel.cbCG_Num = fCpteG.cbCG_Num
      left join f_comptet on f_comptet.ct_num =fEcgSel.CT_Num
      --where fEcgSel.CT_Num = '0011'
      ORDER BY fCpteG.CG_Num,f_comptet.CT_Intitule,fEcgSel.CT_Num ASC