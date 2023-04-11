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

    public interface ICompteCollectifService
    {
        Task<IResult<List<CompteCollectifResponse>>> GetCompteCollectif(string pConnexionName);
        
    }

    public class CompteCollectifService : ICompteCollectifService
    {

        private readonly IQueryService _QueryService;

        public CompteCollectifService(IQueryService queryService)
        {

            _QueryService = queryService;
        }

        public async Task<IResult<List<CompteCollectifResponse>>> GetCompteCollectif(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_F_COMPTEG_MIN");
                var results = await db.QueryAsync<CompteCollectifResponse>(oQuery);

                return await Result<List<CompteCollectifResponse>>.SuccessAsync(results.ToList());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, " Get compte collectif  societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<CompteCollectifResponse>>.FailAsync(ex);
            }
        }

    }
}
