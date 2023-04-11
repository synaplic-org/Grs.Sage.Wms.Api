
using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IFamilleService
    {
        Task<IResult<List<FamilleResponse>>> GetFamilles(string pConnexionName);
    }
    public class FamilleService: IFamilleService
    {
        private readonly IQueryService _QueryService;

        public FamilleService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<FamilleResponse>>> GetFamilles(string pConnexionName)
        {
            try
            {


                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_FAMILLE");
                var results = await db.QueryAsync<FamilleResponse>(oQuery);

                return await Result<List<FamilleResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Familles societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<FamilleResponse>>.FailAsync(ex);
            }

        }
    }
}
