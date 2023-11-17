using Dapper;

using Grs.Sage.Wms.Api.Services;
using Objets100cLib;
using Serilog;
using System;
using System.Collections;
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
        Task<Result<string>> Reception(ComHeaderRequest cm);
        Task<Result<string>> Expedition(ComHeaderRequest cm);

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


        public async Task<Result<string>> Reception(ComHeaderRequest cm)
        {
            if (cm.LaisseBC == false)
            {
                var Result = TarnsformerBL_Achat(cm);
                if (Result == "Succées")
                {
                    return await Result<string>.SuccessAsync(data: Result);
                }
                else
                {
                    return await Result<string>.FailAsync(Result);
                }

            }
            else
            {
                var Result =   CreationBL_Achat(cm);
                if (Result == "Succées")
                {
                    return await Result<string>.SuccessAsync( data: Result);
                }
                else
                {
                    return await Result<string>.FailAsync(Result);
                }
               
            }
            
        }
        public async Task<Result<string>> Expedition(ComHeaderRequest cm)
        {
            if (cm.LaisseBC == false)
            {
                var Result = TransformationBL_VENTE(cm);
                if (Result == "Succées")
                {
                    return await Result<string>.SuccessAsync(data: Result);
                }
                else
                {
                    return await Result<string>.FailAsync(Result);
                }

            }
            else
            {
                var Result = CreationBL_Vente(cm);
                if (Result == "Succées")
                {
                    return await Result<string>.SuccessAsync(data: Result);
                }
                else
                {
                    return await Result<string>.FailAsync(Result);
                }

                
            }

        }

        public static string TarnsformerBL_Achat(ComHeaderRequest cm)
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

                              
                                if (item.ProductId == pLig.Article.AR_Ref && Convert.ToDouble(item.PrixUnitaire) == pLig.DL_PrixUnitaire)
                                {
                                    pLig.DL_QteBL = Convert.ToDouble(item.QuantiteScane);
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
                                    
                                    }

                                }
                            }
                        }

                        // Test pour savoir si le processus peut être validé
                        if (pTransfo.CanProcess)
                        {
                            // Validation du processus
                            pTransfo.Process();
                        }
                        else
                        {
                            // Traitement de récupération des erreurs
                            var msg= RecupError((IPMProcess)pTransfo);
                            return msg;
                        }

                    }
                    else
                    {
                        var msg = "Le Bon de commande n'existe pas!!";
                        return msg;
                    }
                }
                return "Succeés";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return ex.Message;
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);

            }
        }
        public static string TransformationBL_VENTE(ComHeaderRequest cm)
        {
            try
            {


                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création du processus Livrer
                    IPMDocTransformer pTransfo = oCial.Transformation.Vente.CreateProcess_Livrer();
                    // Si le devis DE00002 existe
                    if (oCial.FactoryDocumentVente.ExistPiece(DocumentType.DocumentTypeVenteCommande, cm.ComHeaderId))
                    {
                        // Sélection du devis DE00002
                        IBODocumentVente3 pDoc = oCial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, cm.ComHeaderId);

                        // Ajout du document au processus
                        pTransfo.AddDocument(pDoc);
                        // Création d'une table permettant de stocker
                        // les lots affectés au processus
                        var hashTb = new Hashtable();
                        // Si le document contient des lignes

                        if (pTransfo.ListLignesATransformer.Count > 0)
                        {
                            foreach (var item in cm.ComLine)
                            {
                                foreach (IBODocumentVenteLigne3 pLig in pTransfo.ListLignesATransformer)
                                {
                                   
                                    if (item.ProductId == pLig.Article.AR_Ref)
                                    {
                                        pLig.DL_QteBL = Convert.ToDouble(item.QuantiteScane);
                                        // Test sur le suivi de stock de l'article
                                        if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeSerie)
                                        {
                                            // Affectation des numéros de série
                                            SetSerie(pTransfo, pLig);
                                        }
                                        else if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                                        {


                                            SetLot(pTransfo, pLig, hashTb, item);
                                        }
                                        //else
                                        //{
                                        //    int i = 0;
                                        //    int ind = 0;
                                        //    foreach(IBODocumentVenteLigne3 a in pTransfo.ListLignesATransformer)
                                        //    {
                                        //        if(item.ProductId == a.Article.AR_Ref)
                                        //        {
                                        //            ind = i;
                                        //        }
                                        //        else
                                        //        {
                                        //            i++;
                                        //        }
                                        //    }
                                        //    pTransfo.ListLignesATransformer[ind] = Convert.ToDouble(item.QuantiteScane);

                                        //}
                                        //else
                                        //{
                                        //    pTransfo.AddDocumentLigne(pLig);
                                        //}
                                    }

                                }
                            }
                            foreach (IBODocumentVenteLigne3 item in pTransfo.ListLignesATransformer)
                            {
                                var IsExistLine = cm.ComLine.FirstOrDefault(x => x.ProductId == item.Article.AR_Ref);
                                if (IsExistLine == null)
                                {
                                    item.DL_QteBL =0;
                                }
                            }
                            // Parcours de lignes de document présentes dans le processus

                        }
                        // Test pour savoir si le processus peut être validé
                        if (pTransfo.CanProcess)
                        {
                            // Validation du processus
                            pTransfo.Process();
                        }
                        else
                        {
                            // Traitement de récupération des erreurs
                            var msg = RecupError((IPMProcess)pTransfo);
                            return msg;
                        }
                    }
                    else
                    {
                        var msg = "Le bon de commande n'existe pas!!";
                        return msg;
                    }

                }
                return "Succées";
            }
            catch (Exception ex)
            {

                Console.WriteLine("Erreur : " + ex.Message);
                return ex.Message;
            }
            finally
            {

                // Fermeture de la connexion
                CloseBase(ref oCial);
            }

        }

        public static string CreationBL_Achat(ComHeaderRequest cm)
        {
            try
            {
                oCial = new BSCIALApplication100c();
                if (OpenBase(ref oCial, sPathGcm))
                {
                    IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeAchatLivraison);
                    IBODocumentAchat3 mDoc = (IBODocumentAchat3)mProcessDoc.Document;
                    
                    //IBODocumentAchat3 mDoc = oCial.FactoryDocumentAchat.CreateType(DocumentType.DocumentTypeAchatLivraison);
                    mDoc.SetAutoRecalculTotaux(false);
                    mDoc.SetDefaultFournisseur(oCial.CptaApplication.FactoryFournisseur.ReadNumero(cm.ThirdParty));

                    foreach (var item in cm.ComLine)
                    {
                        //IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeAchatLivraison);
                        //IBODocumentAchat3 mDoc = (IBODocumentAchat3)mProcessDoc.Document;
                      

                        var art = oCial.FactoryArticle.ReadReference(item.ProductId);

                        IBODocumentAchatLigne3 mLig = (IBODocumentAchatLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.ProductId), Convert.ToDouble(item.QuantiteScane));
                        if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            mLig.LS_NoSerie = item.StockId;
                            mLig.LS_Peremption = item.DatePeremption;
                        }
                        

                    }

                    if (!mProcessDoc.CanProcess)
                    {
                        var msg = RecupError(mProcessDoc);
                        return msg;
                    }
                    else
                    {
                        mProcessDoc.Process();

                    }
                }
                return "Succées";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return ex.Message;
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);

            }
        }
        public string CreationBL_Vente(ComHeaderRequest cm)
        {
            try
            {
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création d'un objet processus "Création de document"
                    //IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeVenteLivraison);
                    IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeVenteLivraison);
                    /* Cannot convert EmptyStatementSyntax, CONVERSION ERROR: Conversion for EmptyStatement not implemented, please report this issue in '' at character 350


                                    Input:
                                     'Conversion du document du processus (IBODocument3) dans le type du document de destination : Facture de vente
                                    (IBODocumentVente3)

                                     */
                    //IBODocumentVente3 mDoc = (IBODocumentVente3)mProcessDoc.Document;
                    // Indique au document qu’il ne doit pas recalculer les totaus automatiquement à chaque modification ou ajout de lignes

                    IBODocumentVente3 mDoc = (IBODocumentVente3)mProcessDoc.Document;
                    // Indique au document qu’il ne doit pas recalculer les totaus automatiquement à chaque modification ou ajout de lignes
                    mDoc.SetAutoRecalculTotaux(false);/* TODO ERROR: Skipped SkippedTokensTrivia
;*/
                    mDoc.SetDefaultClient(oCial.CptaApplication.FactoryClient.ReadNumero(cm.ThirdParty));

                    // Parcours de toutes les lignes du document
                    foreach (var item in cm.ComLine)
                    {
                        //IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeVenteLivraison);
                        //IBODocumentVente3 mDoc = (IBODocumentVente3)mProcessDoc.Document;
                        //mDoc.SetAutoRecalculTotaux(false);/* TODO ERROR: Skipped SkippedTokensTrivia;*/


                        //mDoc.SetDefaultClient(oCial.CptaApplication.FactoryClient.ReadNumero(cm.ThirdParty));
                        var art = oCial.FactoryArticle.ReadReference(item.ProductId);

                        IBODocumentVenteLigne3 mLig = (IBODocumentVenteLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.ProductId), Convert.ToDouble(item.QuantiteScane));
                        if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            mLig.LS_NoSerie = item.StockId;
                            mLig.LS_Peremption = item.DatePeremption;
                        }
                        


                    }
                    // Si le document est cohérent et peut être écrit en base
                    if (!mProcessDoc.CanProcess)
                    {
                        var Error = RecupError(mProcessDoc);


                    }
                    else
                    {
                        mProcessDoc.Process();
                        return "Succées";
                    }
                }
                return "Succées";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return ex.Message;
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
                throw ;
            }
        }
        public  static string RecupError(IPMProcess mP)
        {

            // Boucle sur les erreurs contenues dans la collection 
            string msg = "";
            for (int i = 1; i <= mP.Errors.Count; i++)
            {
                // Récupération des éléments erreurs 
                IFailInfo iFail = mP.Errors[i];
                // récupération du numéro d'erreur,
                // de l'indice et de la description de l'erreur 
                msg += "Code Erreur : " + iFail.ErrorCode + " Indice : " + iFail.Indice + " Description : " + iFail.Text + "./";
            }
            if (msg.Length > 0)
                return msg;
            else
            {
                return "";
            }
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
        private static void SetLot(IPMDocTransformer pTransfo, IBODocumentVenteLigne3 pLig, Hashtable hashTb, ComlineRequest Ligne)
        {
            try
            {
                bool bReadAllLot = false;


                // qu'il y a une quantité lot à fournir
                if (!bReadAllLot && pTransfo.UserLotsQteRestantAFournir[pLig] > 0)
                {
                    IBODepot3 pDepot = pLig.Depot;
                    IBOArticleDepot3 pArtDepot = pLig.Article.FactoryArticleDepot.ReadDepot(pDepot);
                    int i = 0;
                    // Parcours des numéros de lot pour l'article
                    foreach (IBOArticleDepotLot pArtDepotLot in pArtDepot.FactoryArticleDepotLot.List)
                    {
                        double dQteTb = 0;
                        double dQteFournir = Convert.ToDouble(Ligne.QuantiteScane);
                        if (pArtDepotLot.NoSerie == Ligne.StockId)
                        {
                            if (!pArtDepotLot.IsEpuised && pArtDepotLot.StockATerme() > 0)
                            {
                                // Création d'un objet lot
                                IUserLot pUserLot = pTransfo.UserLotsToUse[pLig].AddNew();
                                // Si le lot a déjà été affecté mais qu'il
                                // lui reste une quantité disponible
                                if (hashTb.ContainsKey(pArtDepotLot) && (double)hashTb[pArtDepotLot] > 0)
                                {
                                    // Récupération de la quantité disponible du lot
                                    dQteTb = (double)hashTb[pArtDepotLot];
                                    // Si la quantité à fournir est inférieur
                                    // à la quantité disponible du lot
                                    if (dQteFournir <= dQteTb)
                                    {
                                        // Affectation du lot
                                        pUserLot.Set(pArtDepotLot, dQteFournir, pArtDepotLot.Complement);
                                        // Décrémentation de la quantité
                                        // disponible du lot dans la table de hash
                                        hashTb[pArtDepotLot] = dQteTb - dQteFournir;
                                        // Sortie de la boucle car tous les
                                        // lots ont été affectés à la ligne
                                        break;
                                    }
                                    else
                                    {
                                        // Affectation de la quantité restante du lot
                                        pUserLot.Set(pArtDepotLot, dQteTb, pArtDepotLot.Complement);
                                        // Le lot passe à indisponible
                                        hashTb[pArtDepotLot] = 0;
                                    }
                                }
                                else
                                {

                                    // Si la quantité à fournir est inférieur à
                                    // la quantité disponible du lot
                                    if (dQteFournir <= pArtDepotLot.StockATerme())
                                    {
                                        // Affectation du lot
                                        pUserLot.Set(pArtDepotLot, dQteFournir, pArtDepotLot.Complement);
                                        // Ajout du lot dans la table de hash
                                        // avec décrémentation de la quantité disponible
                                        hashTb.Add(pArtDepotLot, pArtDepotLot.StockATerme() - dQteFournir);
                                        // Sortie de la boucle car tous les lots
                                        // ont été affectés à la ligne
                                        //pTransfo.AddDocumentLigne(pLig);
                                        break;
                                    }
                                    else
                                    {
                                        // Affectation de la quantité restante du lot
                                        pUserLot.Set(pArtDepotLot, pArtDepotLot.StockATerme(), pArtDepotLot.Complement);
                                        // Le lot passe à indisponible et est
                                        // ajouté dans la table de hash
                                        hashTb.Add(pArtDepotLot, 0);
                                    }
                                }
                                i += 1;
                                // Si tous les lots de l'article ont été lus
                                if (i == pArtDepot.FactoryArticleDepotLot.List.Count)
                                    bReadAllLot = true;
                            }
                        }

                        // Si le lot n'est pas épuisé

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void SetSerie(IPMDocTransformer pTransfo, IBODocumentVenteLigne3 pLig)
        {
            try
            {
                bool bReadAllSerie = false;
                // Tant qu'on a pas parcouru tous les série/lot et
                // qu'il y a une quantité série à fournir
                if (!bReadAllSerie && pTransfo.UserLotsQteRestantAFournir[pLig] > 0)
                {
                    IBODepot3 pDepot = pLig.Depot;
                    IBOArticleDepot3 pArtDepot = pLig.Article.FactoryArticleDepot.ReadDepot(pDepot);
                    // Parcours des numéros de série pour l'article
                    // et pour le dépôt de la ligne
                    foreach (IBOArticleDepotLot pArtDepotLot in pArtDepot.FactoryArticleDepotLot.List)
                    {
                        // Si le numéro n'est pas épuisé et s'il n'a pas déjà été affecté
                        if (!pArtDepotLot.IsEpuised & pArtDepotLot.StockATerme() > 0 & !SerieAlreadyUse(pTransfo, pLig, pArtDepotLot))
                        {
                            // Création d'un objet série
                            IUserLot pUserLot = pTransfo.UserLotsToUse[pLig].AddNew();
                            // Affectation du numéro de série
                            pUserLot.Set(pArtDepotLot, 1, pArtDepotLot.Complement);
                            bReadAllSerie = false;
                            break;
                        }
                        bReadAllSerie = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static bool SerieAlreadyUse(IPMDocTransformer pTransfo, IBODocumentVenteLigne3 pLig, IBOArticleDepotLot pLot)
        {
            bool bRes = false;
            try
            {
                // Parcours de toutes les lignes du processus
                foreach (IBODocumentVenteLigne3 pL in pTransfo.ListLignesATransformer)
                {
                    // Si l'article est identique et que des num�ros de
                    if ((!(pL.Article == null)
                                && (pL.Article.Equals(pLig.Article)
                                && (pTransfo.UserLotsToUse[pL].Count > 0))))
                    {
                        // Parcours des num�ros de s�rie affect�s � la ligne
                        for (int i = 1; (i <= pTransfo.UserLotsToUse[pL].Count); i++)
                        {
                            IUserLot pTmpUserLot = (IUserLot)pTransfo.UserLotsToUse[pL];
                            // Si le num�ro de s�rie que l'on souhaite
                            // affecter est d�j� associ� � une ligne
                            if (pTmpUserLot.Lot.Equals(pLot))
                            {
                                return true;
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bRes;
        }

    }

}
