using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class EmplacementController : BaseController
    {
        private readonly IEmplacementService _EmplacementService;

        public EmplacementController(IEmplacementService emplacementService) : base()
        {

            _EmplacementService = emplacementService;

        }


        [HttpGet(nameof(GetEmplacements))]
        public async Task<ActionResult> GetEmplacements(string pConnexionName)
        {
            var result = await _EmplacementService.GetEmplacements(pConnexionName);
            return Ok(result);
        }
    }
}
