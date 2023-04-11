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
            services.AddScoped<ICatTarifService, CatTarifSevice>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ISoucheService, SoucheService>();
            services.AddScoped<IAffairesService, AffaireService>();
            services.AddScoped<IDeviseService, DeviseService>();
            services.AddScoped<ICompteCollectifService, CompteCollectifService>();
            services.AddScoped<IExpeditionService, ExpeditionService>();
            services.AddScoped<ICollaborateurService, CollaborateurService>();
            services.AddScoped<IDevisService, DevisService>();
            services.AddScoped<IDepotService, DepotService>();
            services.AddScoped<IJournalService, JournalService>();
            services.AddScoped<IEmplacementService, EmplacementService>();
            services.AddScoped<IFamilleService, FamilleService>();
            services.AddScoped<ITarifClientService, TarifClientService>();
            services.AddScoped<IVenteService, VenteService>(); 
            services.AddScoped<IFournisseurService, FournisseurService>();
            services.AddScoped<ITarifFournissService, TarifFournissService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ILivraisonService, LivraisonService>();
            services.AddScoped<IFactureService, FactureService>();
            services.AddScoped<IGrandLivreService, GrandLivreService>();
            services.AddScoped<IReglementService, ReglementService>();
            services.AddScoped<IBalanceAgeeService, BalanceAgeeService>();
            services.AddScoped<ITierService, TierService>();
            services.AddScoped<IComptabiliteService, ComptabiliteService>();
            services.AddScoped<IConditionLivService, ConditionLivService>();

        }
    }
}   