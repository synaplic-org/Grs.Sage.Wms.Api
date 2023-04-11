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
    public interface IBalanceAgeeService
    {
        Task<IResult<List<BalanceAgeeResponse>>> GetBalance(string pConnexionName);
       

    }
        public class BalanceAgeeService : IBalanceAgeeService
        {

            private readonly IQueryService _QueryService;

            public BalanceAgeeService(IQueryService queryService)
            {

                _QueryService = queryService;
            }


            public async Task<IResult<List<BalanceAgeeResponse>>> GetBalance(string pConnexionName)
            {

                try
                {
                    using var db = _QueryService.NewDbConnection(pConnexionName);
                    var oQuery = _QueryService.GetQuery("SELECT_BALANCE_AGEE");

                    var results = await db.QueryAsync<BalanceAgeeResponse>(oQuery);

                    return await Result<List<BalanceAgeeResponse>>.SuccessAsync(results.ToList());
                }
                catch (System.Exception ex)
                {
                    Log.Fatal(ex, " Get Balance societe {0}  error : {1}", pConnexionName, ex.ToString());
                    return await Result<List<BalanceAgeeResponse>>.FailAsync(ex);
                }

            }
        }
}
