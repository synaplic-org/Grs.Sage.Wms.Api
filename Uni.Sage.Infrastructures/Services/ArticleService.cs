
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

      
        Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName);
     
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

     

    }


}
