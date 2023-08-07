using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grs.Sage.ObjetMetiers
{
    public class DocumentVente
    {
        public Int16 DocType { get; set; }
        public string Depot { get; set; }
        public Int32? IDDepot { get; set; }
        public string NumClient { get; set; }
        public string Client { get; set; }
        public DateTime? DocDate { get; set; }
        public string NumPiece { get; set; }
        public string RefPiece { get; set; }
        public string DocStatut { get; set; }
        public Int16? Imprime { get; set; }
        public Int16? Reliquat { get; set; }
        public string Adrs { get; set; }
        public List<LigneDocument> LgDocument = new List<LigneDocument> ();

    }
}
