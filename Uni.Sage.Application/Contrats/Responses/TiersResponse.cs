using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class TiersResponse
    {
        public string CodeTier { get; set; }
        public string Intitule { get; set; }
        public int Type { get; set; }
        public string CompteCollectif { get; set; }
        public string Qualite { get; set; }
        public string Classement { get; set; }
        public string Contact { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }
        public string Complement { get; set; }
        public string Ville { get; set; }
        public string Pays { get; set; }
        public string Identifiant { get; set; }
        public int CodeCollaborateur { get; set; }
        public string NomCollaborateur { get; set; }
        public string PrenomCollaborateur { get; set; }
    }
}
