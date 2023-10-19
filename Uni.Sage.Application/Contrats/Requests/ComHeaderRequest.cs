using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Requests
{
    public class ComHeaderRequest
    {
        public string ComHeaderId { get; set; }
        public string ThirdParty { get; set; }
        public bool ReliquatBC { get; set; }
        public bool LaisseBC{ get; set; }
        public string DepotSource { get; set; }
        public string DepotCible { get; set; }
        public string Type { get; set; }
        public List<ComlineRequest> ComLine { get; set; }
    }
}
