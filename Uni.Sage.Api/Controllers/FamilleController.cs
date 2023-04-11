using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class FamilleController : BaseController
    {
        private readonly IFamilleService _FamilleService;

        public FamilleController(IFamilleService familleService) : base()
        {
            _FamilleService = familleService;
        }


        [HttpGet(nameof(GetFamilles))]
        public async Task<ActionResult> GetFamilles(string pConnexionName)
        {
            var result = await _FamilleService.GetFamilles(pConnexionName);
            return Ok(result);
        }
    }
}
