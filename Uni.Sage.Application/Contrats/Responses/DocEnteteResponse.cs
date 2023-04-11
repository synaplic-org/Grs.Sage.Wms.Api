using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class DocEnteteResponse : Response
    {
        public int ID { get; set; }
        public string CodeTiers { get; set; }
        public DateTime Date { get; set; }
        public string NumeroDocument { get; set; }
        public string Reference { get; set; }
        public string Affaire { get; set; }
        public int Representant { get; set; }
        public int CodeDepot { get; set; }
        public string Devise { get; set; }
        public int CategorieTarifaire { get; set; }
        

        public List<DocligneResponse> DocLignes { get; set; }
    }
}
