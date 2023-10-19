using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

namespace Uni.Sage.Domain.Entities
{
    public class F_DEPOTEMPL
    {
        public int DP_NO { get; set; }
        public int DE_NO { get; set; }
        public string DP_Code { get; set; }
        public string DP_Intitule { get; set; }
        public int DP_Zone { get; set; }
        public int DP_Type { get; set; }
        public int cbProt { get; set; }
        public string cbCreateur { get; set; }
        public DateTime cbModification { get; set; }
        public int cbReplication { get; set; }
        public int cbFlag { get; set; }
        public DateTime cbCreation { get; set; }

        public F_DEPOTEMPL()
        {
            DP_Zone = 0;
            DP_Type = 0;
            cbProt= 0;
            cbCreateur = "DEV";
            cbModification = DateTime.Now;
            cbReplication = 0;
            cbFlag = 0;
            cbCreation = DateTime.Now;
            DP_Intitule = "Défaut";
            DP_Code = "DEFAUT";

        }
    }
}
