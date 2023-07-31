using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class DocligneResponse : Response
    {


		public string ComHeaderId { get; set; }
		public string ProductId { get; set; }
		public string Designation { get; set; }
		public decimal QuantityRequest { get; set; }
		public int StockID { get; set; }
		public string LotSerie { get; set; }
		public DateTime DatePeremption { get; set; }
		public int Ligne { get; set; }
		
        //public string Unite { get; set; }
        //public string Depot { get; set; }
        //public string Collaborateur { get; set; }
        //public string CodeAffaire { get; set; }
        //public string Affaire { get; set; }
        public decimal MontantHT { get; set; }
		public decimal MontantTTC { get; set; }



		//public string Reference { get; set; }
  //      public string Designation { get; set; }
  //      public int Quantite { get; set; }
  //      public int Remise { get; set; }
  //      public int Prix { get; set; }
    }
}
