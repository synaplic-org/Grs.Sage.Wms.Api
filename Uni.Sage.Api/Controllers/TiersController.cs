using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class TiersController : BaseController
    {

        private readonly ITierService _tierService;

        public TiersController(ITierService tierService) : base()
        {
            _tierService = tierService;
        }

        [HttpPost(nameof(CreateTiers))]

        public async Task<IResult<TiersResponse>> CreateTiers(TiersRequest request)
        {
            var result = await _tierService.CreateTier(request);
            return result;
        }
    }
}
