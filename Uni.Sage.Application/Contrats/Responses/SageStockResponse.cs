using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public  class SageStockResponse
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public string Lot { get; set; }
        public DateTime DatePeremption { get; set; }

        public decimal QteStockDate { get; set; }
        public decimal QteMouvement { get; set; }
        public string CodeFamille { get; set; }
    }
}
