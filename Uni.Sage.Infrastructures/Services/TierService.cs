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
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{

    public interface ITierService
    {
        Task<IResult<TiersResponse>> CreateTier(TiersRequest Request);

    }
    public class TierService : ITierService
    {
        private readonly IQueryService _QueryService;

        public TierService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<TiersResponse>> CreateTier(TiersRequest Request)
        {

            var Result = new Result<TiersResponse>();

            try
            {
                using var db = _QueryService.NewDbConnection(Request.ConnectionName);

                using Repositories.SageRepository Repo = _QueryService.NewRepository(Request.ConnectionName);

                // insertion 
                using var insertRepo = _QueryService.NewRepository(Request.ConnectionName);


                using (var dbContextTransaction = insertRepo.BeginTransaction())
                {
                    try
                    {

                        F_COMPTET f_COMPTET = new F_COMPTET();
                        F_CONTACTT f_CONTACTT = new F_CONTACTT();

                        //insertion F_COMPTET
                        f_COMPTET.CT_Num = Request.CodeTier;
                        f_COMPTET.CT_Intitule = Request.Intitule;
                        f_COMPTET.CT_Adresse = Request.Adresse;
                        f_COMPTET.CT_Qualite = Request.Qualite;
                        f_COMPTET.CT_Classement = Request.Classement;
                        f_COMPTET.CT_Telephone = Request.Telephone;
                        f_COMPTET.CT_Identifiant = Request.Identifiant;
                        f_COMPTET.CT_Ville = Request.Ville;
                        f_COMPTET.CT_Contact = Request.PrenomContact + Request.NomContact;
                        f_COMPTET.CT_NumPayeur = Request.TiersPayeur;
                        f_COMPTET.CT_Type = (short)Request.Type;
                        f_COMPTET.CG_NumPrinc = Request.CompteCollectif;
                        f_COMPTET.CT_Complement = Request.Complement;
                        f_COMPTET.CT_CodePostal = Request.CodePostal;
                        f_COMPTET.CT_CodeRegion = Request.Region;
                        f_COMPTET.N_Devise = short.Parse(Request.Devise);
                        f_COMPTET.N_CatTarif = (short)Request.CategorieTarrifiaire;
                        f_COMPTET.N_Period = (short)Request.Periodicite;
                        f_COMPTET.CT_BLFact = (short)Request.FactureBL;
                        f_COMPTET.CT_Sommeil = (short)Request.MettreEnSommeil;
                        f_COMPTET.N_CatCompta = (short)Request.Comptabilite;
                        f_COMPTET.N_Expedition = (short)Request.ModeExpedition;
                        f_COMPTET.N_Condition = (short)Request.ConditionLivraison;
                       
                       

                        //switch (Request.Type)
                        //{
                        //    case 0: f_COMPTET.CG_NumPrinc = "342100000";
                        //           break;
                        //    case 1: f_COMPTET.CG_NumPrinc = "441100000";
                        //             break;
                        //    case 2: f_COMPTET.CG_NumPrinc = "342100000";
                        //             break;
                        //}

                        //f_COMPTET.CO_No = nombrecollab;
                        await insertRepo.QueryAsync<int>("INSERT_TIERS", f_COMPTET);



                        //insertion F_CONTACTT
                        f_CONTACTT.CT_Num = Request.CodeTier;
                        f_CONTACTT.CT_Prenom = Request.PrenomContact;
                        f_CONTACTT.CT_Nom = Request.NomContact;
                        f_CONTACTT.CT_Civilite = (short)Request.Civilite;
                        f_CONTACTT.CT_Fonction = Request.Fonction;
                        f_CONTACTT.N_Service = (short)Request.Service;
                        f_CONTACTT.N_Contact = (short)Request.TypeContact;
                        f_CONTACTT.CT_Telephone = Request.TelephoneContact;
                        f_CONTACTT.CT_TelPortable = Request.Portable;

                        await insertRepo.QueryAsync<int>("INSERT_CONTACTT", f_CONTACTT);


                        dbContextTransaction.Commit();

                        return await Result<TiersResponse>.SuccessAsync(Result.Data);

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Insert into fournisseur  {0}  error : {1}", Request.ConnectionName, ex.ToString());
                return await Result<TiersResponse>.FailAsync(ex);
            }

        }
    }
}
