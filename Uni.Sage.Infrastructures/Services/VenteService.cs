using Dapper;
using Serilog;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Domain.Extentions;
using Uni.Sage.Infrastructures.Mapper;
using Uni.Sage.Shared.Exceptions;
using Uni.Sage.Shared.Extention;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
    public interface IVenteService
    {
        Task<IResult<List<Devis_EnteteResponse>>> Devis_Entete_Vente(string pConnexionName);

        Task<IResult<List<Devis_LigneResponse>>> Devis_Ligne_Vente(string pConnexionName);

        //Task<IResult<List<DocEnteteRequest>>> CreateDevis(DocEnteteRequest devis);
        //Task<List<string>> CountDLNO(string pConnexionName);
        Task<IResult<DocumentResponse>> CreateCommande(DocEnteteRequest commande);
        Task<IResult<DocumentResponse>> CreateDevis(DocEnteteRequest commande);

    }

    public class VenteService : IVenteService
    {
        private readonly IQueryService _QueryService;


        public VenteService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<IResult<List<Devis_EnteteResponse>>> Devis_Entete_Vente(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEVIS_ENTETE_VENTE");
                var results = await db.QueryAsync<Devis_EnteteResponse>(oQuery);

                return await Result<List<Devis_EnteteResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Devis Entete vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Devis_EnteteResponse>>.FailAsync(ex);
            }
        }

        public async Task<IResult<List<Devis_LigneResponse>>> Devis_Ligne_Vente(string pConnexionName)
        {
            try
            {
                using var db = _QueryService.NewDbConnection(pConnexionName);
                var oQuery = _QueryService.GetQuery("SELECT_DEVIS_LIGNE_VENTE");
                var results = await db.QueryAsync<Devis_LigneResponse>(oQuery);

                return await Result<List<Devis_LigneResponse>>.SuccessAsync(results.ToList());
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, " Get Devis Ligne Vente societe {0}  error : {1}", pConnexionName, ex.ToString());
                return await Result<List<Devis_LigneResponse>>.FailAsync(ex);
            }
        }

        public async Task<List<string>> CountDLNO(string pConnexionName)
        {
            using var db = _QueryService.NewDbConnection(pConnexionName);
            var oQuery = _QueryService.GetQuery("SELECT_COUNT_DLNO");
            var results = await db.QueryAsync<string>(oQuery);
            return results.ToList();
        }
        

        public async Task<IResult<DocumentResponse>> CreateDevis(DocEnteteRequest Request)
        {
             var Result = new Result<DocumentResponse>();

            try
            {
                using var db = _QueryService.NewDbConnection(Request.ConnectionName);

                using Repositories.SageRepository Repo = _QueryService.NewRepository(Request.ConnectionName);

                #region Existence client

                //Existence client

                var oClient = await Repo.QueryFirstOrDefaultAsync<ClientResponse>("SELECT_CLIENT_BYID", new { CodeClient = Request.CodeTiers });
                if (oClient == null)
                {
                    throw new ApiException("Client introuvable!");
                }
                if (oClient.EnSommeil == 1)
                {
                    throw new ApiException("Client en sommeil!");
                }

                #endregion Existence client

                #region Existence souche

                //souche(si fournie vérifier l'existence sinon récupérer celle par default)

                var oSouche = await Repo.QueryFirstOrDefaultAsync<SoucheResponse>("SELECT_SOUCHE_BY_ID", new { Souche = Request.Souche });
                if (oSouche == null)
                {
                    throw new ApiException("Souche de numerotation introuvable!");
                }

                #endregion Existence souche

                #region Existance affaire

                //affaire(si fournie)

                if (!string.IsNullOrWhiteSpace(Request.ReferenceAffaire))
                {
                    var oAffaire = await Repo.QueryFirstOrDefaultAsync<AffaireResponse>("SELECT_AFFAIRE_BY_ID", new { Affaire = Request.ReferenceAffaire });
                    if (oAffaire == null)
                    {
                        throw new ApiException("Affaire introuvable!");
                    }
                }

                #endregion Existance affaire

                #region Existance Representant

                //collaborateur(si fournie sinon récupérer depuis le client)
                if (Request.CodeRepresentant == 0)
                {
                    Request.CodeRepresentant = oClient.CodeCollaborateur;
                }
                if (Request.CodeRepresentant != 0)
                {
                    var oRepresentant = await Repo.QueryFirstOrDefaultAsync<CollaborateurResponse>("SELECT_COLLABORATEUR_BY_ID", new { Representant = Request.CodeRepresentant });
                    if (oRepresentant == null)
                    {
                        throw new ApiException("Representant introuvable!");
                    }
                    //else if (oRepresentant.EnSommeil == 1)
                    //{
                    //    throw new ApiException("Representant en sommeil!");
                    //}
                    else
                    {
                        Request.CodeRepresentant = oRepresentant.CodeCollaborateur;
                    }
                }

                #endregion Existance Representant

                #region Existance depot

                //dépôt(si fournie sinon récupérer depuis le client)

                if (Request.CodeDepot != 0)
                {
                    var oDepot = await Repo.QueryFirstOrDefaultAsync<DepotResponse>("SELECT_DEPOT_BY_ID", new { Depot = Request.CodeDepot });
                    if (oDepot == null)
                    {
                        throw new ApiException("Depot introuvable!");
                    }
                    else
                    {
                        Request.CodeDepot = oDepot.CodeDepot;
                    }
                }
                else
                {
                    
                    var oDepot = await Repo.QueryFirstOrDefaultAsync<DepotResponse>("SELECT_DEPOT_PRINCIPAL");
                    Request.CodeDepot = oDepot.CodeDepot;
                }

                #endregion Existance depot

                #region Existance Categorie tarifaire

                //Existance cattarif

                if (Request.CodeCategorieTarifaire != 0)
                {
                    var oCatTarif = await Repo.QueryFirstOrDefaultAsync<CatTarifResponse>("SELECT_CATTARIF_BY_ID", new { Indice = Request.CodeCategorieTarifaire });
                    if (oCatTarif == null)
                    {
                        throw new ApiException("Categorie tarifaire introuvable!");
                    }
                }
                else
                {
                    Request.CodeCategorieTarifaire = oClient.CodeTarif;
                }

                #endregion Existance Categorie tarifaire

                F_DOCENTETE oF_DOCENTETE = DocEntetMapper.Adapt(Request, 0, 0);
                oF_DOCENTETE.DO_Type = 0;
                oF_DOCENTETE.DO_DocType = 0;
                oF_DOCENTETE.CG_Num = oClient.CompteCollectif;
                oF_DOCENTETE.DO_Tarif=(short)oClient.CodeTarif;

                if (oF_DOCENTETE.DO_Tarif == 0) oF_DOCENTETE.DO_Tarif= 1;
               

                for (int i = 0; i < oF_DOCENTETE.F_DOCLIGNEs.Count; i++)
                {
                    var oLine = oF_DOCENTETE.F_DOCLIGNEs[i];

                    var oArticle = await Repo.QueryFirstOrDefaultAsync<ArticleResponse>("SELECT_ARTICLE_MIN_BY_ID", new { Reference = oLine.AR_Ref });

                    if (oArticle != null && oArticle.EnSommiel == 0)
                    {
                        var oStock = await Repo.QueryFirstOrDefaultAsync<ArticleParDepotResponse>("SELECT_ARTICLE_PAR_DEPOT_BY_ID", new { Reference = oLine.AR_Ref, CodeDepot = Request.CodeDepot });
                        //var oTaxe = await Repo.QueryFirstOrDefaultAsync<TaxeResponse>("SELECT_TAXE_ARTICLE", new { Reference = oLine.AR_Ref });

                        oLine.DL_PoidsNet = oArticle.PoidsNet * oLine.DL_Qte;
                        oLine.DL_PoidsBrut = oArticle.PoidsBrut * oLine.DL_Qte;
                        oLine.EU_Enumere = oArticle.Unite;

                        //if (oStock?.QuantiteReel != 0) oLine.DL_PrixRU = oStock.Montant / oStock.QuantiteReel;
                        if (oStock != null && oStock.QuantiteReel != 0) oLine.DL_CMUP = oStock.Montant / oStock.QuantiteReel;
                        //if (oTaxe != null) oLine.DL_Taxe1 = oTaxe.Taux;
                        //if (oTaxe != null) oLine.DL_CodeTaxe1 = oTaxe.CodeTaxe;
                        oLine.DL_Taxe1 = 20;
                        oLine.DL_CodeTaxe1 = "02";
                    }
                    else if (oArticle == null)
                    {
                        Result.Errors.Add($"Article {oLine.AR_Ref } est inexistent");
                    }
                    else
                    {
                        Result.Errors.Add($"Article {oLine.AR_Ref } est  en sommiel");
                    }
                }

                if (Result.Errors.Count > 0)
                {
                    Result.Succeeded=false;
                    return Result;
                }

                //  DocEntetMapper.Adapt(Request);
                oF_DOCENTETE.Calculate();



                // insertion to F_Docentete
                using var insertRepo = _QueryService.NewRepository(Request.ConnectionName) ;
               
                using (var dbContextTransaction = insertRepo.BeginTransaction())
                {
                    try
                    {
                        oF_DOCENTETE.DO_Piece =await insertRepo.ExecuteScalarAsync<string>("SELECT_F_DOCCURRENTPIECE",
                            new
                            {
                                DC_Souche = oF_DOCENTETE.DO_Souche,
                                DC_Domaine = oF_DOCENTETE.DO_Domaine,
                                DC_IdCol = oF_DOCENTETE.DO_Type
                            });

                        await insertRepo.QueryAsync<int>("INSERT_F_DOCENTETE", oF_DOCENTETE);

                        F_DOCREGLE f_DOCREGLE = new F_DOCREGLE(oF_DOCENTETE);
                        await insertRepo.QueryAsync<int>("INSERT_F_DOCREGL", f_DOCREGLE);




                        // insertion to F_DOCLIGNE
                        var oDlNo = await insertRepo.ExecuteScalarAsync<int>("SELECT_F_DOCLIGNE_MAX_DL_NO");
                        foreach (var oLine in oF_DOCENTETE.F_DOCLIGNEs)
                        {
                            oLine.DL_No = oDlNo;
                            oLine.DO_Piece=oF_DOCENTETE.DO_Piece;
                            oLine.cbDE_No =  oLine.DE_No=oF_DOCENTETE.DE_No;
                            await insertRepo.QueryAsync<int>("INSERT_F_DOCLIGNE", oLine);
                            oDlNo++;
                        }
                        
                        // update numero de piece
                        await insertRepo.QueryAsync<int>("UPDATE_F_DOCCURRENTPIECE",
                           new
                           {
                               DC_Piece = oF_DOCENTETE.DO_Piece.Increment(),
                               DC_Souche = oF_DOCENTETE.DO_Souche,
                               DC_Domaine = oF_DOCENTETE.DO_Domaine,
                               DC_IdCol = oF_DOCENTETE.DO_Type
                           });
                        dbContextTransaction.Commit();
                        Result.Data = new DocumentResponse() { Piece = oF_DOCENTETE.DO_Piece};
                        return Result;
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
                Log.Fatal(ex, " Insert into commande vente societe {0}  error : {1}", Request.ConnectionName, ex.ToString());
                return await Result<DocumentResponse>.FailAsync(ex);
            }
        }



        public async Task<IResult<DocumentResponse>> CreateCommande(DocEnteteRequest Request)
        {
            var Result = new Result<DocumentResponse>();

            try
            {
                using var db = _QueryService.NewDbConnection(Request.ConnectionName);

                using Repositories.SageRepository Repo = _QueryService.NewRepository(Request.ConnectionName);

                #region Existence client

                //Existence client

                var oClient = await Repo.QueryFirstOrDefaultAsync<ClientResponse>("SELECT_CLIENT_BYID", new { CodeClient = Request.CodeTiers });
                if (oClient == null)
                {
                    throw new ApiException("Client introuvable!");
                }
                if (oClient.EnSommeil == 1)
                {
                    throw new ApiException("Client en sommeil!");
                }

                #endregion Existence client

                #region Existence souche

                //souche(si fournie vérifier l'existence sinon récupérer celle par default)

                var oSouche = await Repo.QueryFirstOrDefaultAsync<SoucheResponse>("SELECT_SOUCHE_BY_ID", new { Souche = Request.Souche });
                if (oSouche == null)
                {
                    throw new ApiException("Souche de numerotation introuvable!");
                }

                #endregion Existence souche

                #region Existance affaire

                //affaire(si fournie)

                if (!string.IsNullOrWhiteSpace(Request.ReferenceAffaire))
                {
                    var oAffaire = await Repo.QueryFirstOrDefaultAsync<AffaireResponse>("SELECT_AFFAIRE_BY_ID", new { Affaire = Request.ReferenceAffaire });
                    if (oAffaire == null)
                    {
                        throw new ApiException("Affaire introuvable!");
                    }
                }

                #endregion Existance affaire

                #region Existance Representant

                //collaborateur(si fournie sinon récupérer depuis le client)
                if (Request.CodeRepresentant == 0)
                {
                    Request.CodeRepresentant = oClient.CodeCollaborateur;
                }
                if (Request.CodeRepresentant != 0)
                {
                    var oRepresentant = await Repo.QueryFirstOrDefaultAsync<CollaborateurResponse>("SELECT_COLLABORATEUR_BY_ID", new { Representant = Request.CodeRepresentant });
                    if (oRepresentant == null)
                    {
                        throw new ApiException("Representant introuvable!");
                    }
                    //else if (oRepresentant.EnSommeil == 1)
                    //{
                    //    throw new ApiException("Representant en sommeil!");
                    //}
                    else
                    {
                        Request.CodeRepresentant = oRepresentant.CodeCollaborateur;
                    }
                }

                #endregion Existance Representant

                #region Existance depot

                //dépôt(si fournie sinon récupérer depuis le client)

                if (Request.CodeDepot != 0)
                {
                    var oDepot = await Repo.QueryFirstOrDefaultAsync<DepotResponse>("SELECT_DEPOT_BY_ID", new { Depot = Request.CodeDepot });
                    if (oDepot == null)
                    {
                        throw new ApiException("Depot introuvable!");
                    }
                    else
                    {
                        Request.CodeDepot = oDepot.CodeDepot;
                    }
                }
                else
                {
                    
                    var oDepot = await Repo.QueryFirstOrDefaultAsync<DepotResponse>("SELECT_DEPOT_PRINCIPAL");
                    Request.CodeDepot = oDepot.CodeDepot;
                }

                #endregion Existance depot

                #region Existance Categorie tarifaire

                //Existance cattarif

                if (Request.CodeCategorieTarifaire != 0)
                {
                    var oCatTarif = await Repo.QueryFirstOrDefaultAsync<CatTarifResponse>("SELECT_CATTARIF_BY_ID", new { Indice = Request.CodeCategorieTarifaire });
                    if (oCatTarif == null)
                    {
                        throw new ApiException("Categorie tarifaire introuvable!");
                    }
                }
                else
                {
                    Request.CodeCategorieTarifaire = oClient.CodeTarif;
                }

                #endregion Existance Categorie tarifaire

                F_DOCENTETE oF_DOCENTETE = DocEntetMapper.Adapt(Request, 0, 1);
                oF_DOCENTETE.DO_DocType = 1;
                oF_DOCENTETE.DO_Type = 1;
                oF_DOCENTETE.CG_Num = oClient.CompteCollectif;
                oF_DOCENTETE.DO_Tarif=(short)oClient.CodeTarif;

                if (oF_DOCENTETE.DO_Tarif == 0) oF_DOCENTETE.DO_Tarif= 1;
               

                for (int i = 0; i < oF_DOCENTETE.F_DOCLIGNEs.Count; i++)
                {
                    var oLine = oF_DOCENTETE.F_DOCLIGNEs[i];

                    var oArticle = await Repo.QueryFirstOrDefaultAsync<ArticleResponse>("SELECT_ARTICLE_MIN_BY_ID", new { Reference = oLine.AR_Ref });

                    if (oArticle != null && oArticle.EnSommiel == 0)
                    {
                        var oStock = await Repo.QueryFirstOrDefaultAsync<ArticleParDepotResponse>("SELECT_ARTICLE_PAR_DEPOT_BY_ID", new { Reference = oLine.AR_Ref, CodeDepot = Request.CodeDepot });
                        //var oTaxe = await Repo.QueryFirstOrDefaultAsync<TaxeResponse>("SELECT_TAXE_ARTICLE", new { Reference = oLine.AR_Ref });

                        oLine.DL_PoidsNet = oArticle.PoidsNet * oLine.DL_Qte;
                        oLine.DL_PoidsBrut = oArticle.PoidsBrut * oLine.DL_Qte;
                        oLine.EU_Enumere = oArticle.Unite;

                        //if (oStock?.QuantiteReel != 0) oLine.DL_PrixRU = oStock.Montant / oStock.QuantiteReel;
                        if (oStock != null && oStock.QuantiteReel != 0) oLine.DL_CMUP = oStock.Montant / oStock.QuantiteReel;
                        //if (oTaxe != null) oLine.DL_Taxe1 = oTaxe.Taux;
                        //if (oTaxe != null) oLine.DL_CodeTaxe1 = oTaxe.CodeTaxe;
                        oLine.DL_Taxe1 = 20;
                        oLine.DL_CodeTaxe1 = "02";
                    }
                    else if (oArticle == null)
                    {
                        Result.Errors.Add($"Article {oLine.AR_Ref } est inexistent");
                    }
                    else
                    {
                        Result.Errors.Add($"Article {oLine.AR_Ref } est  en sommiel");
                    }
                }

                if (Result.Errors.Count > 0)
                {
                    Result.Succeeded=false;
                    return Result;
                }

                //  DocEntetMapper.Adapt(Request);
                oF_DOCENTETE.Calculate();



                // insertion to F_Docentete
                using var insertRepo = _QueryService.NewRepository(Request.ConnectionName) ;
               
                using (var dbContextTransaction = insertRepo.BeginTransaction())
                {
                    try
                    {
                        oF_DOCENTETE.DO_Piece =await insertRepo.ExecuteScalarAsync<string>("SELECT_F_DOCCURRENTPIECE",
                            new
                            {
                                DC_Souche = oF_DOCENTETE.DO_Souche,
                                DC_Domaine = oF_DOCENTETE.DO_Domaine,
                                DC_IdCol = oF_DOCENTETE.DO_Type
                            });

                        await insertRepo.QueryAsync<int>("INSERT_F_DOCENTETE", oF_DOCENTETE);

                        F_DOCREGLE f_DOCREGLE = new F_DOCREGLE(oF_DOCENTETE);
                        await insertRepo.QueryAsync<int>("INSERT_F_DOCREGL", f_DOCREGLE);




                        // insertion to F_DOCLIGNE
                        var oDlNo = await insertRepo.ExecuteScalarAsync<int>("SELECT_F_DOCLIGNE_MAX_DL_NO");
                        foreach (var oLine in oF_DOCENTETE.F_DOCLIGNEs)
                        {
                            oLine.DL_No = oDlNo;
                            oLine.DO_Piece=oF_DOCENTETE.DO_Piece;
                            oLine.cbDE_No =  oLine.DE_No=oF_DOCENTETE.DE_No;
                            await insertRepo.QueryAsync<int>("INSERT_F_DOCLIGNE", oLine);
                            oDlNo++;
                        }
                        
                        // update numero de piece
                        await insertRepo.QueryAsync<int>("UPDATE_F_DOCCURRENTPIECE",
                           new
                           {
                               DC_Piece = oF_DOCENTETE.DO_Piece.Increment(),
                               DC_Souche = oF_DOCENTETE.DO_Souche,
                               DC_Domaine = oF_DOCENTETE.DO_Domaine,
                               DC_IdCol = oF_DOCENTETE.DO_Type
                           });
                        dbContextTransaction.Commit();
                        Result.Data = new DocumentResponse() { Piece = oF_DOCENTETE.DO_Piece};
                        return Result;
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
                Log.Fatal(ex, " Insert into commande vente societe {0}  error : {1}", Request.ConnectionName, ex.ToString());
                return await Result<DocumentResponse>.FailAsync(ex);
            }
        }
    }
}