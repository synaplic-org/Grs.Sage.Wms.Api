using Grs.Sage.Wms.Api.Services;
using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Application.Contrats.Responses;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Shared.Communication;
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
	public interface IInventoryService
	{
		Task<Result<bool>> IntegrationInventaire(List<InventoryRequest> request);
	}
	public class InventoryService : IInventoryService
	{
		private readonly IQueryService _QueryService;
		private static BSCIALApplication100c oCial;
		private static string sPathGcm = @"C:\Users\Public\Documents\Sage\Entreprise 100c\Bijou.gcm";

		public InventoryService(IQueryService queryService)
		{
			_QueryService = queryService;
		}
		public async Task<Result<bool>> IntegrationInventaire(List<InventoryRequest> request)
		{
			try
			{
				oCial = new BSCIALApplication100c();
				if (OpenBase(ref oCial, sPathGcm))
				{
					List<InventoryRequest> test = new List<InventoryRequest>();
                    IPMInventaire Inventaire = oCial.CreateProcess_Inventaire();
					Inventaire.Depot = oCial.FactoryDepot.ReadIntitule("Bijou SA");
					Inventaire.DateInventaire = DateTime.Now;
                    IBODepotEmplacement pDepotEmpl = Inventaire.Depot.FactoryDepotEmplacement.ReadCode("A1T2N2P2");

					//DepotEmplZone zone = Inventaire.Depot.
					pDepotEmpl.DP_Zone = DepotEmplZone.DepotEmplZoneB;
                    foreach (var item in request)
					{
                        var art = oCial.FactoryArticle.ReadReference(item.ProductId);
						if (art.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
						{
                            IBOArticle3 cBOArticle = oCial.FactoryArticle.ReadReference(item.ProductId);
                            

                            IBOArticleDepot3 pArtDepot = cBOArticle.FactoryArticleDepot.ReadDepot(Inventaire.Depot);
							var dd = pArtDepot.FactoryArticleDepotLot.ExistNoSerie(item.StockId);
							if (dd == false)
							{
								test.Add(item);
        //                        IBOArticleDepotLot pArtDepotLot = (IBOArticleDepotLot)pArtDepot.FactoryArticleDepotLot.Create();
								
        //                        var ee = pArtDepotLot;
								//var vdv = pArtDepotLot.NoSerie = "efed";
        //                        pArtDepotLot.NoSerie = "tetetetteettet";
								//pArtDepotLot.DatePeremption = DateTime.Now.Date;
        //                        Inventaire.AddArticleSerie(cBOArticle, Convert.ToDouble(item.Quantite), Convert.ToDouble(item.prix), pArtDepotLot, pDepotEmpl);
                                //Inventaire.ad
                            }
                            else
							{
                                IBOArticleDepotLot pArtDepotLot = (IBOArticleDepotLot)pArtDepot.FactoryArticleDepotLot.ReadNoSerie(item.StockId);
								pArtDepotLot.DatePeremption = item.DatePeremption;
                                Inventaire.AddArticleSerie(cBOArticle, Convert.ToDouble(item.QuantiteInventaire), Convert.ToDouble(item.ValeurUnitaire), pArtDepotLot, pDepotEmpl);
                            }
                           
                        }
						else if(art.AR_SuiviStock == SuiviStockType.SuiviStockTypeCmup)
						{
                            // Article CMUP : Nouvelle quantité 7, prix 233, sur emplacement « SECTIONBC7 »
                            IBOArticle3 cBOArticle = oCial.FactoryArticle.ReadReference(item.ProductId);


                            Inventaire.AddArticle(cBOArticle, Convert.ToDouble(item.QuantiteInventaire), Convert.ToDouble(item.ValeurUnitaire), pDepotEmpl);
                        }
                    }




                    // Article double gamme : Nouvelle quantité 3, prix 111, sur emplacement par défaut
                    //cBOArticle = oCial.FactoryArticle.ReadReference("CHAAR/VAR");
                    //Inventaire.AddArticleDoubleGamme(cBOArticle, 3, 111,
                    //cBOArticle.FactoryArticleGammeEnum1.ReadEnumere("42 cm"),
                    //cBOArticle.FactoryArticleGammeEnum2.ReadEnumere("Forçat"),
                    //  null);
                    // Article Série/lot : Nouvelle quantité 5, prix 66666, sur emplacement « SECTIONBC7 »

                    // Processus d’Inventaire
                    if (!Inventaire.CanProcess)
                    {
                        RecupError(Inventaire);

                    }
                    else
                    {
                        Inventaire.Process();

                    }
                   
					// Résultat : 2 documents : un mouvement de sortie et un mouvement d'entrée
					String sResult = "";
					foreach (IBODocumentStock3 DocStock in Inventaire.ListDocuments)
					{
						sResult = sResult + DocStock.DO_Type.ToString() + " " + DocStock.DO_Piece + "\n";
					}
                    IBODepotEmplacement pDepotEmplS = Inventaire.Depot.FactoryDepotEmplacement.ReadCode("A1T2N3P3");

                    //DepotEmplZone zone = Inventaire.Depot.
                    pDepotEmplS.DP_Zone = DepotEmplZone.DepotEmplZoneC;
                    var MouvementSortie = request.Where(x => x.QuantiteInventaire == 0 && x.QuantiteStock > 0).ToList();
					if (MouvementSortie.Count > 0)
					{
                        IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeStockMouvOut);
                        IBODocumentStock3 mDoc = (IBODocumentStock3)mProcessDoc.Document;
                        //IBODocumentStock3 mDoc = oCial.FactoryDocumentStock.CreateType(DocumentType.DocumentTypeStockMouvIn);
                        IBODepot3 mDepotSource = oCial.FactoryDepot.ReadIntitule("Bijou SA");
                        mDoc.DepotStockage = mDepotSource;
                        mDoc.DO_Ref = "inv.";
						
                        mDoc.WriteDefault();
                        foreach (var item in MouvementSortie)
                        {
                            //IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeStockMouvIn);
                            //IPMProcess mDoc = (IPMProcess)mProcessDoc.Document;
                            // Création du processus transférer
                            //IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer();
                            // Affectation du document au processus
                            var arts = oCial.FactoryArticle.ReadReference(item.ProductId);
                            IBODocumentStockLigne3 mLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.ProductId), Convert.ToDouble(item.QuantiteStock));
							//mLig.SetEmplacementEntree(pDepotEmplS);

							//if (arts.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
							//{
							//	mLig.LS_NoSerie = item.StockId;
							//	//mLig.DL_PrixUnitaire = Convert.ToDouble(item.ValeurUnitaire);

							//	//mLig.LS_Peremption = item.DatePeremption;
							//}

						}
                        if (!mProcessDoc.CanProcess)
                        {
                            RecupError(mProcessDoc);

                        }
                        else
                        {
                            mProcessDoc.Process();

                        }
                    }
                    if (test.Count > 0)
					{
						IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeStockMouvIn);
						IBODocumentStock3 mDoc = (IBODocumentStock3)mProcessDoc.Document;
						//IBODocumentStock3 mDoc = oCial.FactoryDocumentStock.CreateType(DocumentType.DocumentTypeStockMouvIn);
						IBODepot3 mDepotSource = oCial.FactoryDepot.ReadIntitule("Bijou SA");
						mDoc.DepotStockage = mDepotSource;
						mDoc.DO_Ref = "inv.";

                        mDoc.WriteDefault();
						foreach (var item in test)
						{
							//IPMDocument mProcessDoc = oCial.CreateProcess_Document(DocumentType.DocumentTypeStockMouvIn);
							//IPMProcess mDoc = (IPMProcess)mProcessDoc.Document;
							// Création du processus transférer
							//IPMDocTransferer pTransfert = oCial.CreateProcess_DocTransferer();
							// Affectation du document au processus
							var arts = oCial.FactoryArticle.ReadReference(item.ProductId);
							IBODocumentStockLigne3 mLig = (IBODocumentStockLigne3)mProcessDoc.AddArticle(oCial.FactoryArticle.ReadReference(item.ProductId), Convert.ToDouble(item.QuantiteInventaire));
							//mLig.SetEmplacementEntree(pDepotEmpl);

							if (arts.AR_SuiviStock == SuiviStockType.SuiviStockTypeLot)
							{
								mLig.LS_NoSerie = item.StockId;
								//mLig.DL_PrixUnitaire = Convert.ToDouble(item.ValeurUnitaire);

								//mLig.LS_Peremption = item.DatePeremption;
							}

						}
						if (!mProcessDoc.CanProcess)
						{
							RecupError(mProcessDoc);

						}
						else
						{
							mProcessDoc.Process();

						}
					}
				}
                return await Result<bool>.SuccessAsync();
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
				throw;

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
