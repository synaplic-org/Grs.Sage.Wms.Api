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
using Uni.Sage.Shared.Wrapper;

namespace Uni.Sage.Infrastructures.Services
{
	public interface IInventoryService
	{
		Task<Result<bool>> IntegrationInventaire();
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
		public async Task<Result<bool>> IntegrationInventaire()
		{
			try
			{
				oCial = new BSCIALApplication100c();
				if (OpenBase(ref oCial, sPathGcm))
				{

					IPMInventaire Inventaire = oCial.CreateProcess_Inventaire();
					Inventaire.Depot = oCial.FactoryDepot.ReadIntitule("Bijou SA");
					Inventaire.DateInventaire = new DateTime(2021, 12, 31);
					IBODepotEmplacement pDepotEmpl = Inventaire.Depot.FactoryDepotEmplacement.ReadCode("SECTIONBC7");
					// Article CMUP : Nouvelle quantité 7, prix 233, sur emplacement « SECTIONBC7 »
					IBOArticle3 cBOArticle = oCial.FactoryArticle.ReadReference("BAAR01");
					Inventaire.AddArticle(cBOArticle, 7, 233, pDepotEmpl);
					// Article double gamme : Nouvelle quantité 3, prix 111, sur emplacement par défaut
					cBOArticle = oCial.FactoryArticle.ReadReference("CHAAR/VAR");
					Inventaire.AddArticleDoubleGamme(cBOArticle, 3, 111,
					cBOArticle.FactoryArticleGammeEnum1.ReadEnumere("42 cm"),
					cBOArticle.FactoryArticleGammeEnum2.ReadEnumere("Forçat"),
				   null);
					// Article Série/lot : Nouvelle quantité 5, prix 66666, sur emplacement « SECTIONBC7 »
					cBOArticle = oCial.FactoryArticle.ReadReference("LINGOR18");

					IBOArticleDepot3 pArtDepot = cBOArticle.FactoryArticleDepot.ReadDepot(Inventaire.Depot);
					IBOArticleDepotLot pArtDepotLot = (IBOArticleDepotLot)pArtDepot.FactoryArticleDepotLot.ReadNoSerie("LOTBDF111111");
					Inventaire.AddArticleSerie(cBOArticle, 5, 66666, pArtDepotLot, pDepotEmpl);
					// Processus d’Inventaire
					Inventaire.Process();
					// Résultat : 2 documents : un mouvement de sortie et un mouvement d'entrée
					String sResult = "";
					foreach (IBODocumentStock3 DocStock in Inventaire.ListDocuments)
					{
						sResult = sResult + DocStock.DO_Type.ToString() + " " + DocStock.DO_Piece + "\n";
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
