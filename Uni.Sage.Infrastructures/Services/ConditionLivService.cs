using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{

    public interface IConditionLivService
    {
        Task<IResult<List<ConditionLivResponse>>> GetCondLiv(string pConnexionName);
    }

    public class ConditionLivService : IConditionLivService
    {

        private readonly IQueryService _QueryService;

        public ConditionLivService(IQueryService queryService)
        {

            _QueryService = queryService;

        }

        public async Task<IResult<List<ConditionLivResponse>>> GetCondLiv(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_CONDITION_LIVRAISON");
                var results = await db.QueryAsync<ConditionLivResponse>(oQuery);

                return await Result<List<ConditionLivResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Condition Livraison societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ConditionLivResponse>>.FailAsync(ex);
            }

        }
        }
     
}
