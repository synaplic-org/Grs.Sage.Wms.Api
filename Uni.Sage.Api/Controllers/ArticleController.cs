using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using System;
using Uni.Sage.Application.Contrats.Responses;

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
        [HttpGet(nameof(GetEtatStock))]
        public async Task<Result<List<EtatStockResponse>>> GetEtatStock(string pConnexionName)
        {
            var result = await _ArticleService.GetEtatStock(pConnexionName);
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
        public async Task<Result<List<EtatStockResponse>>> GetStockParArticle(string pConnexionName,string Reference)
        {
            var result = await _ArticleService.GetStockArticle(pConnexionName,Reference);
            return result;
        }
        [AllowAnonymous]
        [HttpGet(nameof(GetEtatStockParLot))]
        public async Task<Result<List<EtatStockResponse>>> GetEtatStockParLot(string pConnexionName, int Depot)
        {
            var result = await _ArticleService.GetEtatStockLot(pConnexionName,Depot);
            return result;
        }
        [AllowAnonymous]
        [HttpGet(nameof(GetStockADate))]
        public async Task<Result<List<SageStockResponse>>> GetStockADate(string pConnexionName,int depot, DateTime date, string filter)
        {
            return await _ArticleService.GetStockADateAsync(pConnexionName,date,depot,filter);
            
        }
        [AllowAnonymous]
        [HttpGet(nameof(GetArticleById_lot))]
        public async Task<Result<List<ArticleLotResponse>>> GetArticleById_lot(string pConnexionName,string Reference)
        {
            return await _ArticleService.GetArticleById_lot(pConnexionName, Reference);

        }
		[AllowAnonymous]
		[HttpGet(nameof(EtatValoriseArticle))]
		public async Task<Result<List<EtatArticleValoriseResponse>>> EtatValoriseArticle(string pConnexionName, int CodeDepot)
		{
			return await _ArticleService.EtatValoriseArticle(pConnexionName, CodeDepot);

		}


	}
}
