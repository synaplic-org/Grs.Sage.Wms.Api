SELECT top 1 max(DE_No) depot from f_depot 
group by DE_No
order by DE_No desc