using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class CategorieTarifaireController : BaseController
    {
        private readonly ICatTarifService _CatTarifService;

        public CategorieTarifaireController(ICatTarifService CatTarifService) : base()
        {

            _CatTarifService = CatTarifService;

        }


        [HttpGet(nameof(GetCategorieTarifaires))]
        public async Task<ActionResult> GetCategorieTarifaires(string pConnexionName)
        {
            var result = await _CatTarifService.GetCategorieTarifaires(pConnexionName);
            return Ok(result);
        }
    }
}
