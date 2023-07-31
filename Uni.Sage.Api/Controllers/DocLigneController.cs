using Grs.Sage.Wms.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
	public class DocLigneController : BaseController
	{
		private readonly IDocLigneService _DocLigneService;

		public DocLigneController(IDocLigneService DocLigneService) : base()
		{

			_DocLigneService = DocLigneService;

		}

		[AllowAnonymous]
		[HttpGet(nameof(GetDocLignes))]
		public async Task<Result<List<DocligneResponse>>> GetDocLignes(string pConnexionName)
		{
			var result = await _DocLigneService.GetDocLigne(pConnexionName);
			return result;
		}
        [AllowAnonymous]
        [HttpGet(nameof(GetDocLignesByType))]
        public async Task<Result<List<DocligneResponse>>> GetDocLignesByType(string pConnexionName,string Type,string ComHeaderId)
        {
            var result = await _DocLigneService.GetDocLigneByType(pConnexionName, Type, ComHeaderId);
            return result;
        }


    }
}
