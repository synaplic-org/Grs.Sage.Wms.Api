using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public  class DepotEmpResponse
    {
        public int DP_NO { get; set; }
        public int DE_NO { get; set; }
        public string DP_Code { get; set; }
        public string DP_Intitule { get; set; }
        public string pConnexionName { get; set; }
    }
}
