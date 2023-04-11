using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
    public interface ITarifFournissService
    {
        Task<IResult<List<TarifFournissResponse>>> GetTarifsFournisseurs(string pConnexionName);
    }
    public  class TarifFournissService : ITarifFournissService
    {
        private readonly IQueryService _QueryService;

        public TarifFournissService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<TarifFournissResponse>>> GetTarifsFournisseurs(string pConnexionName)
        {

            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_TARIFFOURNISS");

                var results = await db.QueryAsync<TarifFournissResponse>(oQuery);

                return await Result<List<TarifFournissResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get tarifs fournisseurs societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<TarifFournissResponse>>.FailAsync(ex);
            }

        }
    }
}
