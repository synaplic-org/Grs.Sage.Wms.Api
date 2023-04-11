select * from (
SELECT CASE WHEN CA_ComptaVen01  LIKE '%Ven%'  then  1 end indice ,CA_ComptaVen01 Comptabilite FROM  P_CATCOMPTA UNION
SELECT CASE WHEN CA_ComptaVen02  LIKE '%Ven%'  then  2 end indice ,CA_ComptaVen02 Comptabilite FROM  P_CATCOMPTA UNION 
SELECT CASE WHEN CA_ComptaVen03 LIKE '%Ex%' then  3 end indice ,CA_ComptaVen03 Comptabilite FROM  P_CATCOMPTA UNION 
SELECT CASE WHEN CA_ComptaVen04  LIKE '%Ven%'  then  4 end indice ,CA_ComptaVen04 Comptabilite FROM  P_CATCOMPTA UNION 
SELECT CASE WHEN CA_ComptaVen05  LIKE '%Ven%'  then  5 end indice ,CA_ComptaVen05 Comptabilite FROM  P_CATCOMPTA  UNION


SELECT CASE WHEN CA_ComptaAch01  LIKE '%Ach%'  then  51 end indice ,CA_ComptaAch01 Comptabilite FROM  P_CATCOMPTA UNION
SELECT CASE WHEN CA_ComptaAch02  LIKE '%Ach%'  then  52 end indice ,CA_ComptaAch02 Comptabilite FROM  P_CATCOMPTA UNION 
SELECT CASE WHEN CA_ComptaAch03 LIKE '%Ex%' then  53 end indice ,CA_ComptaAch03 Comptabilite FROM  P_CATCOMPTA UNION 
SELECT CASE WHEN CA_ComptaAch04  LIKE '%Ach%'  then  54 end indice ,CA_ComptaAch04 Comptabilite FROM  P_CATCOMPTA UNION 
SELECT CASE WHEN CA_ComptaAch05  LIKE '%Ach%'  then  55 end indice ,CA_ComptaAch05 Comptabilite FROM  P_CATCOMPTA 


 )p where  len(Comptabilite)>0;