using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class ExpeditionController : BaseController
    {
        private readonly IExpeditionService _ExpeditionService;

        public ExpeditionController(IExpeditionService ExpeditionService) : base()
        {
            _ExpeditionService = ExpeditionService;
        }

        [HttpGet(nameof(GetExpedition))]
        public async Task<ActionResult> GetExpedition(string pConnexionName)
        {
            var result = await _ExpeditionService.GetExpedition(pConnexionName);
            return Ok(result);
        }
    }
}
