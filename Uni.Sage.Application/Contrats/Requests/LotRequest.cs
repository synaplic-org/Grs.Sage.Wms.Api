using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Requests
{
    public  class LotRequest
    {
        public decimal QuantiteScane { get; set; }
        public string StockId { get; set; }
        public DateTime DatePeremption { get; set; }
    }
}
