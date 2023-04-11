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

    public interface IComptabiliteService
    {
        Task<IResult<List<ComptabiliteResponse>>> GetCompta(string pConnexionName);

    }
    public class ComptabiliteService : IComptabiliteService
    {
        private readonly IQueryService _QueryService;

        public ComptabiliteService(IQueryService queryService)
        {

            _QueryService = queryService;
        }

        public async Task<IResult<List<ComptabiliteResponse>>> GetCompta(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_CATCOMPTA");
                var results = await db.QueryAsync<ComptabiliteResponse>(oQuery);

                return await Result<List<ComptabiliteResponse>>.SuccessAsync(results.ToList());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, " Get Compta  societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ComptabiliteResponse>>.FailAsync(ex);
            }
        }

    
}
}