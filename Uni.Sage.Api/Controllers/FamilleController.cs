using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using System.Collections.Generic;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class FamilleController : BaseController
    {
        private readonly IFamilleService _FamilleService;

        public FamilleController(IFamilleService familleService) : base()
        {
            _FamilleService = familleService;
        }

		[AllowAnonymous]
		[HttpGet(nameof(GetFamilles))]
        public async Task<Result<List<FamilleResponse>>> GetFamilles(string pConnexionName)
        {
            var result = await _FamilleService.GetFamilles(pConnexionName);
            return result;
        }
    }
}
