using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Requests
{
    public  class DepotEmpRequest
    {
        public int DP_NO { get; set; }
        public int DE_NO { get; set; }
      
        public string pConnexionName { get; set; }
    }
}
