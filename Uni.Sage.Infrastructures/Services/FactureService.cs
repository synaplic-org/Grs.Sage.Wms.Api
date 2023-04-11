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
    public interface IFactureService
    {
        Task<IResult<List<Facture_EntiteResponse>>> Facture_Entete_Vente(string pConnexionName);
        Task<IResult<List<Facture_EntiteLignResponse>>> Facture_Ligne_Vente(string pConnexionName, string do_peice);
    }
    public class FactureService : IFactureService
    {
        private readonly IQueryService _QueryService;

        public FactureService(IQueryService queryService)
        {

            _QueryService = queryService;
        }

        public async Task<IResult<List<Facture_EntiteResponse>>> Facture_Entete_Vente(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_F_DOCENTETE");

                var results = await db.QueryAsync<Facture_EntiteResponse>(oQuery, new { DO_Domaine = 0, DO_Type = 6 });

                return await Result<List<Facture_EntiteResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get facture Entete vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Facture_EntiteResponse>>.FailAsync(ex);
            }

        }

        public async Task<IResult<List<Facture_EntiteLignResponse>>> Facture_Ligne_Vente(string pConnexionName, string do_peice)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_F_DOCLIGNE");
                var results = await db.QueryAsync<Facture_EntiteLignResponse>(oQuery, new { DO_Domaine = 0, DO_Type = 6, Piece = do_peice });

                return await Result<List<Facture_EntiteLignResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get facture Ligne Vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Facture_EntiteLignResponse>>.FailAsync(ex);
            }

        }

    }
}

