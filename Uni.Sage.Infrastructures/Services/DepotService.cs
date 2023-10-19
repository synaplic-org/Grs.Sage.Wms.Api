using Dapper;
using Serilog;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Infrastructures.Mapper;
using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IDepotService
    {
        Task<Result<List<DepotResponse>>> GetDepots(string pConnexionName);
        Task<Result<DepotResponse>> PostDepot(DepotRequest Depot);
    }

    public class DepotService : IDepotService
    {
        private readonly IQueryService _QueryService;

        public DepotService(IQueryService queryService)
        {
            _QueryService = queryService;
        }
        
        public async Task<Result<List<DepotResponse>>> GetDepots(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEPOT");
                var results = await db.QueryAsync<DepotResponse>(oQuery);

                return await Result<List<DepotResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Depots societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<DepotResponse>>.FailAsync(ex);
            }

        }
        public async Task<Result<DepotResponse>> PostDepot(DepotRequest Depot)
        {
            try
            {
                var Result = new Result<DepotResponse>();
                using var db = _QueryService.NewDbConnection(Depot.pConnexionName);

                using var insertRepo = _QueryService.NewRepository(Depot.pConnexionName);
                using (var dbContextTransaction = insertRepo.BeginTransaction())
                {
                    try
                    {
                        
                        F_DEPOT oF_DEPOT = DepotMapper.Adapt(Depot);
                        var id = await insertRepo.QueryAsync<int>("INSERT_DEPOT", oF_DEPOT);
                        dbContextTransaction.Commit();
                        var oQuery = _QueryService.GetQuery("SELECT_DEPOT_MAX");
                        var results = await db.QueryAsync<int>(oQuery);
                        var Res = results.ToList();
                        var oQueryEmp = _QueryService.GetQuery("SELECT_DEPOT_EMP_MAX");
                        var resultsEmp = await db.QueryAsync<int>(oQueryEmp);
                        var ResEmpl = resultsEmp.ToList();
                        DepotEmpRequest DepotEmp = new DepotEmpRequest();
                        DepotEmp.DE_NO = Res[0];
                        DepotEmp.DP_NO= ResEmpl[0];
                        F_DEPOTEMPL oF_DEPOTEmp = DepotEmplMapper.Adapt(DepotEmp);
                        await insertRepo.QueryAsync<int>("INSERT_DEPOT_EMP", oF_DEPOTEmp);
                        //dbContextTransaction.Commit();
                        Result.Data = new DepotResponse() { ErpID = oF_DEPOT.DE_NO };
                        return await Result<DepotResponse>.SuccessAsync(data: Result.Data);

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }

            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " post Depots societe {0}  error : {1}", Depot.pConnexionName, ex.ToString());
                return await Result<DepotResponse>.FailAsync(ex);
            }

        }
    }
}
