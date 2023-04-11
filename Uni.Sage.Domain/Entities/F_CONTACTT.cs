using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public class F_CONTACTT
    {
        public string CT_Num { get; set; }
        public string CT_Nom { get; set; }
        public string CT_Prenom { get; set; }
        public string CT_Fonction { get; set; }
        public string CT_Telephone { get; set; }
        public string CT_TelPortable { get; set; }
        public string CT_Telecopie { get; set; }
        public string CT_EMail { get; set; }
        public short N_Service { get; set; }
        public short cbProt { get; set; }
        public string cbCreateur { get; set; }
        public Nullable<System.DateTime> cbModification { get; set; }
        public Nullable<System.DateTime> cbCreation { get; set; }
        public short cbReplication { get; set; }
        public short cbFlag { get; set; }
        public short CT_Civilite { get; set; }
        public short N_Contact { get; set; }

        public F_CONTACTT()
        {
            CT_EMail = "";
            cbCreateur = "MA30";
            cbModification = DateTime.Now.Date;
            cbReplication = 0;
            cbFlag = 0;
            cbProt = 0;
            cbCreation = DateTime.Now.Date;
            CT_Telephone = "";
            CT_TelPortable = "";
            CT_Telecopie = "";
            

        }


    }
}
