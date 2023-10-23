using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Requests
{
    public  class ComlineRequest
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        
        public decimal QuantiteScane { get; set; }
        public string StockId { get; set; }
        public DateTime DatePeremption { get; set; }
        public decimal PrixUnitaire { get; set; } 
    }
}
