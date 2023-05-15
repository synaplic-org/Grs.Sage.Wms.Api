using Dapper;
using Grs.Sage.Wms.Api.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
	
		public interface IDocLigneService
	{
			Task<Result<List<DocligneResponse>>> GetDocLigne(string pConnexionName);
		}

		public class DocLigneService : IDocLigneService
	{
			private readonly IQueryService _QueryService;

			public DocLigneService(IQueryService queryService)
			{
				_QueryService = queryService;
			}

			public async Task<Result<List<DocligneResponse>>> GetDocLigne(string pConnexionName)
			{
				try
				{

					using var db = _QueryService.NewDbConnection(pConnexionName);
					var oQuery = _QueryService.GetQuery("SELECT_F_DOCLIGNE");
					var results = await db.QueryAsync<DocligneResponse>(oQuery);

					return await Result<List<DocligneResponse>>.SuccessAsync(results.ToList());
				}
				catch (System.Exception ex)
				{
					Log.Fatal(ex, " Get Depots societe {0}  error : {1}", pConnexionName, ex.ToString());
					return await Result<List<DocligneResponse>>.FailAsync(ex);
				}

			}
		}
	
}
