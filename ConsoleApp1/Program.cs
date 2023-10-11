using Objets100cLib;
using System;
using System.Linq;
using System.Collections;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;

namespace Grs.Sage.ObjetMetiers
{
    public class Program 
    { 

        private static BSCIALApplication100c oCial;
        // emplacement du fichier commercial
        private static string sPathGcm = @"C:\Users\Public\Documents\Sage\Entreprise 100c\Bijou.gcm";

        public static void Main()
        {
            try
            {
                DocumentVente Commande = new DocumentVente();

                Commande.DocType = 12;
                Commande.Depot = "Bijou SA";
                Commande.IDDepot = 1;
                Commande.NumClient = "BILLO";
                Commande.Client = "Carat S.a.r.l";
                Commande.DocDate = Convert.ToDateTime("2023-08-01T10:21:47.843Z");
                Commande.NumPiece = "FBC00018";
                Commande.RefPiece = "string";
                Commande.DocStatut = "Saisi";
                Commande.Imprime = 1;
                Commande.Reliquat = 1;
                Commande.Adrs = "string";
                Commande.LgDocument.Add(new LigneDocument
                {
                    Refrence = "BRAAR10",
                    Designation = "Bracelet, anneaux striés",
                    Qte = 3,
                    lot = "",
                    DatePeremption = Convert.ToDateTime("2026-08-01T10:21:47.843Z"),
                    NumLigne = 1000,
                });
                //Commande.LgDocument.Add(new LigneDocument
                //{
                //    Refrence = "CMUP",
                //    Designation = "CMUP",
                //    Qte = 4,
                //    lot = "",
                //    DatePeremption = Convert.ToDateTime("2026-08-01T10:21:47.843Z"),
                //    NumLigne = 1000,
                //});
                //Commande.LgDocument.Add(new LigneDocument
                //{
                //    Refrence = "LOTSAGE",
                //    Designation = "LOTSAGE",
                //    Qte = 2,
                //    lot = "L100",
                //    DatePeremption = Convert.ToDateTime("2023-08-09T10:21:47.843Z"),
                //    NumLigne = 1000,
                //});
                //Commande.LgDocument.Add(new LigneDocument
                //{
                //    Refrence = "LOTSAGE",
                //    Designation = "LOTSAGE",
                //    Qte = 2,
                //    lot = "L100",
                //    DatePeremption = Convert.ToDateTime("2023-08-09T10:21:47.843Z"),
                //    NumLigne = 1000,
                //}); ;
                TransformationBL(Commande);
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
        public static bool Trasfers_Stock(DocumentVente Commande)
        {
            try
            {
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    //IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeStockVirement);
                    // Récupération du mouvement de transfert MT00021
                    IBODocumentStock3 mDoc = oCial.FactoryDocumentStock.CreateType(DocumentType.DocumentTypeStockVirement);

					// Création du processus transférer
					IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer();
                    // Affectation du document au processus
                    
                    pTransfert.Document = mDoc;
                    IBODepot3 mDepot = oCial.FactoryDepot.ReadIntitule("Bijou SA");
                    mDoc.DepotDestination = mDepot;
                    mDoc.DepotOrigine = mDepot;
                    mDoc.WriteDefault();
                    // Affectation de l'article au processus
                    pTransfert.SetDefaultArticle(oCial.FactoryArticle.ReadReference("BAAR01"), 10);
                    // Si le processus peut être validé
                    if (pTransfert.CanProcess)
                        // Validation du processus
                        pTransfert.Process();
                    else
                        // Si CanProcess() a échoué, traitement
                        // des erreurs
                        RecupError((IPMProcess)pTransfert);
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

        //public static bool Trasfers_Stock(DocumentVente Commande)
        //{
        //    try
        //    {
        //        // Instanciation de l'objet base commercial
        //        oCial = new BSCIALApplication100c();
        //        // Ouverture de la base
        //        if (OpenBase(ref oCial, sPathGcm))
        //        {
        //            // Création du processus transférer
        //            //IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer(DocumentType.DocumentTypeStockVirement);
        //            IBODepot3 mDepot = oCial.FactoryDepot.ReadIntitule("Bijou SA");
        //            IPMDocument pDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeStockVirement);
        //            IBODocumentStock3 mDoc = (IBODocumentStock3)pDoc.Document;
        //            mDoc.DepotOrigine = mDepot;
        //            // Affectation du dépôt de destination
        //            mDoc.DepotDestination = mDepot;
        //            mDoc.SetDefaultDO_Piece();
        //            // Ecriture mémoire du document
        //            mDoc.WriteDefault();
        //            // Création du processus transférer
        //            IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer();
        //            // Affectation du document mémoire au processus
        //            pTransfert.Document = mDoc;
        //            // Affectation de l'article au processus  
        //            pTransfert.SetDefaultArticle(oCial.FactoryArticle.ReadReference("BAAR01"), 10);






        //            ////// Récupération du mouvement de transfert MT00021
        //            //IBODocumentStock3 mDoc = (IBODocumentStock3)pTransfert.Document;
        //            // = pDoc.Document
        //            //IBODepot3 mDepot = oCial.FactoryDepot.ReadIntitule("Bijou SA");
        //            //IBODepot3 mDepotD = oCial.FactoryDepot.ReadIntitule("Annexe Bijou SA");
        //            //var uu =  mDepot.DE_Code;
        //            //mDoc.DepotOrigine = mDepot;
        //            //mDoc.DepotDestination = mDepotD;


        //            //pTransfert.Document = mDoc;
        //            // Affectation de l'article au processus
        //            //pTransfert.SetDefaultArticle(oCial.FactoryArticle.ReadReference("BAAR01"), 10);
        //            // Si le processus peut être validé
        //            if (pTransfert.CanProcess)
        //                // Validation du processus
        //                pTransfert.Process();
        //            else
        //                // Si CanProcess() a échoué, traitement
        //                // des erreurs
        //                RecupError((IPMProcess)pTransfert);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Erreur : " + ex.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        // Fermeture de la connexion
        //        CloseBase(ref oCial);
        //    }
        //}

        public static bool TransformationBL(DocumentVente Commande)
        {
            try
            {

                //DocumentVente Commande = new DocumentVente();

                //Commande.DocType = 12;
                //Commande.Depot = "Bijou SA";
                //Commande.IDDepot = 1;
                //Commande.NumClient = "BILLO";
                //Commande.Client = "Billot";
                //Commande.DocDate = Convert.ToDateTime("2023-08-01T10:21:47.843Z");
                //Commande.NumPiece = "FBC00016";
                //Commande.RefPiece = "string";
                //Commande.DocStatut = "Saisi";
                //Commande.Imprime = 1;
                //Commande.Reliquat = 1;
                //Commande.Adrs = "string";
                //Commande.LgDocument.Add(new LigneDocument
                //{
                //    Refrence = "LOT",
                //    Designation = "LOT",
                //    Qte = 2,
                //    lot = "L3",
                //    DatePeremption = Convert.ToDateTime("2026-08-01T10:21:47.843Z"),
                //    NumLigne = 1000,
                //});
                //Commande.LgDocument.Add(new LigneDocument
                //{
                //    Refrence = "LOT",
                //    Designation = "LOT",
                //    Qte = 1,
                //    lot = "L4",
                //    DatePeremption = Convert.ToDateTime("2025-08-01T10:21:47.843Z"),
                //    NumLigne = 1000,
                //});
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
        public static bool TransformationBL_VENTE(DocumentVente Commande)
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
                    if (oCial.FactoryDocumentVente.ExistPiece(DocumentType.DocumentTypeVenteCommande, Commande.NumPiece))
                    {
                        // Sélection du devis DE00002
                        IBODocumentVente3 pDoc = oCial.FactoryDocumentVente.ReadPiece(DocumentType.DocumentTypeVenteCommande, Commande.NumPiece);

                        // Ajout du document au processus
                        pTransfo.AddDocument(pDoc);
                        // Création d'une table permettant de stocker
                        // les lots affectés au processus
                        var hashTb = new Hashtable();
                        // Si le document contient des lignes

                        if (pTransfo.ListLignesATransformer.Count > 0)
                        {
                            foreach (var item in Commande.LgDocument)
                            {
                                foreach (IBODocumentVenteLigne3 pLig in pTransfo.ListLignesATransformer)
                                {
                                    pLig.DL_QtePL = Convert.ToDouble(item.Qte);
                                    if (item.Refrence == pLig.Article.AR_Ref)
                                    {
                                      
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
                                        //    pTransfo.AddDocumentLigne(pLig);
                                        //}
                                    }
                                       
                                }
                            }
                            // Parcours de lignes de document présentes dans le processus
                          
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
        private static void SetLot(IPMDocTransformer pTransfo, IBODocumentVenteLigne3 pLig, Hashtable hashTb, LigneDocument Ligne)
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
                        double dQteFournir = Convert.ToDouble(Ligne.Qte);
                        if (pArtDepotLot.NoSerie == Ligne.lot)
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
        private static void SetSerie( IPMDocTransformer pTransfo,  IBODocumentVenteLigne3 pLig)
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
                        if (!pArtDepotLot.IsEpuised & pArtDepotLot.StockATerme() > 0 & !SerieAlreadyUse( pTransfo,  pLig, pArtDepotLot))
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
        private static bool SerieAlreadyUse( IPMDocTransformer pTransfo,  IBODocumentVenteLigne3 pLig,  IBOArticleDepotLot pLot)
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

         public static bool CreationBL(DocumentVente Commande)
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
                    mDoc.SetDefaultFournisseur(oCial.CptaApplication.FactoryFournisseur.ReadNumero("BILLO"));
                   
                    // Parcours de toutes les lignes du document
                    foreach (var item in Commande.LgDocument)
                    {
                        var art = oCial.FactoryArticle.ReadReference(item.Refrence);
                        
                            IBODocumentAchatLigne3 mLig = (IBODocumentAchatLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference("LOT"), 1);
                        if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            mLig.LS_NoSerie = item.lot;
                            mLig.LS_Peremption = item.DatePeremption.GetValueOrDefault();
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
        public static bool CreationBL_Vente(DocumentVente Commande)
        {
            try
            {
                // Instanciation de l'objet base commercial
                oCial = new BSCIALApplication100c();
                // Ouverture de la base
                if (OpenBase(ref oCial, sPathGcm))
                {
                    // Création d'un objet processus "Création de document"
                    IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeVenteLivraison);
                    ;
                    /* Cannot convert EmptyStatementSyntax, CONVERSION ERROR: Conversion for EmptyStatement not implemented, please report this issue in '' at character 350


                                    Input:
                                     'Conversion du document du processus (IBODocument3) dans le type du document de destination : Facture de vente
                                    (IBODocumentVente3)

                                     */
                    IBODocumentVente3 mDoc = (IBODocumentVente3)mProcessDoc.Document;
                    // Indique au document qu’il ne doit pas recalculer les totaus automatiquement à chaque modification ou ajout de lignes
                    mDoc.SetAutoRecalculTotaux(false);/* TODO ERROR: Skipped SkippedTokensTrivia
;*/

                    // Affectation du client au document
                    // Ajout d'une ligne sur l'article ENSHF de nomenclature commerciale/composé et
                    // conversion dans le type de ligne de document de destination (IBODocumentVenteLigne3).
                    // Lors de l'ajout de cette ligne, les autres lignes composant la nomenclature sont également ajoutées
                    mDoc.SetDefaultClient(oCial.CptaApplication.FactoryClient.ReadNumero("CARAT"));

                    // Parcours de toutes les lignes du document
                    foreach (var item in Commande.LgDocument)
                    {
                        var art = oCial.FactoryArticle.ReadReference(item.Refrence);

                        IBODocumentVenteLigne3 mLig = (IBODocumentVenteLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.Refrence), Convert.ToDouble(item.Qte));
                        if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            mLig.LS_NoSerie = item.lot;
                            mLig.LS_Peremption = item.DatePeremption.GetValueOrDefault();
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
        public static void RecupError( IPMProcess mP)
        {

            // Boucle sur les erreurs contenues dans la collection 
            string msg = "";
            for (int i = 1; i <= mP.Errors.Count; i++)
            {
                // Récupération des éléments erreurs 
                IFailInfo iFail = mP.Errors[i];
                // récupération du numéro d'erreur,
                // de l'indice et de la description de l'erreur 
                msg += "Code Erreur : " + iFail.ErrorCode + " Indice : " + iFail.Indice + " Description : " + iFail.Text ;
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






