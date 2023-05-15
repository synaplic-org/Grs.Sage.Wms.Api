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
	public class DocEnteteController : BaseController
	{
		private readonly IDocEnteteService _DocEnteteService;

		public DocEnteteController(IDocEnteteService DocEnteteService) : base()
		{

			_DocEnteteService = DocEnteteService;

		}

		[AllowAnonymous]
		[HttpGet(nameof(GetDocEntetes))]
		public async Task<Result<List<DocEnteteResponse>>> GetDocEntetes(string pConnexionName)
		{
			var result = await _DocEnteteService.GetDocEntete(pConnexionName);
			return result;
		}



	}
}
