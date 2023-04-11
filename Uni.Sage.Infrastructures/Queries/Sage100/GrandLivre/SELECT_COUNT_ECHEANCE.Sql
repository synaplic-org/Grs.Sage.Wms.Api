SELECT count(*)

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
	
	
