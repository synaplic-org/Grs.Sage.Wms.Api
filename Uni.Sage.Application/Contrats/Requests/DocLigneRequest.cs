using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Application.Contrats.Requests
{
    public class DocLigneRequest : Request
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public decimal Quantite { get; set; }
        public decimal Remise { get; set; }
        public decimal Prix { get; set; }
        public int CodeGamme1 { get; set; }
        public int CodeGamme2 { get; set; }
        public bool IsTTC { get; set; }
        public decimal MontantHT { get; set; }
        public decimal MontantTTC { get; set; }
        public decimal Tva { get; set; }
    }
}
