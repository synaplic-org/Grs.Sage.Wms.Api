
using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface ISoucheService
    {
        //Task<IResult<List<SoucheResponse>>> GetSouchesByDomain(string pConnexionName, string domaine);
        Task<IResult<List<SoucheResponse>>> GetSouchesVente(string pConnexionName);
        Task<IResult<List<SoucheResponse>>> GetSouchesVenteById(string pConnexionName,string Souche);
        Task<IResult<List<SoucheResponse>>> GetSouchesAchat(string pConnexionName);
        Task<IResult<List<SoucheResponse>>> GetSouchesInterne(string pConnexionName);
        

    }

    public class SoucheService : ISoucheService
    {
          
        private readonly IQueryService _QueryService;

        //public const string _Vente = "VENTE";
        //public const string _Achat = "ACHAT";
        //public const string _Interne = "INTERNE";


        public SoucheService(  IQueryService queryService)
        {
           
            _QueryService = queryService;
        }

        
        public async Task<IResult<List<SoucheResponse>>> GetSouchesAchat(string pConnexionName)
        {
            try
            {
               
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_SOUCHE_ACHAT");
                var results = await db.QueryAsync<SoucheResponse>(oQuery);

                return await Result<List<SoucheResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Souche achat societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<SoucheResponse>>.FailAsync(ex);
            }
        }

        //public async Task<IResult<List<SoucheResponse>>> GetSouchesByDomain(string pConnexionName,string pDomain)
        //{
        //    var messages = new List<string>();
        //    try
        //    {
        //        var connexion = _QueryService.NewDbConnection(pConnexionName);
        //        using var db = _QueryService.NewDbConnection(connexion);

        //        var oQuery = string.Empty;
        //        switch (pDomain)
        //        {
        //            case _Vente: oQuery = SharedQueries.SELECT_SOUCHE_VENTE;break;
        //            case _Achat: oQuery = SharedQueries.SELECT_SOUCHE_ACHAT;break;
        //            case _Interne: oQuery = SharedQueries.SELECT_SOUCHE_INTERNE;break;
        //            default: throw new System.ApplicationException($" le domain [{pDomain }]   est introuvable ! ");
        //        }

        //        var results = await db.QueryAsync<SoucheResponse>(oQuery);

        //        return await Result<List<SoucheResponse>>.SuccessAsync(results.ToList());
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Log.Fatal(ex, " Get SouchesByDomain societe {0}  error : {1}", pConnexionName, ex.ToString());
        //        return await Result<List<SoucheResponse>>.FailAsync(ex);
        //    }

        //}

        public async Task<IResult<List<SoucheResponse>>> GetSouchesInterne(string pConnexionName)
        {
            try
            {
           
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_SOUCHE_INTERNE");
                var results = await db.QueryAsync<SoucheResponse>(oQuery);

                return await Result<List<SoucheResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Souche interne societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<SoucheResponse>>.FailAsync(ex);
            }
        }

        public async Task<IResult<List<SoucheResponse>>> GetSouchesVente(string pConnexionName)
        {
            try
            {
                 
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_SOUCHE_VENTE");
                var results = await db.QueryAsync<SoucheResponse>(oQuery);

                return await Result<List<SoucheResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Souche vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<SoucheResponse>>.FailAsync(ex);
            }

        }
        public async Task<IResult<List<SoucheResponse>>> GetSouchesVenteById(string pConnexionName,string Souche)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_SOUCHE_BY_ID");
                var results = await db.QueryAsync<SoucheResponse>(oQuery,new { Souche });
                

                return await Result<List<SoucheResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Souche vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<SoucheResponse>>.FailAsync(ex);
            }

        }
    }


}
