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
        public string ErpId { get; set; }
        public string Name { get; set; }
        public decimal QuantiteReel { get; set; }
        //public decimal Montant { get; set; }
        public int WareHouseID { get; set; }
        public string WareHouseName { get; set; }

    }
    public class ArticleStockResponse
    {
        public int DE_NO { get; set; }
        public string Depot { get; set; }
        public string Reference { get; set; }
        public string Article { get; set; }
        public string famcent { get; set; }
        public string FA_CodeFamille { get; set; }
        public string famille { get; set; }
        public string N_lot { get; set; }
        public DateTime LS_Peremption { get; set; }
        public DateTime LS_Fabrication { get; set; }
        public decimal qte { get; set; }
    }
}
