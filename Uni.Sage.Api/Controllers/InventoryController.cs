using Grs.Sage.Wms.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Infrastructures.Services;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Controllers
{
	public class InventoryController : BaseController
	{
		private readonly IInventoryService _InventoryService;

		public InventoryController(IInventoryService inventoryService) : base()
		{

            _InventoryService = inventoryService;

		}
        [AllowAnonymous]
        [HttpPost(nameof(Inventaire))]
        public Task<Result<bool>> Inventaire(List<InventoryRequest> request)
        {
            var result = _InventoryService.IntegrationInventaire(request);
            return result;
        }

    }
}
