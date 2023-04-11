
using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IDevisService
    {
        Task<IResult<List<Devis_EnteteResponse>>> Devis_Entete_Vente(string pConnexionName);
        Task<IResult<List<Devis_EnteteResponse>>> Devis_Entete_Achat(string pConnexionName);
        Task<IResult<List<Devis_LigneResponse>>> Devis_Ligne_Vente(string pConnexionName);
        Task<IResult<List<Devis_LigneResponse>>> DevisLigneAchat(string pConnexionName);
    }
    public class DevisService : IDevisService
    {
        private readonly IQueryService _QueryService;

        public DevisService( IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<Devis_EnteteResponse>>> Devis_Entete_Vente(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEVIS_ENTETE_VENTE");
                var results = await db.QueryAsync<Devis_EnteteResponse>(oQuery);

                return await Result<List<Devis_EnteteResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Devis Entete vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Devis_EnteteResponse>>.FailAsync(ex);
            }

        }
        public async Task<IResult<List<Devis_EnteteResponse>>> Devis_Entete_Achat(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEVIS_ENTETE_ACHAT");
                var results = await db.QueryAsync<Devis_EnteteResponse>(oQuery);

                return await Result<List<Devis_EnteteResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Devis Entete achat societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Devis_EnteteResponse>>.FailAsync(ex);
            }

        }
        public async Task<IResult<List<Devis_LigneResponse>>> Devis_Ligne_Vente(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEVIS_LIGNE_VENTE");
                var results = await db.QueryAsync<Devis_LigneResponse>(oQuery);

                return await Result<List<Devis_LigneResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Devis Ligne Vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Devis_LigneResponse>>.FailAsync(ex);
            }

        }
        public async Task<IResult<List<Devis_LigneResponse>>> DevisLigneAchat(string pConnexionName)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEVIS_LIGNE_ACHAT");
                var results = await db.QueryAsync<Devis_LigneResponse>(oQuery);

                return await Result<List<Devis_LigneResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Devis Ligne Achat societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Devis_LigneResponse>>.FailAsync(ex);
            }

        }


    }
}
