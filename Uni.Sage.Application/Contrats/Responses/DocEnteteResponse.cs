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
		public int Domaine { get; set; }
		public int Type { get; set; }
		public string Piece { get; set; }
		public int Statut { get; set; }
		public DateTime Date { get; set; }
		public string Reference { get; set; }
		public string CodeClient { get; set; }
		public int CodeCollaborateur { get; set; }
		public string Collaborateur { get; set; }
		public int CodeDepot { get; set; }
		public string Depot { get; set; }
		public string CodeAffaire { get; set; }
		public string Affaire { get; set; }
		public int CodeSouche { get; set; }
		public string Souche { get; set; }
		public string CodeCompteCollectif { get; set; }
		public string CompteCollectif { get; set; }
		public decimal TotalHT { get; set; }
		public decimal TotalTTC { get; set; }
		//public int ID { get; set; }
  //      public string CodeTiers { get; set; }
  //      public DateTime Date { get; set; }
  //      public string NumeroDocument { get; set; }
  //      public string Reference { get; set; }
  //      public string Affaire { get; set; }
  //      public int Representant { get; set; }
  //      public int CodeDepot { get; set; }
  //      public string Devise { get; set; }
  //      public int CategorieTarifaire { get; set; }
        

        //public List<DocligneResponse> DocLignes { get; set; }
    }
}
