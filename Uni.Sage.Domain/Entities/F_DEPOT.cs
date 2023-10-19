using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public class F_DEPOT
    {
        public int DE_NO { get; set; }
        public string DE_Intitule { get; set; }
        public string DE_Adresse { get; set; }
        public string DE_Complement { get; set; }
        public string DE_CodePostal { get; set; }
        public string DE_Ville { get; set; }
        public string DE_Contact { get; set; }
        public int DE_Principal { get; set; }
        public int DE_CatCompta { get; set; }
        public string DE_Region { get; set; }
        public string DE_Pays { get; set; }
        public string DE_EMail { get; set; }
        public string DE_Code { get; set; }
        public string DE_Telephone { get; set; }
        public string DE_Telecopie { get; set; }
        public int DE_Replication { get; set; }
        public int? DP_NoDefaut { get; set; }
        public int? cbDP_NoDefaut { get; set; }
        public int DE_Exclure { get; set; }
        public int DE_Souche01 { get; set; }
        public int DE_Souche02 { get; set; }
        public int DE_Souche03 { get; set; }
        public int cbProt { get; set; }
        public string cbCreateur { get; set; }
        public DateTime cbModification { get; set; }
        public int cbReplication { get; set; }
        public int   cbFlag { get; set; }
        public DateTime cbCreation { get; set; }
            
        public F_DEPOT()
        {
            DE_Principal = 0;
            DE_CatCompta = 1;
            DE_Replication = 0;
            DP_NoDefaut = null;
            cbDP_NoDefaut = null;
            DE_Exclure = 0;
            DE_Souche01 = 0;
            DE_Souche02 = 0;
            DE_Souche03 = 0;
            cbProt = 0;
            cbCreateur = "DEV";
            cbModification = DateTime.Now;
            cbReplication = 0;
            cbFlag = 0;
            cbCreation = DateTime.Now; 

        }
    }
}
