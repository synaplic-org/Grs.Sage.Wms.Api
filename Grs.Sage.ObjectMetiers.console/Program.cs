

using Objets100cLib;

namespace Grs.Sage.Wms.Api
{


    public class Program
    {

        private static BSCIALApplication100c oCial;
        // emplacement du fichier commercial
        private static string sPathGcm = @"C:\Users\Public\Documents\Sage\Entreprise 100c\Bijou.gcm";

        public static void Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information

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
                        var tete = pTransfo.ListLignesATransformer;
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
                            RecupError((IPMEncoder)pTransfo);
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
        public static void RecupError(IPMEncoder mP)
        {
            try
            {
                // Boucle sur les erreurs contenues dans la collection
                for (int i = 1; i <= mP.Errors.Count; i++)
                {
                    // Récupération des éléments erreurs
                    IFailInfo iFail = mP.Errors[i];
                    // récupération du numéro d'erreur, de l'indice et de la description de l'erreur
                    Console.WriteLine("Code Erreur : " + iFail.ErrorCode + " Indice : " + iFail.Indice);
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






