using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class JournalController : BaseController
    {
        private readonly IJournalService _JournalService;

        public JournalController(IJournalService journalService) : base()
        {

            _JournalService = journalService;

        }


        [HttpGet(nameof(GetJournaux))]
        public async Task<ActionResult> GetJournaux(string pConnexionName)
        {
            var result = await _JournalService.GetJournaux(pConnexionName);
            return Ok(result);
        }
    }
}
