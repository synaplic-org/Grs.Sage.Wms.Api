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
    public interface ILivraisonService
    {
        Task<IResult<List<Livraison_EnteteResponse>>> Livraison_Entete_Vente(string pConnexionName);
        Task<IResult<List<Livraison_EnteteLigneResponse>>> Livraison_Ligne_Vente(string pConnexionName, string do_peice);
    }

    public class LivraisonService : ILivraisonService
    {
        private readonly IQueryService _QueryService;

        public LivraisonService(IQueryService queryService)
        {

            _QueryService = queryService;
        }

        public async Task<IResult<List<Livraison_EnteteResponse>>> Livraison_Entete_Vente(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_F_DOCENTETE");
                
                var results = await db.QueryAsync<Livraison_EnteteResponse>(oQuery, new { DO_Domaine = 0, DO_Type = 2 });

                return await Result<List<Livraison_EnteteResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get livraison Entete vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Livraison_EnteteResponse>>.FailAsync(ex);
            }

        }

        public async Task<IResult<List<Livraison_EnteteLigneResponse>>> Livraison_Ligne_Vente(string pConnexionName,string do_peice)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_F_DOCLIGNE");
                var results = await db.QueryAsync<Livraison_EnteteLigneResponse>(oQuery, new { DO_Domaine = 0, DO_Type = 2, Piece= do_peice });

                return await Result<List<Livraison_EnteteLigneResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get livraison Ligne Vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Livraison_EnteteLigneResponse>>.FailAsync(ex);
            }

        }
    }
}
