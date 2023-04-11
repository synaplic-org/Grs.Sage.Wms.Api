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


    public interface IDeviseService
    {
        Task<IResult<List<DeviseResponse>>> GetDevise(string pConnexionName);

    }
    public class DeviseService : IDeviseService
    {
        private readonly IQueryService _QueryService;

        public DeviseService(IQueryService queryService)
        {

            _QueryService = queryService;
        }

        public async Task<IResult<List<DeviseResponse>>> GetDevise(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_P_DEVISE");
                var results = await db.QueryAsync<DeviseResponse>(oQuery);

                return await Result<List<DeviseResponse>>.SuccessAsync(results.ToList());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, " Get devise  societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<DeviseResponse>>.FailAsync(ex);
            }
        }


    }
}
