using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public  class F_COMPTET
    {
        public string CT_Num { get; set; }
        public byte[] cbCT_Num { get; set; }
        public string CT_Intitule { get; set; }
        public Nullable<short> CT_Type { get; set; }
        public string CG_NumPrinc { get; set; }
        public string CT_Qualite { get; set; }
        public string CT_Classement { get; set; }
        public byte[] cbCT_Classement { get; set; }
        public string CT_Contact { get; set; }
        public string CT_Adresse { get; set; }
        public string CT_Complement { get; set; }
        public string CT_CodePostal { get; set; }
        public byte[] cbCT_CodePostal { get; set; }
        public string CT_Ville { get; set; }
        public string CT_CodeRegion { get; set; }
        public string CT_Pays { get; set; }
        public string CT_Raccourci { get; set; }
        public byte[] cbCT_Raccourci { get; set; }
        public Nullable<short> BT_Num { get; set; }
        public Nullable<short> N_Devise { get; set; }
        public string CT_Ape { get; set; }
        public string CT_Identifiant { get; set; }
        public string CT_Siret { get; set; }
        public string CT_Statistique01 { get; set; }
        public string CT_Statistique02 { get; set; }
        public string CT_Statistique03 { get; set; }
        public string CT_Statistique04 { get; set; }
        public string CT_Statistique05 { get; set; }
        public string CT_Statistique06 { get; set; }
        public string CT_Statistique07 { get; set; }
        public string CT_Statistique08 { get; set; }
        public string CT_Statistique09 { get; set; }
        public string CT_Statistique10 { get; set; }
        public string CT_Commentaire { get; set; }
        public Nullable<decimal> CT_Encours { get; set; }
        public Nullable<decimal> CT_Assurance { get; set; }
        public string CT_NumPayeur { get; set; }
        public byte[] cbCT_NumPayeur { get; set; }
        public Nullable<short> N_Risque { get; set; }
        public Nullable<int> CO_No { get; set; }
        public Nullable<int> cbCO_No { get; set; }
        public Nullable<short> N_CatTarif { get; set; }
        public Nullable<decimal> CT_Taux01 { get; set; }
        public Nullable<decimal> CT_Taux02 { get; set; }
        public Nullable<decimal> CT_Taux03 { get; set; }
        public Nullable<decimal> CT_Taux04 { get; set; }
        public Nullable<short> N_CatCompta { get; set; }
        public Nullable<short> N_Period { get; set; }
        public Nullable<short> CT_Facture { get; set; }
        public Nullable<short> CT_BLFact { get; set; }
        public Nullable<short> CT_Langue { get; set; }
        public Nullable<short> N_Expedition { get; set; }
        public Nullable<short> N_Condition { get; set; }
        public Nullable<short> CT_Saut { get; set; }
        public Nullable<short> CT_Lettrage { get; set; }
        public Nullable<short> CT_ValidEch { get; set; }
        public Nullable<short> CT_Sommeil { get; set; }
        public Nullable<int> DE_No { get; set; }
        public Nullable<int> cbDE_No { get; set; }
        public Nullable<short> CT_ControlEnc { get; set; }
        public Nullable<short> CT_NotRappel { get; set; }
        public Nullable<short> N_Analytique { get; set; }
        public Nullable<short> cbN_Analytique { get; set; }
        public string CA_Num { get; set; }
        public byte[] cbCA_Num { get; set; }
        public string CT_Telephone { get; set; }
        public string CT_Telecopie { get; set; }
        public string CT_EMail { get; set; }
        public string CT_Site { get; set; }
        public string CT_Coface { get; set; }
        public Nullable<short> CT_Surveillance { get; set; }
        public Nullable<System.DateTime> CT_SvDateCreate { get; set; }
        public string CT_SvFormeJuri { get; set; }
        public string CT_SvEffectif { get; set; }
        public Nullable<decimal> CT_SvCA { get; set; }
        public Nullable<decimal> CT_SvResultat { get; set; }
        public Nullable<short> CT_SvIncident { get; set; }
        public Nullable<System.DateTime> CT_SvDateIncid { get; set; }
        public Nullable<short> CT_SvPrivil { get; set; }
        public string CT_SvRegul { get; set; }
        public string CT_SvCotation { get; set; }
        public Nullable<System.DateTime> CT_SvDateMaj { get; set; }
        public string CT_SvObjetMaj { get; set; }
        public Nullable<System.DateTime> CT_SvDateBilan { get; set; }
        public Nullable<short> CT_SvNbMoisBilan { get; set; }
        public Nullable<short> N_AnalytiqueIFRS { get; set; }
        public Nullable<short> cbN_AnalytiqueIFRS { get; set; }
        public string CA_NumIFRS { get; set; }
        public Nullable<short> CT_PrioriteLivr { get; set; }
        public Nullable<short> CT_LivrPartielle { get; set; }
        public Nullable<int> MR_No { get; set; }
        public Nullable<int> cbMR_No { get; set; }
        public Nullable<short> CT_NotPenal { get; set; }
        public Nullable<int> EB_No { get; set; }
        public Nullable<int> cbEB_No { get; set; }
        public string CT_NumCentrale { get; set; }
        public byte[] cbCT_NumCentrale { get; set; }
        public Nullable<System.DateTime> CT_DateFermeDebut { get; set; }
        public Nullable<System.DateTime> CT_DateFermeFin { get; set; }
        public Nullable<short> CT_FactureElec { get; set; }
        public Nullable<short> CT_TypeNIF { get; set; }
        public string CT_RepresentInt { get; set; }
        public string CT_RepresentNIF { get; set; }
        public Nullable<short> CT_EdiCodeType { get; set; }
        public string CT_EdiCode { get; set; }
        public string CT_EdiCodeSage { get; set; }
        public Nullable<short> CT_ProfilSoc { get; set; }
        public Nullable<short> CT_StatutContrat { get; set; }
        public Nullable<System.DateTime> CT_DateMAJ { get; set; }
        public Nullable<short> CT_EchangeRappro { get; set; }
        public Nullable<short> CT_EchangeCR { get; set; }
        public Nullable<int> PI_NoEchange { get; set; }
        public Nullable<int> cbPI_NoEchange { get; set; }
        public Nullable<short> CT_BonAPayer { get; set; }
        public Nullable<short> CT_DelaiTransport { get; set; }
        public Nullable<short> CT_DelaiAppro { get; set; }
        public string CT_LangueISO2 { get; set; }
        public Nullable<short> CT_AnnulationCR { get; set; }
        public Nullable<short> cbProt { get; set; }
        public int cbMarq { get; set; }
        public string cbCreateur { get; set; }
        public Nullable<System.DateTime> cbModification { get; set; }
        public Nullable<int> cbReplication { get; set; }
        public Nullable<short> cbFlag { get; set; }
        public Nullable<System.DateTime> cbCreation { get; set; }
        public Nullable<System.Guid> cbCreationUser { get; set; }
        public string CT_Facebook { get; set; }
        public string CT_LinkedIn { get; set; }
        public Nullable<short> CT_ExclureTrait { get; set; }
        public Nullable<short> CT_GDPR { get; set; }
        public Nullable<short> CT_Prospect { get; set; }
        //public Nullable<short> CT_OrderDay01 { get; set; }
        //public Nullable<short> CT_OrderDay02 { get; set; }
        //public Nullable<short> CT_OrderDay03 { get; set; }
        //public Nullable<short> CT_OrderDay04 { get; set; }
        //public Nullable<short> CT_OrderDay05 { get; set; }
        //public Nullable<short> CT_OrderDay06 { get; set; }
        //public Nullable<short> CT_OrderDay07 { get; set; }
        //public Nullable<short> CT_DeliveryDay01 { get; set; }
        //public Nullable<short> CT_DeliveryDay02 { get; set; }
        //public Nullable<short> CT_DeliveryDay03 { get; set; }
        //public Nullable<short> CT_DeliveryDay04 { get; set; }
        //public Nullable<short> CT_DeliveryDay05 { get; set; }
        //public Nullable<short> CT_DeliveryDay06 { get; set; }
        //public Nullable<short> CT_DeliveryDay07 { get; set; }
        //public Nullable<int> CAL_No { get; set; }
        //public Nullable<int> cbCAL_No { get; set; }
        public byte[] cbCG_NumPrinc { get; set; }

        public F_COMPTET()
        {

            //CT_Type = 0;
            //CG_NumPrinc = "34210000";
            //CT_Contact = "";
            //CT_Complement = "";
            //CT_CodePostal = "";

            //CT_CodeRegion = "";
            CT_Raccourci = "";

            BT_Num = 0;
            //N_Devise = 0;
            CT_Ape = "";
            CT_Siret = "";
            CT_Statistique01 = "";
            CT_Statistique02 = "";
            CT_Statistique03 = "";
            CT_Statistique04 = "";
            CT_Statistique05 = "";
            CT_Statistique06 = "";
            CT_Statistique07 = "";
            CT_Statistique08 = "";
            CT_Statistique09 = "";
            CT_Statistique10 = "";
            CT_Commentaire = "";
            CT_Encours = Convert.ToDecimal(0.000000);
            CT_Assurance = Convert.ToDecimal(0.000000);
            N_Risque = 1;
            //N_CatTarif = searchCatTarif.cbIndice,
            CT_Taux01 = Convert.ToDecimal(0.000000);
            CT_Taux02 = Convert.ToDecimal(0.000000);
            CT_Taux03 = Convert.ToDecimal(0.000000);
            CT_Taux04 = Convert.ToDecimal(0.000000);
            //N_Period = 1;
            CT_Facture = 1;
            //CT_BLFact = 0;
            CT_Langue = 0;
            //N_Expedition = 1;
            // N_Condition = 1;
            CT_Saut = 1;
            CT_Lettrage = 1;
            CT_ValidEch = 0;
            //CT_Sommeil = 0;

            DE_No = 0;

            CT_ControlEnc = 0;
            CT_NotRappel = 0;
            N_Analytique = 0;

            CA_Num = null;

            CT_Telecopie = "";
            CT_Site = "";
            CT_Coface = "";
            CT_Surveillance = 0;
            CT_SvDateCreate = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_SvFormeJuri = "";
            CT_SvEffectif = "";
            CT_SvCA = Convert.ToDecimal(0.000000);
            CT_SvResultat = Convert.ToDecimal(0.000000);
            CT_SvIncident = 0;
            CT_SvDateIncid = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_SvPrivil = 0;
            CT_SvRegul = "";
            CT_SvCotation = "";
            CT_SvDateMaj = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_SvObjetMaj = "";
            CT_SvDateBilan = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_SvNbMoisBilan = 0;
            N_AnalytiqueIFRS = 0;
            CT_PrioriteLivr = 0;
            CT_LivrPartielle = 0;
            MR_No = 0;

            CT_NotPenal = 0;
            EB_No = 0;

            cbProt = 0;
            cbCreateur = "Mbl";
            cbModification = DateTime.Now.Date;
            cbReplication = 0;
            cbFlag = 0;

            CT_DateFermeDebut = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_DateFermeFin = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_FactureElec = 0;
            CT_TypeNIF = 0;
            CT_RepresentInt = "";
            CT_RepresentNIF = "";
            CT_EdiCodeType = 0;
            CT_EdiCode = "";
            CT_EdiCodeSage = "";
            CT_ProfilSoc = 0;
            CT_StatutContrat = 0;
            CT_DateMAJ = Convert.ToDateTime("1753-01-01T00:00:00");
            CT_EchangeRappro = 0;
            CT_EchangeCR = 1;
            PI_NoEchange = 0;

            CT_BonAPayer = 0;
            CT_DelaiTransport = 0;
            CT_DelaiAppro = 0;
            CT_LangueISO2 = "";
            CT_AnnulationCR = 1;
            CT_Facebook = "";
            CT_LinkedIn = "";
            CT_ExclureTrait = 0;
            CT_GDPR = 0;
            CT_Prospect = 0;
            cbCreation = DateTime.Now.Date;
            cbCreationUser = null;
            //CT_OrderDay01 = 1;
            //CT_OrderDay02 = 1;
            //CT_OrderDay03 = 1;
            //CT_OrderDay04 = 1;
            //CT_OrderDay05 = 1;
            //CT_OrderDay06 = 0;
            //CT_OrderDay07 = 0;
            //CT_DeliveryDay01 = 1;
            //CT_DeliveryDay02 = 1;
            //CT_DeliveryDay03 = 1;
            //CT_DeliveryDay04 = 1;
            //CT_DeliveryDay05 = 1;
            //CT_DeliveryDay06 = 0;
            //CT_DeliveryDay07 = 0;
            //CAL_No = 2;
        }
    }
}
