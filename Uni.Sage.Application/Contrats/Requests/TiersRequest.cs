using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Application.Contrats.Requests
{
    public class TiersRequest : Request
    {
        public string ConnectionName { get; set; }
        public string CodeTier { get; set; }
        public string Intitule { get; set; }
        public string CompteCollectif { get; set; }
        public string Qualite { get; set; }
        public string Classement { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public string Telephone { get; set; }
        public string Interlocuteur { get; set; }
        public string Identifiant { get; set; }
        public string RC { get; set; }
        public string IF { get; set; }
        public string ICE { get; set; }
        public string Patente { get; set; }
        public string CentraleAchat { get; set; }
        public int CategorieTarrifiaire { get; set; }
        public string Representant { get; set; }
        public int DepotRattachement { get; set; }
        public string TiersPayeur { get; set; }
        public string CodeAffaire { get; set; }
        public string Devise { get; set; }
        public int Periodicite { get; set; }
        public string PrioriteLivraison { get; set; }
        public string DelaiTransport { get; set; }
        public int Comptabilite { get; set; }
        public int MettreEnSommeil { get; set; }
        public int FactureBL { get; set; }
        public int TypeContact { get; set; }
        public string NomContact { get; set; }
        public string PrenomContact { get; set; }
        public int Civilite { get; set; }
        public int Service { get; set; }
        public string Fonction { get; set; }
        public string Portable { get; set; }
        public string Adresse { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Region { get; set; }
        public string Pays { get; set; }
        public string Complement { get; set; }
        public string TelephoneContact { get; set; }
        public int ModeExpedition { get; set; }
        public int ConditionLivraison { get; set; }
        public string Acheteur { get; set; }


    }
}
