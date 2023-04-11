using Microsoft.AspNetCore.Mvc;
using Uni.Sage.Infrastructures.Services;
using System.Threading.Tasks;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class FactureController : BaseController
    {
        private readonly IFactureService _FactureService;

        public FactureController(IFactureService FactureService) : base()
        {

            _FactureService = FactureService;

        }


        [HttpGet(nameof(GetEnteteFactureList))]
        public async Task<ActionResult> GetEnteteFactureList(string pConnexionName)
        {
            var result = await _FactureService.Facture_Entete_Vente(pConnexionName);
            return Ok(result);
        }

        [HttpGet(nameof(GetLigneFactureList))]
        public async Task<ActionResult> GetLigneFactureList(string pConnexionName, string do_piece)
        {
            var result = await _FactureService.Facture_Ligne_Vente(pConnexionName, do_piece);
            return Ok(result);
        }
    }
}
