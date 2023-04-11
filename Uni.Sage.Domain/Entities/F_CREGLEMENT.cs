using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public partial class F_CREGLEMENT
    {
            public Nullable<int> RG_No { get; set; }
            public string CT_NumPayeur { get; set; }
            public byte[] cbCT_NumPayeur { get; set; }
            public Nullable<System.DateTime> RG_Date { get; set; }
            public string RG_Reference { get; set; }
            public string RG_Libelle { get; set; }
            public Nullable<decimal> RG_Montant { get; set; }
            public Nullable<decimal> RG_MontantDev { get; set; }
            public Nullable<short> N_Reglement { get; set; }
            public Nullable<short> RG_Impute { get; set; }
            public Nullable<short> RG_Compta { get; set; }
            public Nullable<int> EC_No { get; set; }
            public Nullable<int> cbEC_No { get; set; }
            public Nullable<short> RG_Type { get; set; }
            public Nullable<decimal> RG_Cours { get; set; }
            public Nullable<short> N_Devise { get; set; }
            public string JO_Num { get; set; }
            public string CG_NumCont { get; set; }
            public byte[] cbCG_NumCont { get; set; }
            public Nullable<System.DateTime> RG_Impaye { get; set; }
            public string CG_Num { get; set; }
            public byte[] cbCG_Num { get; set; }
            public Nullable<short> RG_TypeReg { get; set; }
            public string RG_Heure { get; set; }
            public string RG_Piece { get; set; }
            public byte[] cbRG_Piece { get; set; }
            public Nullable<int> CA_No { get; set; }
            public Nullable<int> CO_NoCaissier { get; set; }
            public Nullable<int> cbCO_NoCaissier { get; set; }
            public Nullable<short> RG_Banque { get; set; }
            public Nullable<short> RG_Transfere { get; set; }
            public short RG_Cloture { get; set; }
            public Nullable<short> RG_Ticket { get; set; }
            public Nullable<short> RG_Souche { get; set; }
            public string CT_NumPayeurOrig { get; set; }
            public byte[] cbCT_NumPayeurOrig { get; set; }
            public Nullable<System.DateTime> RG_DateEchCont { get; set; }
            public string CG_NumEcart { get; set; }
            public byte[] cbCG_NumEcart { get; set; }
            public string JO_NumEcart { get; set; }
            public Nullable<decimal> RG_MontantEcart { get; set; }
            public Nullable<int> RG_NoBonAchat { get; set; }
            public Nullable<short> cbProt { get; set; }
            public int cbMarq { get; set; }
            public string cbCreateur { get; set; }
            public Nullable<System.DateTime> cbModification { get; set; }
            public Nullable<int> cbReplication { get; set; }
            public Nullable<short> cbFlag { get; set; }
            public System.DateTime RG_DateCreate { get; set; }
            public string User_reg { get; set; }
            public Nullable<System.DateTime> cbCreation { get; set; }
            public Nullable<System.Guid> cbCreationUser { get; set; }
            public Nullable<short> RG_Valide { get; set; }
            public Nullable<decimal> RG_Anterieur { get; set; }
            public Nullable<decimal> RG_MontantCommission { get; set; }
            public Nullable<decimal> RG_MontantNet { get; set; }
            public byte[] cbHash { get; set; }
            public Nullable<short> cbHashVersion { get; set; }
            public Nullable<System.DateTime> cbHashDate { get; set; }
            public Nullable<int> cbHashOrder { get; set; }
            public Nullable<int> cbCA_No { get; set; }

        public F_CREGLEMENT()
        {
           RG_MontantDev = 0;
            RG_Impute = 1;
            RG_Compta = 0;
            EC_No = 0;
            RG_Type = 0;
            RG_Cours = 0;
            N_Devise = 0;
            RG_Impaye = new DateTime?(Convert.ToDateTime("1753-01-01T00:00:00"));
            RG_TypeReg = 0;
            RG_Heure = DateTime.Now.ToString("000HHmmss");
            CA_No = 0;
            CO_NoCaissier = 0;
            RG_Banque = 0;
            RG_Transfere = 0;
            RG_Cloture = 0;
            RG_Ticket = 0;
            RG_Souche = 0;
            RG_DateEchCont = new DateTime?(Convert.ToDateTime("1753-01-01T00:00:00"));
            RG_MontantEcart = 0;
            RG_NoBonAchat = 0;
            RG_Valide = 1;
            RG_Anterieur = 0;
            RG_MontantCommission = 0;
            RG_MontantNet = 0;
            cbProt = 0;
            cbCreateur = "DEV";
            cbReplication = 0;
            cbFlag = 0;
            cbHashVersion = 1;
            cbHashOrder = 1;
            cbModification = new DateTime?(DateTime.Now.Date);
            cbCreation = new DateTime?(DateTime.Now.Date);
            cbHashDate = new DateTime?(DateTime.Now.Date);



        }
    }
}
