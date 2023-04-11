using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class LivraisonController : BaseController
    {
        private readonly ILivraisonService _LivraisonService;

        public LivraisonController(ILivraisonService livraisonService) : base()
        {

            _LivraisonService = livraisonService;

        }


        [HttpGet(nameof(GetEnteteLivraisonList))]
        public async Task<ActionResult> GetEnteteLivraisonList(string pConnexionName)
        {
            var result = await _LivraisonService.Livraison_Entete_Vente(pConnexionName);
            return Ok(result);
        }

        [HttpGet(nameof(GetLigneLivraisonList))]
        public async Task<ActionResult> GetLigneLivraisonList(string pConnexionName,string do_piece)
        {
            var result = await _LivraisonService.Livraison_Ligne_Vente(pConnexionName, do_piece);
            return Ok(result);
        }
    }
}
