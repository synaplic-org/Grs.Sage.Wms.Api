using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Wrapper;
using Uni.Sage.Domain.Entities;

namespace Grs.Sage.Wms.Api.Controllers
    {
        public class CollaborateurController : BaseController
        {
            private readonly ICollaborateurService _CollaborateurService;

            public CollaborateurController(ICollaborateurService CollaborateurService) : base()
            {

                _CollaborateurService = CollaborateurService;

            }


            [HttpGet(nameof(GetCollaborateur))]
            public async Task<Result<List<CollaborateurResponse>>> GetCollaborateur(string pConnexionName)
            {
                var result = await _CollaborateurService.GetCollaborateur(pConnexionName);
                return result;
            }
        }
    }

