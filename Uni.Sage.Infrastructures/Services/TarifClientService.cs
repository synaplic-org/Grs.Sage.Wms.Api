using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Model;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface ITarifClientService
    {
        Task<IResult<List<TarifClientResponse>>> GetTarifClient(string pConnexionName);
    }
    public class TarifClientService: ITarifClientService
    {
        private readonly IQueryService _QueryService;

        public TarifClientService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<TarifClientResponse>>> GetTarifClient(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_TARIFCLIENT");
                var results = await db.QueryAsync<TarifClientResponse>(oQuery);

                return await Result<List<TarifClientResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get tarif client societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<TarifClientResponse>>.FailAsync(ex);
            }

        }
   
    }
}
