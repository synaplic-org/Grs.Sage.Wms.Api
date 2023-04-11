using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IExpeditionService
    {
        Task<IResult<List<ExpeditionResponse>>> GetExpedition(string pConnexionName);
    }

    public class ExpeditionService : IExpeditionService
    {
        
        private readonly IQueryService _QueryService;

        public ExpeditionService(  IQueryService queryService)
        {
            
            _QueryService = queryService;

        }

        public async Task<IResult<List<ExpeditionResponse>>> GetExpedition(string pConnexionName)
        {
            try
            {
              
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_EXPEDITION");
                var results = await db.QueryAsync<ExpeditionResponse>(oQuery);

                return await Result<List<ExpeditionResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Expeditions societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ExpeditionResponse>>.FailAsync(ex);
            }

        }

    }


}
