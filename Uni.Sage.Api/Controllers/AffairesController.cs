using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class AffairesController : BaseController
    {
        private readonly IAffairesService _AffairesService;

        public AffairesController(IAffairesService affairesService) : base()
        {
            _AffairesService = affairesService;
        }


        [HttpGet(nameof(GetAffaires))]
        public async Task<ActionResult> GetAffaires(string pConnexionName)
        {
            var result =  await _AffairesService.GetAffaires(pConnexionName);
            return Ok(result);
        }
        [HttpGet(nameof(GetTiersAffaires))]
        public async Task<ActionResult> GetTiersAffaires(string pConnexionName)
        {
            var result =  await _AffairesService.GetTiersAffaires(pConnexionName);
            return Ok(result);
        }

        [HttpGet(nameof(GetAffairesAchat))]
        public async Task<ActionResult> GetAffairesAchat(string pConnexionName)
        {
            var result = await _AffairesService.GetAffairesAchat(pConnexionName);
            return Ok(result);
        }

        [HttpGet(nameof(GetAffairesVente))]
        public async Task<ActionResult> GetAffairesVente(string pConnexionName)
        {
            var result = await _AffairesService.GetAffairesVente(pConnexionName);
            return Ok(result);
        }

    }
}
