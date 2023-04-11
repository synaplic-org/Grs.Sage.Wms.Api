using Dapper;
using Serilog;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface IAffairesService
    {
        Task<IResult<List<AffaireResponse>>> GetAffaires(string pConnexionName);
        Task<IResult<List<AffaireResponse>>> GetTiersAffaires(string pConnexionName);
        Task<IResult<List<AffaireResponse>>> GetAffairesAchat(string pConnexionName);
        Task<IResult<List<AffaireResponse>>> GetAffairesVente(string pConnexionName);
    }

    public class AffaireService : IAffairesService
    {
        
        private readonly IQueryService _QueryService;

        public AffaireService(  IQueryService queryService)
        {
           
            _QueryService = queryService;
        }

        public async Task<IResult<List<AffaireResponse>>> GetAffaires(string pConnexionName)
        {
            try
            {
                 
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_AFFAIRE");
                var results = await db.QueryAsync<AffaireResponse>(oQuery);

                return await Result<List<AffaireResponse>>.SuccessAsync(results.ToList());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, " Get affaire  societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<AffaireResponse>>.FailAsync(ex);
            }
        }
        public async Task<IResult<List<AffaireResponse>>> GetTiersAffaires(string pConnexionName)
        {
            try
            {
                 
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_F_COMPTEA_MIN");
                var results = await db.QueryAsync<AffaireResponse>(oQuery);

                return await Result<List<AffaireResponse>>.SuccessAsync(results.ToList());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, " Get affaire  societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<AffaireResponse>>.FailAsync(ex);
            }
        }
        public async Task<IResult<List<AffaireResponse>>> GetAffairesVente(string pConnexionName)
        {
            try
            {
               
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_AFFAIRE_VENTE");
                var results = await db.QueryAsync<AffaireResponse>(oQuery);

                return await Result<List<AffaireResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get affaire vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<AffaireResponse>>.FailAsync(ex);
            }
        }
        public async Task<IResult<List<AffaireResponse>>> GetAffairesAchat(string pConnexionName)
        {
            try
            {
                
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_AFFAIRE_ACHAT");
                var results = await db.QueryAsync<AffaireResponse>(oQuery);

                return await Result<List<AffaireResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get affaire achat societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<AffaireResponse>>.FailAsync(ex);
            }
        }

    }

}
