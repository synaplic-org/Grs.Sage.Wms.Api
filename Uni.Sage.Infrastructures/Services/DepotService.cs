
using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IDepotService
    {
        Task<Result<List<DepotResponse>>> GetDepots(string pConnexionName);
    }

    public class DepotService : IDepotService
    {
        private readonly IQueryService _QueryService;

        public DepotService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<Result<List<DepotResponse>>> GetDepots(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEPOT");
                var results = await db.QueryAsync<DepotResponse>(oQuery);

                return await Result<List<DepotResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Depots societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<DepotResponse>>.FailAsync(ex);
            }

        }
    }
}
