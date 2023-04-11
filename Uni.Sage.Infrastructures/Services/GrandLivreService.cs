using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Serilog;
using Uni.Sage.Application.Contrats.Responses;
using System.Threading.Tasks;
using Uni.Sage.Shared.Wrapper;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Infrastructures.Services
{
    public interface  IGrandLivreService
    {
        Task<IResult<List<GrandLivreResponse>>> GetGrandLivres(string pConnexionName);
        Task<Result<PaginatedResult<GrandLivreResponse>>> GetEcheance(string pConnexionName, int pageNumber, int pageSize);

    }
    public class GrandLivreService : IGrandLivreService
    {
        private readonly IQueryService _QueryService;

        public GrandLivreService(IQueryService queryService)
        {

            _QueryService = queryService;
        }


        public async Task<Result<PaginatedResult<GrandLivreResponse>>> GetEcheance(string pConnexionName, int pageNumber, int pageSize)
        {

            try
            {
                using var insertRepo = _QueryService.NewRepository(pConnexionName);
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_ECHEANCE_PAGINATED");
                pageNumber = (pageNumber - 1) * pageSize;
                var parameters = new { pageNumber = pageNumber, pageSize = pageSize };

                var result = await db.QueryAsync<GrandLivreResponse>(oQuery, parameters);

                bool success = false;

                if (result != null)
                {
                    success = true;
                }
                var total = await insertRepo.ExecuteScalarAsync<string>("SELECT_COUNT_ECHEANCE");
                var totalRecords = Int32.Parse(total);
                var response = new PaginatedResult<GrandLivreResponse>(success, result.ToList(), null, totalRecords, pageNumber, pageSize);

                return await Result<PaginatedResult<GrandLivreResponse>>.SuccessAsync(response);
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Grands Livre societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<PaginatedResult<GrandLivreResponse>>.FailAsync(ex);
            }

        }

        public async Task<IResult<List<GrandLivreResponse>>> GetGrandLivres(string pConnexionName)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_GRAND_LIVRE");

                var results = await db.QueryAsync<GrandLivreResponse>(oQuery);

                return await Result<List<GrandLivreResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Grands Livre societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<GrandLivreResponse>>.FailAsync(ex);
            }

        }
    }
}
