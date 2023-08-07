using Dapper;
using Grs.Sage.Wms.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
	
		public interface IDocEnteteService
		{
			Task<Result<List<DocEnteteResponse>>> GetDocEntete(string pConnexionName);
            string TransformerBl(DocumentVente Commande);
        object TransformerBL(DocumentVente commande);
    }

		public class DocEnteteService : IDocEnteteService
		{
			private readonly IQueryService _QueryService;

			public DocEnteteService(IQueryService queryService)
			{
				_QueryService = queryService;
			}

			public async Task<Result<List<DocEnteteResponse>>> GetDocEntete(string pConnexionName)
			{
				try
				{

					using var db = _QueryService.NewDbConnection(pConnexionName);
					var oQuery = _QueryService.GetQuery("SELECT_F_DOCENTETE");
					var results = await db.QueryAsync<DocEnteteResponse>(oQuery);

					return await Result<List<DocEnteteResponse>>.SuccessAsync(results.ToList());
				}
				catch (System.Exception ex)
				{
					Log.Fatal(ex, " Get Depots societe {0}  error : {1}", pConnexionName, ex.ToString());
					return await Result<List<DocEnteteResponse>>.FailAsync(ex);
				}

			}

        public string TransformerBl(DocumentVente Commande)
        {
            throw new NotImplementedException();
        }

        public object TransformerBL(DocumentVente commande)
        {
            throw new NotImplementedException();
        }
        // Objet base commerciale

    }

}
