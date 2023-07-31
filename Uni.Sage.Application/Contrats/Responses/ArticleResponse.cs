using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public class ArticleResponse
    {
        public string ErpID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FamilyID { get; set; }
        public bool IsEnabled { get; set; }
        public int StockType { get; set; }
     


    }
    public class ArticleParDepotResponse
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public decimal QuantiteReel { get; set; }
        public decimal Montant { get; set; }
        public int CodeDepot { get; set; }
        public string Depot { get; set; }

    }
}
