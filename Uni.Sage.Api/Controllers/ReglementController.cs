using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class ReglementController : BaseController
    {
        private readonly IReglementService _ReglementService;

        public ReglementController(IReglementService reglementService) : base()
        {

            _ReglementService = reglementService;

        }


        [HttpGet(nameof(GetReglements))]
        public async Task<ActionResult> GetReglements(string pConnexionName)
        {
            var result = await _ReglementService.GetReglement(pConnexionName);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetReglementsPaginate(int pageNumber, int pageSize, string pConnexionName)
        {
            bool success = false;
            int totalRecords;
            var pagedData = await _ReglementService.GetReglement(pConnexionName);
            var result = pagedData.Data.Skip((pageNumber - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToList();
            if (pagedData != null)
            {
                success = true;
            }
             totalRecords = pagedData.Data.Count;
            return Ok(new PaginatedResult<ReglementResponse>(success, result, null,totalRecords, pageNumber, pageSize));
        }

        [HttpPost(nameof(CreateReglement))]

        public async Task<IResult<ReglementResponse>> CreateReglement(ReglementRequest reglement)
        {
            var result = await _ReglementService.CreateReglement(reglement);
            return result;
        }

        [HttpGet(nameof(GetMode))]
        public async Task<ActionResult> GetMode(string pConnexionName)
        {
            var result = await _ReglementService.GetMode(pConnexionName);
            return Ok(result);
        }
    }
}
