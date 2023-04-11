using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class GrandLivreController : BaseController
    {
        private readonly IGrandLivreService _GrandLivreService;

        public GrandLivreController(IGrandLivreService grandLivreService) : base()
        {

            _GrandLivreService = grandLivreService;

        }


        [HttpGet(nameof(GetGrandLivres))]
        public async Task<ActionResult> GetGrandLivres(string pConnexionName)
        {
            var result = await _GrandLivreService.GetGrandLivres(pConnexionName);
            return Ok(result);
        }

        [HttpGet(nameof(GetEcheance))]
        public async Task<IActionResult> GetEcheance(string pConnexionName, int pageNumber, int pageSize)
        {
            var result = await _GrandLivreService.GetEcheance(pConnexionName, pageNumber, pageSize);

            return Ok(result.Data);
        }

    }
}
