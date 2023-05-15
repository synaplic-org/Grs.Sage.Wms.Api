using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using System.Collections.Generic;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class EmplacementController : BaseController
    {
        private readonly IEmplacementService _EmplacementService;

        public EmplacementController(IEmplacementService emplacementService) : base()
        {

            _EmplacementService = emplacementService;

        }


		[AllowAnonymous]
		[HttpGet(nameof(GetEmplacements))]
        public async Task<Result<List<EmplacementResponse>>> GetEmplacements(string pConnexionName)
        {
            var result = await _EmplacementService.GetEmplacements(pConnexionName);
            return result;
        }
    }
}
