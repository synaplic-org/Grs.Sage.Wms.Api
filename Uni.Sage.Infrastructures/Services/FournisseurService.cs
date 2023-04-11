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
    public interface IFournisseurService
    {
        Task<IResult<List<FournisseurResponce>>> GetFournisseurs(string pConnexionName);

    }
    public class FournisseurService : IFournisseurService
    {
        private readonly IQueryService _QueryService;

        public FournisseurService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<FournisseurResponce>>> GetFournisseurs(string pConnexionName)
        {

            try
            {

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_FOURNISSEUR");

                var results = await db.QueryAsync<FournisseurResponce>(oQuery);

                return await Result<List<FournisseurResponce>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get fournisseurs societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<FournisseurResponce>>.FailAsync(ex);
            }

        }


       

    }
}
