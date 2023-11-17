using Grs.Sage.Wms.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Objets100cLib;
using System;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api.Controllers
{
	public class InventoryController : BaseController
	{
		private readonly IInventoryService _InventoryService;

		public InventoryController(IInventoryService inventoryService) : base()
		{

            _InventoryService = inventoryService;

		}
       

    }
}
