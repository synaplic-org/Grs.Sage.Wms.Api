using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class BalanceAgeeController : BaseController
    {

            private readonly IBalanceAgeeService _BalanceService;

            public BalanceAgeeController(IBalanceAgeeService balanceService) : base()
            {

            _BalanceService = balanceService;

            }


            [HttpGet(nameof(GetBalance))]
            public async Task<ActionResult> GetBalance(string pConnexionName)
            {
                var result = await _BalanceService.GetBalance(pConnexionName);
                return Ok(result);
            }
        }
}
