﻿using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Wrapper;
using Uni.Sage.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Grs.Sage.Wms.Api.Controllers
    {
        public class CollaborateurController : BaseController
        {
            private readonly ICollaborateurService _CollaborateurService;

            public CollaborateurController(ICollaborateurService CollaborateurService) : base()
            {

                _CollaborateurService = CollaborateurService;
            }

		    [AllowAnonymous]
		    [HttpGet(nameof(GetCollaborateur))]
            public async Task<Result<List<CollaborateurResponse>>> GetCollaborateur(string pConnexionName)
            {
                return await _CollaborateurService.GetCollaborateur(pConnexionName);
            }
        }
    }

