using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
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
        public async  Task<Result<List<FournisseurResponce>>> GetFournisseurs(string pConnexionName)
        {
            var result = await _FournisseurService.GetFournisseurs(pConnexionName);
            return result;
        }

    }
}
