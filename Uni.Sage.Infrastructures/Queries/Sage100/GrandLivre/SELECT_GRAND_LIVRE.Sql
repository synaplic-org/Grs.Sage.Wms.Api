SELECT entete.DO_Tiers CodeClient
	, F_COMPTET.CT_Intitule IntituleClient
	, entete.DO_Piece NumFacture
	, DO_Date DateFacture
	, DO_TotalTTC-isnull(acc.acc,0) TotalFacture
	, DR_Date dateech
	, req.MontantRegle
	,DO_TotalTTC-isnull(acc.acc,0)-isnull(req.MontantRegle,0) ResteApaye
	, CASE
	  WHEN (DATEDIFF(DAY, DR_Date, GETDATE() ) < 0) THEN 0
	  ELSE DATEDIFF(DAY,  DR_Date, GETDATE())
	  END NbrJr

	FROM F_DOCENTETE entete
	INNER JOIN F_COMPTET ON CT_Num = entete.DO_Tiers
	LEFT OUTER JOIN (
	SELECT F_REGLECH.Do_piece, SUM(F_REGLECH.RC_Montant) MontantRegle
	FROM F_REGLECH
	where RG_TypeReg=0
	GROUP BY F_REGLECH.DO_Piece
	) req ON req.DO_Piece = entete.DO_Piece
	LEFT OUTER JOIN (
	SELECT F_REGLECH.Do_piece, SUM(F_REGLECH.RC_Montant) ACC
	FROM F_REGLECH
	where RG_TypeReg=1
	GROUP BY F_REGLECH.DO_Piece
	) acc ON ACC.DO_Piece = entete.DO_Piece
	left join (select do_piece ,max(DR_Date) DR_Date from F_DOCREGL
	group by do_piece) datech
	on datech.DO_Piece=entete.DO_Piece
	WHERE   entete.DO_Type > 3
	AND entete.DO_Domaine = 0

	
