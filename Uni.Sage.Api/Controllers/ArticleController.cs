using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _ArticleService;

        public ArticleController(IArticleService articleService) : base()
        {

            _ArticleService = articleService;

        }

        [AllowAnonymous]
        [HttpGet(nameof(GetArticles))]
        public async Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName)
        {
            var result = await _ArticleService.GetArticles(pConnexionName);
            return result;
        }
        [AllowAnonymous]
        [HttpGet(nameof(GetStockParDepot))]
        public async Task<Result<List<ArticleParDepotResponse>>> GetStockParDepot(string pConnexionName)
        {
            var result = await _ArticleService.GetStockParDepot(pConnexionName);
            return result;
        }
        [AllowAnonymous]
        [HttpGet(nameof(GetStockParArticle))]
        public async Task<Result<List<ArticleStockResponse>>> GetStockParArticle(string pConnexionName,string Reference)
        {
            var result = await _ArticleService.GetStockArticle(pConnexionName,Reference);
            return result;
        }




    }
}
