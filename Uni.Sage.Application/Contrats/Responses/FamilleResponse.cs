using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public class FamilleResponse
    {
        public string CodeFamille { get; set; }
        public string Intitule { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string FamilleCentrale { get; set; }


    }
}
