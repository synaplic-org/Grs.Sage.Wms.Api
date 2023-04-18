
using Dapper;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uni.Sage.Domain.Entities;

using Uni.Sage.Shared.Wrapper;

namespace Grs.Sage.Wms.Api.Services
{
    public interface ICollaborateurService
    {
        Task<Result<List<CollaborateurResponse>>> GetCollaborateur(string pConnexionName);
    }

    public class CollaborateurService : ICollaborateurService
    {
       
        private readonly IQueryService _QueryService;

        public CollaborateurService(  IQueryService queryService)
        {
          
            _QueryService = queryService;
        }

        public async Task<Result<List<CollaborateurResponse>>> GetCollaborateur(string pConnexionName)
        {   
            try
            {

                

                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_COLLABORATEUR_MIN");
                var results = await db.QueryAsync<CollaborateurResponse>(oQuery);

                return await Result<List<CollaborateurResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get collaborateur societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<CollaborateurResponse>>.FailAsync(ex);
            }
        }

    }


}

