
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

        //Task<Result<List<ArticleResponse>>> GetArticles1(string pConnexionName);
        Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName);
        Task<IResult<List<ClientArticleResponse>>> GetClientArticles(string pConnexionName,string CodeClient, string famille);
        Task<IResult<List<ArticleResponse>>> GetArticlesParFamille(string pConnexionName,string famille);
        Task<IResult<List<FamilleSousFamilleResponse>>> GetFamillesSousFamilles(string pConnexionName);
        Task<IResult<List<ArticleParDepotResponse>>> GetArticlesParDepot(string pConnexionName);
        Task<IResult<ArticleResponse>> UpdateArticlePic(string pConnexionName,string articleReference,string picture );
    }

    public class ArticleService : IArticleService
    {
       
        private readonly IQueryService _QueryService;

        public ArticleService(IQueryService queryService)
        {
            
            _QueryService = queryService;
        }

        //public async Task<Result<List<ArticleResponse>>> GetArticles1(string pConnexionName)
        //{

        //    try
        //    {
        //        using var db = _QueryService.NewDbConnection(pConnexionName);
        //        var oQuery = _QueryService.GetQuery("SELECT_ARTICLE_MIN");

        //        var results = await db.QueryAsync<ArticleResponse>(oQuery);

        //        return await Result<List<ArticleResponse>>.SuccessAsync(results.ToList());
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
        //        return await Result<List<ArticleResponse>>.FailAsync(ex);
        //    }

        //}


        public async Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName)
        {

            try
            {
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

        public async Task<IResult<List<ArticleParDepotResponse>>> GetArticlesParDepot(string pConnexionName)
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

        public async Task<IResult<List<ClientArticleResponse>>> GetClientArticles(string pConnexionName,string CodeClient, string famille)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var parameters = new { CodeClient = CodeClient, famille = famille };

                var oQuery = _QueryService.GetQuery("SELECT_CLIENT_ARTICLE");

                var results = await db.QueryAsync<ClientArticleResponse>(oQuery, parameters);
                //var oQueryy = db.QueryAsync<ClientArticleResponse>("SELECT_CLIENT_ARTICLE", parameters);
                //var results = await db.QueryAsync<ClientArticleResponse>(oQueryy);
                //var results =  oQueryy.Result.ToList();


                return await Result<List<ClientArticleResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ClientArticleResponse>>.FailAsync(ex);
            }

        } 
        public async Task<IResult<List<ArticleResponse>>> GetArticlesParFamille(string pConnexionName, string famille)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var parameters = new {famille = famille };

                var oQuery = _QueryService.GetQuery("SELECT_ARTICLE_PAR_FAMILLE");

                var results = await db.QueryAsync<ArticleResponse>(oQuery, parameters);
                //var oQueryy = db.QueryAsync<ClientArticleResponse>("SELECT_CLIENT_ARTICLE", parameters);
                //var results = await db.QueryAsync<ClientArticleResponse>(oQueryy);
                //var results =  oQueryy.Result.ToList();


                return await Result<List<ArticleResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ArticleResponse>>.FailAsync(ex);
            }

        }

        public async Task<IResult<List<FamilleSousFamilleResponse>>> GetFamillesSousFamilles(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_FAMILLE _SOUSFAMILLE");

                var results = await db.QueryAsync<FamilleSousFamilleResponse>(oQuery);

                return await Result<List<FamilleSousFamilleResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Articles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<FamilleSousFamilleResponse>>.FailAsync(ex);
            }
        }

        public async Task<IResult<ArticleResponse>> UpdateArticlePic(string pConnexionName, string articleReference, string picture)
        {

            var Result = new Result<ArticleResponse>();

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);

                using Uni.Sage.Infrastructures.Repositories.SageRepository Repo = _QueryService.NewRepository(pConnexionName);

                using var insertRepo = _QueryService.NewRepository(pConnexionName);



                using (var dbContextTransaction = insertRepo.BeginTransaction())
                {
                    try
                    {
                        var parameters = new { Picture = picture,Reference = articleReference };

                        await insertRepo.QueryAsync<int>("UPDATE_ARTICLE_PICTURE", parameters);


                        dbContextTransaction.Commit();

                        return await Result<ArticleResponse>.SuccessAsync(Result.Data);

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " UPDATE ARTICLE  {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<ArticleResponse>.FailAsync(ex);
            }

        }

    }


}
