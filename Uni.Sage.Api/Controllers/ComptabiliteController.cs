using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{



    public class ComptabiliteController : BaseController
    {
        private readonly IComptabiliteService _comptabiliteService;

        public ComptabiliteController(IComptabiliteService comptabiliteService) : base()
        {
            _comptabiliteService = comptabiliteService;
        }


        [HttpGet(nameof(GetCompta))]
        public async Task<ActionResult> GetCompta(string pConnexionName)
        {
            var result = await _comptabiliteService.GetCompta(pConnexionName);
            return Ok(result);
        }
    }
   
}
