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
    public interface IJournalService
    {
        Task<IResult<List<JournalResponse>>> GetJournaux(string pConnexionName);
    }

    public class JournalService : IJournalService
    {
        private readonly IQueryService _QueryService;

        public JournalService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<JournalResponse>>> GetJournaux(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_JOURNAUX");
                var results = await db.QueryAsync<JournalResponse>(oQuery);

                return await Result<List<JournalResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Depots societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<JournalResponse>>.FailAsync(ex);
            }

        }
    }
}
