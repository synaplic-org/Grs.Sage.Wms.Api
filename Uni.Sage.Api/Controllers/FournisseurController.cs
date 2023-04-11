using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class FournisseurController : BaseController
    {
        private readonly IFournisseurService _FournisseurService;

        public FournisseurController(IFournisseurService fournisseurService) : base()
        {
            _FournisseurService = fournisseurService;
        }


        [HttpGet(nameof(GetFournisseurs))]
        public async Task<ActionResult> GetFournisseurs(string pConnexionName)
        {
            var result = await _FournisseurService.GetFournisseurs(pConnexionName);
            return Ok(result);
        }

    }
}
