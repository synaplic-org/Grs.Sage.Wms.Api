using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Filter;
using Uni.Sage.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;

namespace Grs.Sage.Wms.Api.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IClientService _ClientService;

        public ClientController(IClientService clientService) : base()
        {

            _ClientService = clientService;

        }

		[AllowAnonymous]
		[HttpGet(nameof(GetClients))]
        public async Task<IResult<List<ClientResponse>>> GetClients(string pConnexionName)
        {
            var result = await _ClientService.GetClients(pConnexionName);
            return result;  
        }
		[AllowAnonymous]
		[HttpGet(nameof(GetComptet))]
        public async Task<IResult<List<ClientResponse>>> GetComptet(string pConnexionName)
        {
            var result = await _ClientService.GetComptet(pConnexionName);
            return result;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllClientPaginate(int pageNumber, int pageSize, string pConnexionName)
        //{
        //    //var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        //    var pagedData =  _ClientService.GetClients(pConnexionName).Result.Data
        //                                        .Skip((pageNumber - 1) * pageSize)
        //                                        .Take(pageSize)
        //                                        .ToList();
        //    var totalRecords =  _ClientService.GetClients(pConnexionName).Result.Data.Count;
        //   return Ok(new PagedResponse<List<ClientResponse>>(pagedData, pageNumber, pageSize));
        //}
    }
}
