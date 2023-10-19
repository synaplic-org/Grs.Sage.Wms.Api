SELECT top 1 max(DP_No + 1) DP_NO from F_DEPOTEMPL 
group by DP_No
order by DP_No desc