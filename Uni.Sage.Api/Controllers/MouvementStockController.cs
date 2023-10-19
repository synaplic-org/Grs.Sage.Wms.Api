using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class MouvementStockController : BaseController
    {
        private readonly IMouvementStockService _MouvementStockService;

        public MouvementStockController(IMouvementStockService MouvementStockService) : base()
        {

            _MouvementStockService = MouvementStockService;

        }
        [AllowAnonymous]
        [HttpPost(nameof(Transfers))]
        public Task<Result<bool>> Transfers(ComHeaderRequest Commande)
        {
            var result = _MouvementStockService.Transfers(Commande);
            return result;
        }
    }
}
