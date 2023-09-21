using Dapper;
using Grs.Sage.ObjetMetiers;
using Grs.Sage.Wms.Api.Services;
using Objets100cLib;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
	
    public interface IDocEnteteService
	{
	    Task<Result<List<DocEnteteResponse>>> GetDocEntete(string pConnexionName);
        bool TransformerBl(ComHeaderRequest cm);
        Task<Result<bool>> Reception(ComHeaderRequest cm);

    }
   
    public class DocEnteteService : IDocEnteteService
    {
        private readonly IQueryService _QueryService;
        private static BSCIALApplication100c oCial;
        private static string sPathGcm = @"C:\Users\Public\Documents\Sage\Entreprise 100c\Bijou.gcm";
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
        public static bool TransformationBL(Grs.Sage.ObjetMetiers.DocumentVente Commande)
        {
            try
            {

                //DocumentVente Commande = new DocumentVente();

                Commande.DocType = 12;
                Commande.Depot = "Bijou SA";
                Commande.IDDepot = 1;
                Commande.NumClient = "BILLO";
                Commande.Client = "Billot";
                Commande.DocDate = Convert.ToDateTime("2023-08-01T10:21:47.843Z");
                Commande.NumPiece = "FBC00016";
                Commande.RefPiece = "string";
                Commande.DocStatut = "Saisi";
                Commande.Imprime = 1;
                Commande.Reliquat = 1;
                Commande.Adrs = "string";
                Commande.LgDocument.Add(new Grs.Sage.ObjetMetiers.LigneDocument
                {
                    Refrence = "LOT",
                    Designation = "LOT",
                    Qte = 2,
                    lot = "L3",
                    DatePeremption = Convert.ToDateTime("2026-08-01T10:21:47.843Z"),
                    NumLigne = 1000,
                });
                Commande.LgDocument.Add(new Grs.Sage.ObjetMetiers.LigneDocument
                {
                    Refrence = "LOT",
                    Designation = "LOT",
                    Qte = 1,
                    lot = "L4",
                    DatePeremption = Convert.ToDateTime("2025-08-01T10:21:47.843Z"),
                    NumLigne = 1000,
                });
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création du processus Réceptionner
                    IPMDocTransformer pTransfo = oCial.Transformation.Achat.CreateProcess_Receptionner();
                    //Description(de);
                    // Si le bon de commande FBC00006 existe
                    if (oCial.FactoryDocumentAchat.ExistPiece(DocumentType.DocumentTypeAchatCommandeConf, Commande.NumPiece))
                    {
                        // Sélection du bon de commande FBC00006
                        IBODocumentAchat3 pDoc = oCial.FactoryDocumentAchat.ReadPiece(DocumentType.DocumentTypeAchatCommandeConf, Commande.NumPiece);
                        // Ajout du document au processus
                        pTransfo.AddDocument(pDoc);
                        // Initialisation des numéros série/lot à créer
                        foreach (var item in Commande.LgDocument)
                        {
                            foreach (IBODocumentAchatLigne3 pLig in pTransfo.ListLignesATransformer)
                            {
                                pLig.DL_QteBC = Convert.ToDouble(item.Qte);
                                if (item.Refrence == pLig.Article.AR_Ref)
                                {
                                    // Si le suivi de l'article est Série/Lot
                                    if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeSerie || pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                                    {
                                        // Récupération du dépôt de la ligne
                                        IBODepot3 pDepot = pLig.Depot;
                                        // Récupération du dépôt de stockage de l'article
                                        IBOArticleDepot3 pArtDepot = pLig.Article.FactoryArticleDepot.ReadDepot(pDepot);
                                        // Tant que des série/lot doivent être fournis
                                        //if (pTransfo.UserLotsQteRestantAFournir[pLig] > 0)
                                        //{
                                        // Création d'un Lot/Série pour l'article
                                        IBOArticleDepotLot pArtDepotLot = (IBOArticleDepotLot)pArtDepot.FactoryArticleDepotLot.Create();
                                        // Création d'un lot/série pour la ligne
                                        IUserLot pUserLot = pTransfo.UserLotsToUse[pLig].AddNew(); /* TODO ERROR: Skipped SkippedTokensTrivia *//* TODO ERROR: Skipped SkippedTokensTrivia */

                                        if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                                        {

                                            pArtDepotLot.NoSerie = item.lot;
                                            pArtDepotLot.DatePeremption = item.DatePeremption.GetValueOrDefault();
                                            // Ajout du lot au processus pour toute la quantité nécessaire
                                            //pUserLot.Set(pArtDepotLot, pTransfo.UserLotsQteRestantAFournir[pLig], pArtDepotLot.Complement);
                                            pUserLot.Set(pArtDepotLot, Convert.ToDouble(item.Qte), pArtDepotLot.Complement);


                                        }
                                        //else
                                        //{
                                        //    // Affectation du numéro de série
                                        //    pArtDepotLot.NoSerie = myLine.lot;
                                        //    // Ajout du numéro de série au processus
                                        //    pUserLot.Set(pArtDepotLot, 1, pArtDepotLot.Complement);
                                        //    // Incrémentation du numéro de série
                                        //    //sNumSerie = pArtDepotLot.FactoryArticleDepotLot.NextNoSerie[sNumSerie];
                                        //}
                                        //}


                                    }

                                }
                            }
                        }

                        // Test pour savoir si le processus peut être validé
                        if (pTransfo.CanProcess)
                        {
                            // Validation du processus
                            pTransfo.Process();
                            return true;
                        }
                        else
                        {
                            // Traitement de récupération des erreurs
                            RecupError((IPMProcess)pTransfo);
                            return false;
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);

            }
        }
        public bool TransformerBl(ComHeaderRequest cm)
        {
            try
            {
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création d'un objet processus "Création de document"
                    IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeAchatLivraison);
                    ;
                    /* Cannot convert EmptyStatementSyntax, CONVERSION ERROR: Conversion for EmptyStatement not implemented, please report this issue in '' at character 350


                                    Input:
                                     'Conversion du document du processus (IBODocument3) dans le type du document de destination : Facture de vente
                                    (IBODocumentVente3)

                                     */
                    IBODocumentAchat3 mDoc = (IBODocumentAchat3)mProcessDoc.Document;
                    // Indique au document qu’il ne doit pas recalculer les totaus automatiquement à chaque modification ou ajout de lignes
                    mDoc.SetAutoRecalculTotaux(false);/* TODO ERROR: Skipped SkippedTokensTrivia
;*/

                    // Affectation du client au document
                    // Ajout d'une ligne sur l'article ENSHF de nomenclature commerciale/composé et
                    // conversion dans le type de ligne de document de destination (IBODocumentVenteLigne3).
                    // Lors de l'ajout de cette ligne, les autres lignes composant la nomenclature sont également ajoutées
                    mDoc.SetDefaultFournisseur(oCial.CptaApplication.FactoryFournisseur.ReadNumero(mDoc.Fournisseur.CT_Num));

                    // Parcours de toutes les lignes du document
                    foreach (var item in cm.ComLine)
                    {
                        var art = oCial.FactoryArticle.ReadReference(item.ProductId);

                        IBODocumentAchatLigne3 mLig = (IBODocumentAchatLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.ProductId), Convert.ToDouble(item.QuantiteScane));
                        if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            mLig.LS_NoSerie = item.StockId;
                            mLig.LS_Peremption = item.DatePeremption;
                        }

                    }
                    //foreach (IBODocumentAchatLigne3 mLig in mDoc.FactoryDocumentLigne.List)
                    // Application de la remise par défaut pour chacune des lignes
                    //mLig.SetDefaultRemise();

                    // Si le document est cohérent et peut être écrit en base
                    if (!mProcessDoc.CanProcess)
                    {
                        // Récupération des erreurs
                        RecupError(mProcessDoc);
                    }
                    else
                    {
                        // Gérération de document dans la base
                        mProcessDoc.Process();

                       
                    }





                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);

            }

        }


        public async Task<Result<bool>> Reception(ComHeaderRequest cm)
        {
            if (cm.LaisseBC == false)
            {
               var Result = TarnsformerBL_Achat(cm);
                
               return await Result<bool>.SuccessAsync(Result);
              
            }
            else
            {
                var Result =  CreationBL_Achat(cm);
                return await Result<bool>.SuccessAsync(Result);
            }
            
        }

        public static bool TarnsformerBL_Achat(ComHeaderRequest cm)
        {
            try
            {

                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création du processus Réceptionner
                    IPMDocTransformer pTransfo = oCial.Transformation.Achat.CreateProcess_Receptionner();
                    //Description(de);
                    // Si le bon de commande FBC00006 existe
                    if (oCial.FactoryDocumentAchat.ExistPiece(DocumentType.DocumentTypeAchatCommandeConf, cm.ComHeaderId))
                    {
                        // Sélection du bon de commande FBC00006
                        IBODocumentAchat3 pDoc = oCial.FactoryDocumentAchat.ReadPiece(DocumentType.DocumentTypeAchatCommandeConf, cm.ComHeaderId);
                        // Ajout du document au processus
                        pTransfo.AddDocument(pDoc);
                        // Initialisation des numéros série/lot à créer
                        foreach (var item in cm.ComLine)
                        {
                            foreach (IBODocumentAchatLigne3 pLig in pTransfo.ListLignesATransformer)
                            {

                                pLig.DL_QteBC = Convert.ToDouble(item.QuantiteScane);
                                if (item.ProductId == pLig.Article.AR_Ref && Convert.ToDouble(item.PrixUnitaire) == pLig.DL_PrixUnitaire)
                                {
                                    // Si le suivi de l'article est Série/Lot
                                    if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeSerie || pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                                    {
                                        // Récupération du dépôt de la ligne
                                        IBODepot3 pDepot = pLig.Depot;
                                        // Récupération du dépôt de stockage de l'article
                                        IBOArticleDepot3 pArtDepot = pLig.Article.FactoryArticleDepot.ReadDepot(pDepot);
                                        // Tant que des série/lot doivent être fournis
                                        //if (pTransfo.UserLotsQteRestantAFournir[pLig] > 0)
                                        //{
                                        // Création d'un Lot/Série pour l'article
                                        IBOArticleDepotLot pArtDepotLot = (IBOArticleDepotLot)pArtDepot.FactoryArticleDepotLot.Create();
                                        // Création d'un lot/série pour la ligne
                                        IUserLot pUserLot = pTransfo.UserLotsToUse[pLig].AddNew(); /* TODO ERROR: Skipped SkippedTokensTrivia *//* TODO ERROR: Skipped SkippedTokensTrivia */

                                        if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                                        {

                                            pArtDepotLot.NoSerie = item.StockId;
                                            pArtDepotLot.DatePeremption = item.DatePeremption;
                                            // Ajout du lot au processus pour toute la quantité nécessaire
                                            //pUserLot.Set(pArtDepotLot, pTransfo.UserLotsQteRestantAFournir[pLig], pArtDepotLot.Complement);
                                            pUserLot.Set(pArtDepotLot, Convert.ToDouble(item.QuantiteScane), pArtDepotLot.Complement);


                                        }
                                        //else
                                        //{
                                        //    // Affectation du numéro de série
                                        //    pArtDepotLot.NoSerie = myLine.lot;
                                        //    // Ajout du numéro de série au processus
                                        //    pUserLot.Set(pArtDepotLot, 1, pArtDepotLot.Complement);
                                        //    // Incrémentation du numéro de série
                                        //    //sNumSerie = pArtDepotLot.FactoryArticleDepotLot.NextNoSerie[sNumSerie];
                                        //}
                                        //}


                                    }

                                }
                            }
                        }

                        // Test pour savoir si le processus peut être validé
                        if (pTransfo.CanProcess)
                        {
                            // Validation du processus
                            pTransfo.Process();
                            return true;
                        }
                        else
                        {
                            // Traitement de récupération des erreurs
                            RecupError((IPMProcess)pTransfo);
                            return false;
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);

            }
        }

        public static bool CreationBL_Achat(ComHeaderRequest cm)
        {
            try
            {
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création d'un objet processus "Création de document"
                    IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeAchatLivraison);
                    ;
                    /* Cannot convert EmptyStatementSyntax, CONVERSION ERROR: Conversion for EmptyStatement not implemented, please report this issue in '' at character 350


                                    Input:
                                     'Conversion du document du processus (IBODocument3) dans le type du document de destination : Facture de vente
                                    (IBODocumentVente3)

                                     */
                    IBODocumentAchat3 mDoc = (IBODocumentAchat3)mProcessDoc.Document;
                    // Indique au document qu’il ne doit pas recalculer les totaus automatiquement à chaque modification ou ajout de lignes
                    mDoc.SetAutoRecalculTotaux(false);/* TODO ERROR: Skipped SkippedTokensTrivia
;*/

                    // Affectation du client au document
                    // Ajout d'une ligne sur l'article ENSHF de nomenclature commerciale/composé et
                    // conversion dans le type de ligne de document de destination (IBODocumentVenteLigne3).
                    // Lors de l'ajout de cette ligne, les autres lignes composant la nomenclature sont également ajoutées
                    mDoc.SetDefaultFournisseur(oCial.CptaApplication.FactoryFournisseur.ReadNumero(cm.ThirdParty));

                    // Parcours de toutes les lignes du document
                    foreach (var item in cm.ComLine)
                    {
                        var art = oCial.FactoryArticle.ReadReference(item.ProductId);

                        IBODocumentAchatLigne3 mLig = (IBODocumentAchatLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.ProductId), Convert.ToDouble(item.QuantiteScane));
                        if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            mLig.LS_NoSerie = item.StockId;
                            mLig.LS_Peremption = item.DatePeremption;
                        }

                    }
                    //foreach (IBODocumentAchatLigne3 mLig in mDoc.FactoryDocumentLigne.List)
                    // Application de la remise par défaut pour chacune des lignes
                    //mLig.SetDefaultRemise();

                    // Si le document est cohérent et peut être écrit en base
                    if (!mProcessDoc.CanProcess)
                    {
                        // Récupération des erreurs
                        RecupError(mProcessDoc);
                    }
                    else
                    {
                        // Gérération de document dans la base
                        mProcessDoc.Process();

                        /* Cannot convert EmptyStatementSyntax, CONVERSION ERROR: Conversion for EmptyStatement not implemented, please report this issue in '' at character 1713


                                            Input:
                                            © 2022 Sage 148

                                             */
                    }





                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return false;
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);

            }
        }




        public static bool OpenBase(ref BSCIALApplication100c BaseCial, string sGcm, string sUid = "<Administrateur>", string sPwd = "")
        {
            try
            {
                // Affectation de l'emplacement du fichier commercial 
                BaseCial.Name = sGcm;
                // MsgBox(sGcm)
                // Affectation du code utilisateur 
                BaseCial.Loggable.UserName = sUid;
                // Affectation du mot de passe 
                BaseCial.Loggable.UserPwd = sPwd;
                // Ouverture de la base commerciale 
                BaseCial.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }
        public static void RecupError(IPMProcess mP)
        {

            // Boucle sur les erreurs contenues dans la collection 
            string msg = "";
            for (int i = 1; i <= mP.Errors.Count; i++)
            {
                // Récupération des éléments erreurs 
                IFailInfo iFail = mP.Errors[i];
                // récupération du numéro d'erreur,
                // de l'indice et de la description de l'erreur 
                msg += "Code Erreur : " + iFail.ErrorCode + " Indice : " + iFail.Indice + " Description : " + iFail.Text;
            }
            if (msg.Length > 0)
                throw new ApplicationException(msg);
        }
        private static bool CloseBase(ref BSCIALApplication100c bCial)
        {
            try
            {
                if (bCial.IsOpen)
                    bCial.Close();
                return true;


            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la fermeture de la base : " + ex.Message);
                return false;
            }
        }

    }

}
