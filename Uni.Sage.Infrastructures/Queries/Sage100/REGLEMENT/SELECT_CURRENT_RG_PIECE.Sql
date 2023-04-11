select RG_Piece from F_CREGLEMENT
WHERE cbMarq = (Select MAX(cbMarq)  from F_CREGLEMENT)