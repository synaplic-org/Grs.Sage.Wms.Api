using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface ICatTarifService
    {
        Task<IResult<List<CatTarifResponse>>> GetCategorieTarifaires(string pConnexionName);
    }
    public class CatTarifSevice : ICatTarifService
    {
        private readonly IQueryService _QueryService;

        public CatTarifSevice(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<CatTarifResponse>>> GetCategorieTarifaires(string pConnexionName)
        {
            
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_CATTARIF");

                var results = await db.QueryAsync<CatTarifResponse>(oQuery);

                return await Result<List<CatTarifResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Categorie tarifaire societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<CatTarifResponse>>.FailAsync(ex);
            }

        }
    }
}
