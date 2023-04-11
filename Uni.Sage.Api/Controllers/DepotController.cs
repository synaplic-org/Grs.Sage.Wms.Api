using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class DepotController : BaseController
    {
        private readonly IDepotService _DepotService;

        public DepotController(IDepotService depotService) : base()
        {

            _DepotService = depotService;

        }


        [HttpGet(nameof(GetDepots))]
        public async Task<ActionResult> GetDepots(string pConnexionName)
        {
            var result = await _DepotService.GetDepots(pConnexionName);
            return Ok(result);
        }
    }
}
