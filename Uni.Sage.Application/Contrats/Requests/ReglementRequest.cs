using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Application.Contrats.Requests
{
    public class ReglementRequest : Request
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CodeClient { get; set; }
        public string Libelle { get; set; }
        public string Client { get; set; }
        public string Reference { get; set; }
        public string Mode { get; set; }
        public decimal Montant { get; set; }
        public string CreatedBy { get; set; }
        public string Journal { get; set; }
        public string CompteCollectif { get; set; }


    }
}
