using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class SoucheController : BaseController
    {
        private readonly ISoucheService _SoucheService;

        public SoucheController(ISoucheService soucheService) : base()
        {

            _SoucheService = soucheService;

        }


        [HttpGet(nameof(GetSouchesVente))]
        public async Task<ActionResult> GetSouchesVente(string pConnexionName)
        {
            var result = await _SoucheService.GetSouchesVente(pConnexionName);
            return Ok(result);
        }
        [HttpGet(nameof(GetSouchesAchat))]
        public async Task<ActionResult> GetSouchesAchat(string pConnexionName)
        {
            var result = await _SoucheService.GetSouchesAchat(pConnexionName);
            return Ok(result);
        }
        [HttpGet(nameof(GetSouchesInterne))]
        public async Task<ActionResult> GetSouchesInterne(string pConnexionName)
        {
            var result = await _SoucheService.GetSouchesInterne(pConnexionName);
            return Ok(result);
        }
    }
}
