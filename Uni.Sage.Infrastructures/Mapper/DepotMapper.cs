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
    public class DepotMapper
    {
        public static DepotResponse Adapts(F_DEPOT f_DEPOT)
        {
            var result = new DepotResponse();
           
            return result;
        }
        public static F_DEPOT Adapt(DepotRequest Request)
        {
            //using var db = _QueryService.NewDbConnection(Request.ConnectionName);
            var result = new F_DEPOT();
            result.DE_Intitule = Request.Name;
            result.DE_Adresse = Request.Adress;
            result.DE_Complement = Request.Complement;
           result.DE_CodePostal = Request.CodePostal ;
           result.DE_Ville = Request.Ville ;
           result.DE_Contact = Request.Contact ;
           result.DE_Region = Request.Region ;
           result.DE_Pays = Request.Pays ;
           result.DE_EMail = Request.Email ;
           //result.DE_Code = Request.Code ;
           result.DE_Telephone = Request.Telephone ;
           result.DE_Telecopie = Request.Telecopie ;

           
            return result;
        }
    }
}
