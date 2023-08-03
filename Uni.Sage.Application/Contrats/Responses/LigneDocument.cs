using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class LigneDocument
    {
        public Int32? IDDepot { get; set; }
        public Int16 DocType { get; set; }
        public string NumPiece { get; set; }
        public DateTime? DateDoc { get; set; }
        public string Refrence { get; set; }
        public string Designation { get; set; }
        public decimal? Qte { get; set; }
        public DateTime? DateBC { get; set; }
        public string NumBC { get; set; }
        public string Unite { get; set; }
        public decimal? QteBC { get; set; }
        public Int32? NumLigne { get; set; }
        public decimal? QteALivre { get; set; }
    }
}
