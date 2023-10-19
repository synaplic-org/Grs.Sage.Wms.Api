using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;

namespace Uni.Sage.Infrastructures.Mapper
{
    public  class DepotEmplMapper
    {
        public static DepotEmpResponse Adapts(F_DEPOTEMPL f_DEPOT)
        {
            var result = new DepotEmpResponse();

            return result;
        }
        public static F_DEPOTEMPL Adapt(DepotEmpRequest Request)
        {
            //using var db = _QueryService.NewDbConnection(Request.ConnectionName);
            var result = new F_DEPOTEMPL();
            result.DE_NO = Request.DE_NO;
            result.DP_NO= Request.DP_NO;

            return result;
        }
    }
}
