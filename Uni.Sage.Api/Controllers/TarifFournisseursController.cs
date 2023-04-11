using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class TarifFournisseursController : BaseController
    {
        private readonly ITarifFournissService _TarifFournissService;

        public TarifFournisseursController(ITarifFournissService tarifFournissService) : base()
        {

            _TarifFournissService = tarifFournissService;

        }


        [HttpGet(nameof(GetTarifsFournisseurs))]
        public async Task<ActionResult> GetTarifsFournisseurs(string pConnexionName)
        {
            var result = await _TarifFournissService.GetTarifsFournisseurs(pConnexionName);
            return Ok(result);
        }
    }
}
