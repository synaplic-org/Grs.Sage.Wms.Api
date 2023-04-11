using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{

    public class ConditionLivController : BaseController
    {
        private readonly IConditionLivService _conditionLiv;

        public ConditionLivController(IConditionLivService conditionLiv) : base()
        {
            _conditionLiv = conditionLiv;
        }


        [HttpGet(nameof(GetCondLiv))]
        public async Task<ActionResult> GetCondLiv(string pConnexionName)
        {
            var result = await _conditionLiv.GetCondLiv(pConnexionName);
            return Ok(result);
        }
       }
      
}
