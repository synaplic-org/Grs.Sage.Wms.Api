using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IEmplacementService
    {
        Task<Result<List<EmplacementResponse>>> GetEmplacements(string pConnexionName);
    }
    public class EmplacementService : IEmplacementService
    {
        private readonly IQueryService _QueryService;

        public EmplacementService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<Result<List<EmplacementResponse>>> GetEmplacements(string pConnexionName)
        {
            try
            {
              
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_EMPLACEMENT");
                var results = await db.QueryAsync<EmplacementResponse>(oQuery);

                return await Result<List<EmplacementResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Emplacements societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<EmplacementResponse>>.FailAsync(ex);
            }

        }
    }
}
