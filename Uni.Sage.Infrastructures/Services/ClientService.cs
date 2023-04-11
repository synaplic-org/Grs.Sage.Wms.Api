
using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IClientService
    {
        Task<IResult<List<ClientResponse>>> GetClients(string pConnexionName);
        Task<IResult<List<ClientResponse>>> GetClientsById(string pConnexionName, string CodeClient);
    }
    public class ClientService : IClientService
    {
        private readonly IQueryService _QueryService;

        public ClientService( IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<ClientResponse>>> GetClients(string pConnexionName)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_CLIENT");
                var results = await db.QueryAsync<ClientResponse>(oQuery);

                return await Result<List<ClientResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Clients societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ClientResponse>>.FailAsync(ex);
            }

        }
        public async Task<IResult<List<ClientResponse>>> GetClientsById(string pConnexionName, string CodeClient)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_CLIENT_BYID");
                var results = await db.QueryAsync<ClientResponse>(oQuery, new { CodeClient });

                return await Result<List<ClientResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Clients societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ClientResponse>>.FailAsync(ex);
            }

        }
    }
}
