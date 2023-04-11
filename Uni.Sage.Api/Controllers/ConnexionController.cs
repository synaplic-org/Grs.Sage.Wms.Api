using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class ConnexionController : BaseController
    {
        public ConnexionController(IConnexionService connexionService) : base()
        {
            _ConnexionService = connexionService;
        }

        private readonly IConnexionService _ConnexionService;
        //private readonly IQueryService _QueryService;

        [HttpGet(nameof(GetConnections))]
        public async Task<IActionResult> GetConnections()
        {
            var rs = await _ConnexionService.GetPublicConnections();
            return Ok(rs);
        }

        [HttpGet(nameof(GetRessources))]
        public  IActionResult GetRessources()
        {
            var rs =  _ConnexionService.ReadResourceListe();
            return Ok(rs);
        }
    }
}