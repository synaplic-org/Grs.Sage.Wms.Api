using Dapper;
using Grs.Sage.Wms.Api.Services;
using Objets100cLib;
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
        // Objet base commerciale
        private static BSCIALApplication100c oCial;
        // emplacement du fichier commercial
        private static string sPathGcm = @"C:\Temp\Bijou.gcm";
        public static void Main()
        {
            try
            {
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création du processus Réceptionner
                    IPMDocTransformer pTransfo = oCial.Transformation.Achat.CreateProcess_Receptionner();
                    //Description(de);
                    

                      // Si le bon de commande FBC00006 existe
                    if (oCial.FactoryDocumentAchat.ExistPiece(DocumentType.DocumentTypeAchatCommandeConf, "FBC00003"))
                    {
                        // Sélection du bon de commande FBC00006
                        IBODocumentAchat3 pDoc = oCial.FactoryDocumentAchat.ReadPiece(DocumentType.DocumentTypeAchatCommandeConf, "FBC00003");
                        
                        // Ajout du document au processus
                        pTransfo.AddDocument(pDoc);
                        // Initialisation des numéros série/lot à créer
                        string sNumLot = "LOT0000";
                        string sNumSerie = "SERIE0000";
                        // Parcours des lignes de document présentes dans le processus
                        foreach (IBODocumentAchatLigne3 pLig in pTransfo.ListLignesATransformer)
                        {
                            // Si le suivi de l'article est Série/Lot
                            if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot || pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeSerie)
                            {
                                // Récupération du dépôt de la ligne
                                IBODepot3 pDepot = pLig.Depot;
                                // Récupération du dépôt de stockage de l'article
                                IBOArticleDepot3 pArtDepot = pLig.Article.FactoryArticleDepot.ReadDepot(pDepot);
                                // Tant que des série/lot doivent être fournis
                                while (pTransfo.UserLotsQteRestantAFournir[pLig] > 0) 
                                {
                                    // Création d'un Lot/Série pour l'article
                                    IBOArticleDepotLot pArtDepotLot = (IBOArticleDepotLot)pArtDepot.FactoryArticleDepotLot.Create();
                                    // Création d'un lot/série pour la ligne
                                    IUserLot pUserLot = pTransfo.UserLotsToUse[pLig].AddNew();// Description; /* TODO ERROR: Skipped SkippedTokensTrivia *//* TODO ERROR: Skipped SkippedTokensTrivia */
                                    ;


                                    pTransfo.UserLotsToUse[pLig].AddNew();
                                    if (pLig.Article.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                                    {
                                        // Calcul d'un numéro de lot inexistant
                                        while (pArtDepotLot.FactoryArticleDepotLot.ExistNoSerie(sNumLot))
                                            sNumLot = pArtDepotLot.FactoryArticleDepotLot.NextNoSerie[sNumLot];
                                        // Affectation du numéro de lot
                                        pArtDepotLot.NoSerie = sNumLot;
                                        // Ajout du lot au processus pour toute la quantité nécessaire
                                        pUserLot.Set(pArtDepotLot, pTransfo.UserLotsQteRestantAFournir[pLig], pArtDepotLot.Complement);
                                        // Incrémentation du numéro de lot
                                        sNumLot = pArtDepotLot.FactoryArticleDepotLot.NextNoSerie[sNumLot];
                                    }
                                    else
                                    {
                                        // Calcul d'un numéro de série inexistant
                                        while (pArtDepotLot.FactoryArticleDepotLot.ExistNoSerie(sNumSerie))
                                            sNumSerie = pArtDepotLot.FactoryArticleDepotLot.NextNoSerie[sNumSerie];
                                        // Affectation du numéro de série
                                        pArtDepotLot.NoSerie = sNumSerie;
                                        // Ajout du numéro de série au processus
                                        pUserLot.Set(pArtDepotLot, 1, pArtDepotLot.Complement);
                                        // Incrémentation du numéro de série
                                        sNumSerie = pArtDepotLot.FactoryArticleDepotLot.NextNoSerie[sNumSerie];
                                    }
                                }
                            }

                            //Description(de);
                          
                        }
                        // Test pour savoir si le processus peut être validé
                        if (pTransfo.CanProcess)
                            // Validation du processus
                            pTransfo.Process();
                        else
                            // Traitement de récupération des erreurs 
                            RecupError(  (IPMEncoder)pTransfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            finally
            {
                // Fermeture de la connexion
                CloseBase(ref oCial);
            }
        }
        public string  TransformerBl(DocumentVente Commande)
        {
            var oNumPieceBl = "";
            Exception mEx = null;
            try
            {

                // MsgBox("Instanciation de l'objet base commercial ")
                BSCIALApplication100c oCial;
                // emplacement du fichier commercial 
                // Dim sPathGcm As String = "C:\Temp\Bijou.gcm"

                oCial = new BSCIALApplication100c();
                try
                {
                    // 

                    // MsgBox("Ouverture de la base ")
                    if (OpenBase(ref oCial, sPathGcm))
                    {
                        // Création du processus Commander 
                        IPMDocTransformer pTransfo = oCial.Transformation.Vente.CreateProcess_Livrer();
                        // Si le devis DE00036 existe 
                        if (oCial.FactoryDocumentVente.ExistPiece(DocumentType.DocumentTypeVenteCommande, Commande.NumPiece))
                        {
                            // Sélection du devis DE00036 
                            IBODocumentVente3 pDoc = oCial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, Commande.NumPiece);
                            // Si le document contient au moins une ligne 
                            if (pDoc.FactoryDocumentLigne.List.Count > 0)
                            {
                                // Sélection de la première ligne du devis 
                                if (Commande.LgDocument.Sum(o => o.Qte - o.QteALivre) != 0)
                                {
                                    foreach (IBODocumentVenteLigne3 Plign3 in pDoc.FactoryDocumentLigne.List)
                                    {
                                        if (Plign3.Article != null)
                                        {
                                            var sligne = Plign3;
                                            var oligne = Commande.LgDocument.FirstOrDefault(oi => oi.Refrence == sligne.Article.AR_Ref & oi.Qte == Convert.ToDecimal(sligne.DL_Qte) & oi.Designation == sligne.DL_Design);

                                            if (oligne.QteALivre == 0)
                                                continue;
                                            else if (oligne.Qte == oligne.QteALivre)
                                                pTransfo.AddDocumentLigne(sligne);
                                            else
                                            {
                                                sligne.DL_Qte = decimal.ToDouble(oligne.Qte.GetValueOrDefault() - oligne.QteALivre.GetValueOrDefault());
                                                IBODocumentVenteLigne3 onewligne = (IBODocumentVenteLigne3)pDoc.FactoryDocumentLigne.Create();
                                                {
                                                    var withBlock = onewligne;
                                                    withBlock.SetDefault();
                                                    withBlock.SetDefaultArticleReference(sligne.Article.AR_Ref, decimal.ToDouble(oligne.QteALivre.GetValueOrDefault()));
                                                    withBlock.DL_CMUP = sligne.DL_CMUP;
                                                    withBlock.DL_PrixUnitaire = sligne.DL_PrixUnitaire;
                                                    withBlock.DL_QteBL = decimal.ToDouble(oligne.QteALivre.GetValueOrDefault());
                                                    withBlock.Remise = sligne.Remise;
                                                    withBlock.AC_RefClient = sligne.AC_RefClient;
                                                    withBlock.AF_RefFourniss = sligne.AF_RefFourniss;
                                                    withBlock.CompteA = sligne.CompteA;
                                                    withBlock.Collaborateur = sligne.Collaborateur;
                                                    withBlock.DL_Design = sligne.DL_Design;
                                                    withBlock.DL_NoColis = sligne.DL_NoColis;
                                                    withBlock.DL_PrixRU = sligne.DL_PrixRU;
                                                    withBlock.Depot = sligne.Depot;
                                                    withBlock.Write();
                                                    sligne.Write();
                                                }
                                                pTransfo.AddDocumentLigne(onewligne);
                                            }
                                        }
                                    }
                                }
                                else
                                    pTransfo.AddDocument(pDoc);


                                // Dim pLig As IBODocumentVenteLigne3 = pDoc.FactoryDocumentLigne.List(1)

                                // Ajout de la ligne au processus 

                                // Test pour savoir si le processus peut être validé 
                                if (pTransfo.CanProcess)
                                {
                                    // MsgBox("Validation du processus ")

                                    pTransfo.Process();
                                    // Affichage du numéro de pièce du document créé 
                                    // par le processus de transformation 
                                    //oNumPieceBl = (IBODocumentVente3)pTransfo.ListDocumentsResult(1).DO_Piece;
                                }
                                else
                                    // MsgBox("Traitement de récupération des erreurs ")
                                    RecupError((IPMEncoder)pTransfo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    mEx = ex;
                }
                finally
                {
                    // MsgBox("Fermeture de la connexion ")
                    CloseBase(ref oCial);
                }
            }
            catch (Exception ex)
            {
                mEx = ex;
            }

            if (mEx != null)
                throw mEx;
            // MsgBox(oNumPieceBl)
            return oNumPieceBl;
        }

        //public static string TransformerBl(DATA.DocumentVente Commande, List<DATA.LigneDocument> pLignes, string PathGcm, string user, string Password)
        //{
        //    var oNumPieceBl = "";
        //    Exception mEx = null;
        //    try
        //    {

        //        // MsgBox("Instanciation de l'objet base commercial ")
        //        BSCIALApplication100c oCial;
        //        // emplacement du fichier commercial 
        //        // Dim sPathGcm As String = "C:\Temp\Bijou.gcm"

        //        oCial = new BSCIALApplication100c();
        //        try
        //        {
        //            // 

        //            // MsgBox("Ouverture de la base ")
        //            if (OpenBase(oCial, PathGcm, sUid: user, sPwd: Password))
        //            {
        //                // Création du processus Commander 
        //                var pTransfo = oCial.Transformation.Achat.CreateProcess_Receptionner;
        //                // Si le devis DE00036 existe 
        //                if (oCial.FactoryDocumentVente.ExistPiece(DocumentType.DocumentTypeVenteCommande, Commande.NumPiece))
        //                {
        //                    // Sélection du devis DE00036 
        //                    IBODocumentVente3 pDoc = oCial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, Commande.NumPiece);
        //                    // Si le document contient au moins une ligne 
        //                    if (pDoc.FactoryDocumentLigne.List.Count > 0)
        //                    {
        //                        // Sélection de la première ligne du devis 
        //                        if (pLignes.Sum(o => o.Qte - o.QteALivre) != 0)
        //                        {
        //                            foreach (IBODocumentVenteLigne3 Plign3 in pDoc.FactoryDocumentLigne.List)
        //                            {
        //                                if (Plign3.Article != null)
        //                                {
        //                                    var sligne = Plign3;
        //                                    var oligne = pLignes.FirstOrDefault(oi => oi.Refrence == sligne.Article.AR_Ref & oi.Qte == sligne.DL_Qte & oi.Designation == sligne.DL_Design);

        //                                    if (oligne.QteALivre == 0)
        //                                        continue;
        //                                    else if (oligne.Qte == oligne.QteALivre)
        //                                        pTransfo.AddDocumentLigne(sligne);
        //                                    else
        //                                    {
        //                                        sligne.DL_Qte = oligne.Qte - oligne.QteALivre;
        //                                        IBODocumentVenteLigne3 onewligne = (IBODocumentVenteLigne3)pDoc.FactoryDocumentLigne.Create();
        //                                        {
        //                                            var withBlock = onewligne;
        //                                            withBlock.SetDefault();
        //                                            withBlock.SetDefaultArticleReference(sligne.Article.AR_Ref, oligne.QteALivre);
        //                                            withBlock.DL_CMUP = sligne.DL_CMUP;
        //                                            withBlock.DL_PrixUnitaire = sligne.DL_PrixUnitaire;
        //                                            withBlock.DL_QteBL = oligne.QteALivre;
        //                                            withBlock.Remise = sligne.Remise;
        //                                            withBlock.AC_RefClient = sligne.AC_RefClient;
        //                                            withBlock.AF_RefFourniss = sligne.AF_RefFourniss;
        //                                            withBlock.CompteA = sligne.CompteA;
        //                                            withBlock.Collaborateur = sligne.Collaborateur;
        //                                            withBlock.DL_Design = sligne.DL_Design;
        //                                            withBlock.DL_NoColis = sligne.DL_NoColis;
        //                                            withBlock.DL_PrixRU = sligne.DL_PrixRU;
        //                                            withBlock.Depot = sligne.Depot;
        //                                            withBlock.Write();
        //                                            sligne.Write();
        //                                        }
        //                                        pTransfo.AddDocumentLigne(onewligne);
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                            pTransfo.AddDocument(pDoc);


        //                        // Dim pLig As IBODocumentVenteLigne3 = pDoc.FactoryDocumentLigne.List(1)

        //                        // Ajout de la ligne au processus 

        //                        // Test pour savoir si le processus peut être validé 
        //                        if (pTransfo.CanProcess)
        //                        {
        //                            // MsgBox("Validation du processus ")

        //                            pTransfo.Process();
        //                            // Affichage du numéro de pièce du document créé 
        //                            // par le processus de transformation 
        //                            oNumPieceBl = (IBODocumentVente3)pTransfo.ListDocumentsResult(1).DO_Piece;
        //                        }
        //                        else
        //                            // MsgBox("Traitement de récupération des erreurs ")
        //                            RecupError((IPMProcess)pTransfo);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            mEx = ex;
        //        }
        //        finally
        //        {
        //            // MsgBox("Fermeture de la connexion ")
        //            CloseBase(oCial);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        mEx = ex;
        //    }

        //    if (mEx != null)
        //        throw mEx;
        //    // MsgBox(oNumPieceBl)
        //    return oNumPieceBl;
        //}
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
        public static void RecupError( IPMEncoder mP)
        {
            try
            {
                // Boucle sur les erreurs contenues dans la collection
                for (int i = 1; i <= mP.Errors.Count; i++)
                {
                    // Récupération des éléments erreurs
                    IFailInfo iFail = mP.Errors[i];
                    // récupération du numéro d'erreur, de l'indice et de la description de l'erreur
                    Console.WriteLine("Code Erreur : " + iFail.ErrorCode + " Indice : " + iFail.Indice );
                    ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
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
    }

}
