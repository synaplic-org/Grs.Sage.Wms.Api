select CO_No ErpID,isnull(CO_Nom,'') Name, isnull(CO_Prenom,'') Prenom,
isnull(CO_Fonction,'') Fonction,isnull(CO_Adresse,'') Adress,isnull(CO_Complement,'') Complement,
isnull(CO_Ville,'') City,isnull(CO_Pays,'') Country ,isnull(CO_Telephone,'') Telephone,isnull(CO_EMail,'') Email,CO_Sommeil IsEnabled
from F_COLLABORATEUR

