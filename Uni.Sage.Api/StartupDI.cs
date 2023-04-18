using Microsoft.Extensions.DependencyInjection;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Infrastructures.Services;

namespace Grs.Sage.Wms.Api
{
    public partial class Startup
    {
        public void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IConnexionService, ConnexionService>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ICollaborateurService, CollaborateurService>();
            services.AddScoped<IDepotService, DepotService>();
            services.AddScoped<IEmplacementService, EmplacementService>();
            services.AddScoped<IFamilleService, FamilleService>();
            services.AddScoped<IFournisseurService, FournisseurService>();
        }
    }
}   