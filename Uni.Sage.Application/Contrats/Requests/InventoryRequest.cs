using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Requests
{
    public class InventoryRequest
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal QuantiteInventaire { get; set; }
        public decimal QuantiteStock { get; set; }
        public string StockId { get; set; }
        public DateTime DatePeremption { get; set; }
        public decimal ValeurUnitaire { get; set; }
    }
}
