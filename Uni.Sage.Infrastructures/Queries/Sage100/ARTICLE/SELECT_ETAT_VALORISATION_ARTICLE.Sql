select f_artstock.AR_Ref ProductId,f_artstock.de_no CodeDepot,
case when AS_QteSto <> 0 then AS_MontSto/AS_QteSto else 0 end   PMP,AR_PrixAch PrixAchat
,DE_Intitule Intitule,isnull(D_Pachat.DL_PrixUnitaire,0) [DernierPachat]
from F_ARTSTOCK
inner join  f_article on f_article.ar_ref=f_artstock.AR_Ref
inner join F_DEPOT on f_depot.DE_No=f_artstock.DE_No
left join
(
select B.AR_Ref,DL_PrixUnitaire from
(
select ar_ref,max(do_date) max_date,max(DL_No) dno
--,de_no
FROM (
select ar_ref,DL_PrixUnitaire,do_piece,do_date,DL_No,DE_No from f_docligne
where do_type=13

union all

select ar_ref,DL_PrixUnitaire,DL_PieceBL,DL_DateBL,DL_No,DE_No  from f_docligne
where do_type in (16,17)
) a

group by ar_ref
--,de_no
) b
inner join f_docligne on f_docligne.dl_no=B.dno
) D_Pachat
on D_Pachat.AR_Ref=F_ARTSTOCK.AR_Ref
where f_artstock.DE_No =@CodeDepot
