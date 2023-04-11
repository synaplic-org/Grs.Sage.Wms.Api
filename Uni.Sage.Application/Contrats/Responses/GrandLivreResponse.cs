using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class GrandLivreResponse
    {
        public string CodeClient { get; set; }    
        public string intituleClient { get; set; }
        public string Numfacture { get; set; }
        public DateTime Datefacture { get; set; }     
        public decimal TotalFacture { get; set; }
        public DateTime Dateech { get; set; }
        public decimal Montantregle { get; set; }
        public decimal ResteApaye{ get; set; }
        public int nbrjr { get; set; }


    }
}
