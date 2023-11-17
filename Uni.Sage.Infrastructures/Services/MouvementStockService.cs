using Grs.Sage.Wms.Api.Services;
using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
    public interface IMouvementStockService
    {
       
        Task<Result<bool>> Transfers(ComHeaderRequest Request);
    }
    public class MouvementStockService: IMouvementStockService
    {
        private readonly IQueryService _QueryService;
        private static BSCIALApplication100c oCial;
        private static string sPathGcm = @"C:\Users\Public\Documents\Sage\Entreprise 100c\Bijou.gcm";

        public MouvementStockService(IQueryService queryService)
        {
            _QueryService = queryService;
        }

        public async Task<Result<bool>> Transfers(ComHeaderRequest Request)
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

                    //// Création du processus transférer
                    //IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer();
                    //// Affectation du document au processus

                    //pTransfert.Document = mDoc;



                    IBODepot3 mDepotSource = oCial.FactoryDepot.ReadIntitule(Request.DepotSource) ;
                    IBODepot3 mDepotsCible = oCial.FactoryDepot.ReadIntitule(Request.DepotCible);
                    mDoc.DepotDestination = mDepotsCible;
                    mDoc.DepotOrigine = mDepotSource;
                    mDoc.WriteDefault();
                    foreach (var item in Request.ComLine)
                    {
                        // Création du processus transférer
                        IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer();
                        // Affectation du document au processus

                        pTransfert.Document = mDoc;


                        //pTransfert.ListLignesResult = Convert.ToDouble(item.QuantiteScane);
                        // Affectation de l'article au processus
                        var mArt = oCial.FactoryArticle.ReadReference(item.ProductId);
                        pTransfert.SetDefaultArticle(mArt, Convert.ToDouble(item.QuantiteScane));
                        if (mArt.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
                        {
                            // Création d'un objet IUserLot
                            IUserLot mUserLot = pTransfert.UserLotsToUse.AddNew();
                            // Intitialisation du lot article


                            IBOArticleDepotLot mArtDepotLot = mArt.FactoryArticleDepot.ReadDepot(mDepotSource).FactoryArticleDepotLot.ReadNoSerie(item.StockId);
                            // Affectation du lot au processus
                            mUserLot.Lot = mArtDepotLot;
                            // Affectation de la quantité du lot
                            mUserLot.QteToUse = Convert.ToDouble(item.QuantiteScane);
                        }
                        if (pTransfert.CanProcess)
                            // Validation du processus
                            pTransfert.Process();
                        else
                            // Si CanProcess() a échoué, traitement
                            // des erreurs
                            RecupError((IPMProcess)pTransfert);
                    }

                    // Si le processus peut être validé
                   
                }
                return await Result<bool>.SuccessAsync(data: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
                return await Result<bool>.FailAsync();
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
    }
}
