using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _ArticleService;

        public ArticleController(IArticleService articleService) : base()
        {

            _ArticleService = articleService;

        }

        //[HttpGet(nameof(GetArticles1))]
        //public async Task<Result<List<ArticleResponse>>> GetArticles1(string pConnexionName)
        //{
        //    var result = await _ArticleService.GetArticles1(pConnexionName);
        //    return result;
        //}

        [HttpGet(nameof(GetArticles))]
        public async Task<Result<List<ArticleResponse>>> GetArticles(string pConnexionName)
        {
            var result = await _ArticleService.GetArticles(pConnexionName);
            return result;
        }

        [HttpGet(nameof(GetClientArticles))]
        public async Task<ActionResult> GetClientArticles(string pConnexionName, string CodeClient, string famille)
        {
            var result = await _ArticleService.GetClientArticles(pConnexionName, CodeClient, famille);
            return Ok(result);
        } 
        [HttpGet(nameof(GetArticlesParFamille))]
        public async Task<ActionResult> GetArticlesParFamille(string pConnexionName,string famille)
        {
            var result = await _ArticleService.GetArticlesParFamille(pConnexionName,famille);
            return Ok(result);
        }

        [HttpGet(nameof(GetFamillesSousFamilles))]
        public async Task<ActionResult> GetFamillesSousFamilles(string pConnexionName)
        {
            var result = await _ArticleService.GetFamillesSousFamilles(pConnexionName);
            return Ok(result);
        }

        [HttpGet(nameof(GetArticlesParDepot))]
        public async Task<ActionResult> GetArticlesParDepot(string pConnexionName)
        {
            var result = await _ArticleService.GetArticlesParDepot(pConnexionName);
            return Ok(result);
        } 
        
        [HttpPut(nameof(UpdateArticlePic))]
        public async Task<ActionResult> UpdateArticlePic(string pConnexionName,string articleReference,string picture)
        {
            var result = await _ArticleService.UpdateArticlePic(pConnexionName,articleReference,picture);
            return Ok(result);
        }



    }
}
