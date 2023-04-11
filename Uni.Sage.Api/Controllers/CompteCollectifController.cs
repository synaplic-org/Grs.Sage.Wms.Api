using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class CompteCollectifController : BaseController
    {

        private readonly ICompteCollectifService _compteCollectifService;
        public CompteCollectifController(ICompteCollectifService compteCollectifService) : base()
        {
            _compteCollectifService = compteCollectifService;
        }


        [HttpGet(nameof(GetCompte))]
        public async Task<ActionResult> GetCompte(string pConnexionName)
        {
            var result = await _compteCollectifService.GetCompteCollectif(pConnexionName);
            return Ok(result);
        }
    }
}
