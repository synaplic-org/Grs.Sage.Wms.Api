
using Dapper;
using Refit;
using Serilog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Communication;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IArticleService
    {

        Task<Result<List<ArticleStockResponse>>> GetStockArticle(string pConnexionName, string Reference);
        Task<Result<List<EtatStockResponse>>> GetEtatStock(string pConnexionName);
        Task<Result<List<EtatStockResponse>>> GetEtatStockLot(string pConnexionName,int Depot);
        Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName);
        Task<Result<List<ArticleParDepotResponse>>> GetStockParDepot(string pConnexionName);

    }

    public class ArticleService : IArticleService
    {
       
        private readonly IQueryService _QueryService;
        public ArticleService(IQueryService queryService)
        {
            
            _QueryService = queryService;
        }

        public async Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName)
        {

            try
            {
                Log.Information("#######   hehhhooooscks up!  #######");
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_ARTICLE_MIN");

                var results = await db.QueryAsync<ArticleResponse>(oQuery);

                return await Result<List<ArticleResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ArticleResponse>>.FailAsync(ex);
            }

        }
        public async Task<Result<List<EtatStockResponse>>> GetEtatStock(string pConnexionName)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_ETAT_STOCK");

                var results = await db.QueryAsync<EtatStockResponse>(oQuery);

                return await Result<List<EtatStockResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Etat stock societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<EtatStockResponse>>.FailAsync(ex);
            }

        }
        public async Task<Result<List<EtatStockResponse>>> GetEtatStockLot(string pConnexionName,int Depot)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_ETAT_STOCK_LOT");

                var results = await db.QueryAsync<EtatStockResponse>(oQuery, new { Depot });

                return await Result<List<EtatStockResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Etat stock societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<EtatStockResponse>>.FailAsync(ex);
            }

        }
        public async Task<Result<List<ArticleParDepotResponse>>> GetStockParDepot(string pConnexionName)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_ARTICLE_PAR_DEPOT");

                var results = await db.QueryAsync<ArticleParDepotResponse>(oQuery);

                return await Result<List<ArticleParDepotResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ArticleParDepotResponse>>.FailAsync(ex);
            }

        }
        public async Task<Result<List<ArticleStockResponse>>> GetStockArticle(string pConnexionName,string Reference)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_STOCK_ARTICLE_MIN");

                var results = await db.QueryAsync<ArticleStockResponse>(oQuery, new { Reference });

                return await Result<List<ArticleStockResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ArticleStockResponse>>.FailAsync(ex);
            }

        }



    }


}
