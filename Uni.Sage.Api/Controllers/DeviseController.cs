using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class DeviseController : BaseController
    {
        private readonly IDeviseService _deviseService;

        public DeviseController(IDeviseService deviseService) : base()
        {
            _deviseService = deviseService;
        }


        [HttpGet(nameof(GetDevise))]
        public async Task<ActionResult> GetDevise(string pConnexionName)
        {
            var result = await _deviseService.GetDevise(pConnexionName);
            return Ok(result);
        }
    }
}
