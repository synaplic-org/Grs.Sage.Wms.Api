using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Extention;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
    public interface IReglementService
    {
        Task<IResult<List<ReglementResponse>>> GetReglement(string pConnexionName);
        Task<IResult<ReglementResponse>> CreateReglement(ReglementRequest Request);
        Task<IResult<List<ModePaiementResponse>>> GetMode(string pConnexionName);

    }
    public class ReglementService : IReglementService
    {
            private readonly IQueryService _QueryService;

            public ReglementService(IQueryService queryService)
            {

                _QueryService = queryService;
            }

    
    public async Task<IResult<List<ReglementResponse>>> GetReglement(string pConnexionName)
    {

        try
        {
            using var db = _QueryService.NewDbConnection(pConnexionName);
            var oQuery = _QueryService.GetQuery("SELECT_REGLEMENT");

            var results = await db.QueryAsync<ReglementResponse>(oQuery);

            return await Result<List<ReglementResponse>>.SuccessAsync(results.ToList());
        }
        catch (System.Exception ex)
        {
            Log.Fatal(ex, " Get Reglements societe {0}  error : {1}", pConnexionName, ex.ToString());
            return await Result<List<ReglementResponse>>.FailAsync(ex);
        }

    }

        public async Task<IResult<ReglementResponse>> CreateReglement(ReglementRequest Request)
        {
            var Result = new Result<ReglementResponse>();

            try
            {
                using var db = _QueryService.NewDbConnection(Request.ConnectionName);

                using Repositories.SageRepository Repo = _QueryService.NewRepository(Request.ConnectionName);

                // insertion to F_CREGLEMENT
                using var insertRepo = _QueryService.NewRepository(Request.ConnectionName);
                

                using (var dbContextTransaction = insertRepo.BeginTransaction())
                {
                    try
                    {
                        var piece = await insertRepo.ExecuteScalarAsync<string>("SELECT_CURRENT_RG_PIECE");

                        F_CREGLEMENT f_CREGLEMENT = new F_CREGLEMENT();
                        var modeReg = Request.Mode;
               
                        f_CREGLEMENT.CT_NumPayeur = Request.CodeClient;
                        f_CREGLEMENT.CT_NumPayeurOrig = Request.CodeClient;
                        f_CREGLEMENT.RG_Date = Request.Date;
                        f_CREGLEMENT.RG_DateCreate = Request.Date;
                        f_CREGLEMENT.RG_Reference = Request.Reference;
                        f_CREGLEMENT.RG_Libelle = Request.Libelle;
                        f_CREGLEMENT.RG_Montant = Request.Montant;
                        f_CREGLEMENT.N_Reglement = Convert.ToInt16(modeReg);
                        f_CREGLEMENT.RG_Piece = piece.Increment();
                        f_CREGLEMENT.JO_Num = Request.Journal;
                        f_CREGLEMENT.CG_Num = Request.CompteCollectif;
                        await insertRepo.QueryAsync<int>("INSERT_REGLEMENT",f_CREGLEMENT);
                        dbContextTransaction.Commit();

                        return await Result<ReglementResponse>.SuccessAsync(Result.Data);

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Insert into reglement societe {0}  error : {1}", Request.ConnectionName, ex.ToString());
                return await Result<ReglementResponse>.FailAsync(ex);
            }
        }

        public async Task<IResult<List<ModePaiementResponse>>> GetMode(string pConnexionName)
        {

            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_MODE_PAIEMENT");

                var results = await db.QueryAsync<ModePaiementResponse>(oQuery);

                return await Result<List<ModePaiementResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Reglements societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<ModePaiementResponse>>.FailAsync(ex);
            }

        }


    }
}
