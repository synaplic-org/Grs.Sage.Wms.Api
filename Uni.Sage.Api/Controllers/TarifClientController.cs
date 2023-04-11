using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class TarifClientController : BaseController
    {
        private readonly ITarifClientService _TarifClientService;

        public TarifClientController(ITarifClientService tarifClientService) : base()
        {

            _TarifClientService = tarifClientService;

        }


        [HttpGet(nameof(GetTarifClient))]
        public async Task<ActionResult> GetTarifClient(string pConnexionName)
        {
            var result = await _TarifClientService.GetTarifClient(pConnexionName);
            return Ok(result);
        }

    

    }
}
