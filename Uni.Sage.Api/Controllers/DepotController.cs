using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using System.Collections.Generic;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class DepotController : BaseController
    {
        private readonly IDepotService _DepotService;

        public DepotController(IDepotService depotService) : base()
        {

            _DepotService = depotService;

        }

		[AllowAnonymous]
		[HttpGet(nameof(GetDepots))]
        public async Task<Result<List<DepotResponse>>> GetDepots(string pConnexionName)
        {
            var result = await _DepotService.GetDepots(pConnexionName);
            return result;
        }
        [AllowAnonymous]
        [HttpPost(nameof(CreateDepot))]

        public async Task<IResult<DepotResponse>> CreateDepot(DepotRequest depot)
        {
            var result = await _DepotService.PostDepot(depot);
           
            return result;
        }
    }
}
