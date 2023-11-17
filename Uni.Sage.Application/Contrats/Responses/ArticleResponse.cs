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
        public string CodeBarre { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string FamilyID { get; set; }
        public bool IsEnabled { get; set; }
        public int StockType { get; set; }
     


    }
    public class ArticleLotResponse
    {
        public string ProductId { get; set; }
        public string StockId { get; set; }
        public DateTime DatePeremption { get; set; }
    }
    public class EtatArticleValoriseResponse
    {
        public string ProductId { get; set; }
        public int CodeDepot { get; set; }
        public decimal PMP { get; set; }
        public decimal PrixAchat { get; set; }
        public string Intitule { get; set; }
        public decimal DernierPachat { get; set; }
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
    public class EtatStockResponse
    {
        public string Reference { get; set; }
        public string Designation { get; set; }
        public decimal stockReel { get; set; }
        public decimal StockPreparer { get; set; }
        public decimal stockDispo { get; set; }
        public decimal Qtecommande { get; set; }
        public decimal QteReserve { get; set; }
        public decimal StockATerme { get; set; }
        public string Depot { get; set; }
        public int DE_No { get; set; }
        public string Lot { get; set; }

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
