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

     


    }
}
