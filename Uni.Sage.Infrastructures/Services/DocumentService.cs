using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
    public interface IDocumentService
    {
        Task<IResult<List<DocumentResponse>>> GetDocumentDevisVente(string pConnexionName ,int Souche);
        Task<IResult<List<string>>> UpdateDocumentDevisVente(string pConnexionName, int Souche, string NPiece);
    }
    public  class DocumentService : IDocumentService
    {
        private readonly IQueryService _QueryService;

        public DocumentService(IQueryService queryService)
        {

            _QueryService = queryService;
        }
        public async Task<IResult<List<DocumentResponse>>> GetDocumentDevisVente(string pConnexionName, int Souche)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DOCCURRENT_PIECE_DEVIS_VENTE");
                var results = await db.QueryAsync<DocumentResponse>(oQuery, new { Souche });


                return await Result<List<DocumentResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Piece vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<DocumentResponse>>.FailAsync(ex);
            }

        }


        public async Task<IResult<List<string>>> UpdateDocumentDevisVente(string pConnexionName, int Souche, string NPiece)
        {
            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("UPDATE_DOCCURRENT_PIECE_DEVIS_VENTE");
                var results = await db.QueryAsync<string>(oQuery, new { Souche,NPiece });


                return await Result<List<string>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Piece vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<string>>.FailAsync(ex);
            }

        }
    }
}
