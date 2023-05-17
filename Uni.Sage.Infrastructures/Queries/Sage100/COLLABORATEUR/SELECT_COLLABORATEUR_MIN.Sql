select CO_No CodeCollaborateur,isnull(CO_Matricule,'') Matricule,isnull(CO_Nom,'') Nom, isnull(CO_Prenom,'') Prenom,
isnull(CO_Fonction,'') Fonction,isnull(CO_Adresse,'') Adresse,isnull(CO_Complement,'') Complement,isnull(CO_CodePostal,'') CodePostal,
isnull(CO_Ville,'') Ville,isnull(CO_Service,'') Service,isnull(CO_Telephone,'') Telephone
,isnull(CO_TelPortable,'') TelPortable,isnull(CO_EMail,'') Email  from F_COLLABORATEUR
where CO_Sommeil = 0
