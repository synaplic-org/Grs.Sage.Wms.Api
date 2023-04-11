using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Grs.Sage.Wms.Api.Services;
using Uni.Sage.Application.Contrats.Requests;
using Uni.Sage.Domain.Entities;
using Uni.Sage.Infrastructures.Services;
using System.Linq;
using System.Collections.Generic;
using Uni.Sage.Shared.Extention;

namespace Grs.Sage.Wms.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Vente/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class DevisController : Controller
    {
        private readonly IVenteService _VenteService;
        private readonly ISoucheService _SoucheService;
        private readonly IDocumentService _DocumentService;
        private readonly IClientService _ClientService;
        private readonly IArticleService _ArticleService;


        public DevisController(IVenteService venteService, ISoucheService soucheService, IDocumentService documentService,
            IClientService clientService, IArticleService articleService) : base()
        {

            _VenteService = venteService;
            _SoucheService = soucheService;
            _DocumentService = documentService;
            _ClientService = clientService;
            _ArticleService = articleService;

        }

        [HttpPost(nameof(GetEnteteDevisList))]
        public async Task<ActionResult> GetEnteteDevisList(DevisListRequest devisList)
        {
            var result = await _VenteService.Devis_Entete_Vente(devisList.ConnectionName);
            return Ok(result);
        }

        [HttpPost(nameof(GetLigneDevisList))]
        public async Task<ActionResult> GetLigneDevisList(DevisListRequest devisList)
        {
            var result = await _VenteService.Devis_Ligne_Vente(devisList.ConnectionName);
            return Ok(result);
        }

        string Piece;

        [HttpPost(nameof(PosteDevisVente))]
        public async Task<ActionResult> PosteDevisVente(Devis_EnteteRequest devis)
        {
            #region Insertion to F_docentete
            F_DOCENTETE entete = new F_DOCENTETE();
            entete.DO_Domaine = 0;
            entete.DO_Type = 0;
            entete.DO_Period = 1;
            entete.DO_Devise = 0;
            entete.DO_Cours = 0;
            entete.LI_No = 1;
            entete.DO_Expedit = 1;
            entete.DO_NbFacture = 1;
            entete.DO_BLFact = 0;
            entete.DO_TxEscompte = 0;
            entete.DO_Reliquat = 0;
            entete.DO_Imprim = 0;
            entete.DO_Coord01 = "";
            entete.DO_Coord02 = "";
            entete.DO_Coord03 = "";
            entete.DO_DateLivr = DateTime.MinValue.MinGregorianDate();
            entete.DO_Condition = 1;
            entete.DO_Tarif = 1;
            entete.DO_Colisage = 1;
            entete.DO_TypeColis = 1;
            entete.DO_Transaction =0;
            entete.DO_Langue = 0;
            entete.DO_Ecart = 0;
            entete.DO_Regime = 0;
            entete.N_CatCompta = 1;
            entete.DO_Ventile = 0;
            entete.AB_No = 0;
            entete.DO_DebutAbo = DateTime.MinValue.MinGregorianDate();
            entete.DO_FinAbo = DateTime.MinValue.MinGregorianDate();
            entete.DO_DebutPeriod = DateTime.MinValue.MinGregorianDate();
            entete.DO_FinPeriod = DateTime.MinValue.MinGregorianDate();
            entete.DO_Heure = "000" + DateTime.Now.ToString("HHmmss");
            entete.CA_No = 0;
            entete.CO_NoCaissier = 0;
            entete.DO_Transfere = 0;
            entete.DO_Cloture = 0;
            entete.DO_NoWeb = "";
            entete.DO_Attente = 0;
            entete.DO_Provenance = 0;
            entete.CA_NumIFRS = "";
            entete.MR_No = 0;
            entete.DO_TypeFrais = 0;
            entete.DO_ValFrais = 0;
            entete.DO_TypeLigneFrais = 0;
            entete.DO_TypeFranco = 0;
            entete.DO_ValFranco = 0;
            entete.DO_TypeLigneFranco = 0;
            entete.DO_MajCpta = 0;
            entete.DO_Motif = "";
            entete.DO_Contact = "";
            entete.DO_FactureElec = 0;
            entete.DO_TypeTransac = 0;
            entete.DO_DateLivrRealisee = DateTime.MinValue.MinGregorianDate();
            entete.DO_DateExpedition = DateTime.MinValue.MinGregorianDate();
            entete.DO_FactureFrs = "";
            entete.DO_PieceOrig = "";
            entete.DO_EStatut = 0;
            entete.DO_DemandeRegul = 0;
            entete.ET_No = 0;
            entete.DO_Valide = 0;
            entete.DO_Coffre = 0;
            entete.DO_StatutBAP = 0;
            entete.DO_Escompte = 1;
            entete.DO_DocType = 0;
            entete.DO_TypeCalcul = 0;
            entete.DO_MontantRegle = 0;
            entete.DO_AdressePaiement = "";
            entete.DO_PaiementLigne = 0;
            entete.DO_MotifDevis = 0;
            entete.cbProt = 0;
            entete.cbCreateur = "ERP1";
            entete.cbModification = DateTime.Now;
            entete.cbReplication = 0;
            entete.cbFlag = 0;
            entete.cbCreation = DateTime.Now;
            entete.cbHashVersion = 1;
            entete.DO_Conversion = 0;
            entete.DO_Taxe1 = 0;
            entete.DO_Taxe2 = 0;
            entete.DO_Taxe3 = 0;
            entete.DO_TypeTaux1 = 0;
            entete.DO_TypeTaux2 = 0;
            entete.DO_TypeTaux3 = 0;
            entete.DO_TypeTaxe1 = 0;
            entete.DO_TypeTaxe2 = 0;
            entete.DO_TypeTaxe3 = 0;

            // search la souche
            var oIsExistSouche = await _SoucheService.GetSouchesVenteById(devis.ConnectionName,devis.Souche);
            var oNumeroPiece = await _DocumentService.GetDocumentDevisVente(devis.ConnectionName, oIsExistSouche.Data.Select(x=>x.Indice).FirstOrDefault());
            Piece = oNumeroPiece.Data.Select(x => x.Piece).FirstOrDefault();
            // search le client
            var oIsExistClient = await _ClientService.GetClientsById(devis.ConnectionName, devis.Client);


            // insertion le valeur
            entete.DO_Date = devis.Date;
            entete.DO_Piece = Piece;
            entete.DO_Ref = devis.Reference;
            entete.DO_Tiers = devis.Client;
            entete.CO_No = oIsExistClient.Data.Select(x=>x.CodeCollaborateur).FirstOrDefault();
            entete.DE_No = oIsExistClient.Data.Select(x => x.CodeDepot).FirstOrDefault();
            entete.CT_NumPayeur = devis.Client;
            entete.CA_Num = devis.Affaire;
            entete.DO_Souche = Convert.ToInt16(oIsExistSouche.Data.Select(x => x.Indice).FirstOrDefault());
            entete.CG_Num = devis.CompteCollectif;
            entete.DO_Statut = 0;
            entete.DO_TotalHT = 0;
            entete.DO_TotalHTNet = 0;
            entete.DO_TotalTTC = 0;
            entete.DO_NetAPayer = 0;



            // update Numero Piece to F_DOCCURRENTPIECE
            List<string> LDO = new List<string>();
            int LDO_PIECE = 0;
            LDO.Add(Piece);
            LDO_PIECE = LDO.Select(o => int.Parse(o.Substring(2))).Max();
            var oNpiece = "DE" + (LDO_PIECE + 1).ToString().PadLeft(5, '0');



            var oUpdatePiece =  await _DocumentService.UpdateDocumentDevisVente(devis.ConnectionName, oIsExistSouche.Data.Select(x => x.Indice).FirstOrDefault(), oNpiece);
            var tets = oUpdatePiece;
            
            
            #endregion



            //#region Insertion to F_docregle

            //F_DOCREGLE docregle = new F_DOCREGLE();
            //docregle.DO_Domaine = 0;
            //docregle.DO_Type = 0;
            //docregle.DO_Piece = oNumeroPiece.Data.Select(x => x.Piece).FirstOrDefault();
            //docregle.DR_TypeRegl = 2;
            //docregle.DR_Date = devis.Date;
            //docregle.DR_Libelle = "";
            //docregle.DR_Pourcent = 0;
            //docregle.DR_Montant = 0;
            //docregle.DR_MontantDev = 0;
            //docregle.DR_Equil = 1;
            //docregle.EC_No = 0;
            //docregle.DR_Regle = 0;
            //docregle.N_Reglement = 1;
            //docregle.CA_No = 0;
            //docregle.DO_DocType = 0;
            //docregle.CbProt = 0;
            //docregle.CbCreateur = "ERP1";
            //docregle.CbModification = DateTime.Now;
            //docregle.CbReplication = 0;
            //docregle.CbFlag = 0;
            //docregle.CbCreation = DateTime.Now;
            //docregle.CbHashVersion = 1;
            //docregle.DR_AdressePaiement = "";


            //#endregion




            //#region Insertion to F_docligne
            //var oLigneDevis = devis.Lignes;
            //for (int i = 0; i < oLigneDevis.Count; i++)
            //{
            //    F_DOCLIGNE docligne = new F_DOCLIGNE();
            //    docligne.DO_Domaine = 0;
            //    docligne.DO_Type = 0;
            //    docligne.DL_PieceBC = "";
            //    docligne.DL_PieceBL = "";
            //    docligne.DL_DateBC = DateTime.MinValue.MinGregorianDate();
            //    docligne.DL_DateBL = DateTime.MinValue.MinGregorianDate();
            //    docligne.DL_TNomencl = 0;
            //    docligne.DL_TRemPied = 0;
            //    docligne.DL_TRemExep = 0;
            //    docligne.DL_PoidsNet = 0;
            //    docligne.DL_PoidsBrut = 0;
            //    docligne.DT_No = 0;
            //    docligne.AF_RefFourniss = "";
            //    docligne.DL_NoRef = 1;
            //    docligne.DL_TypePL = 0;
            //    docligne.DO_DateLivr = DateTime.MinValue.MinGregorianDate();
            //    docligne.DL_Frais = 0;
            //    docligne.DL_Valorise = 1;
            //    docligne.DL_NonLivre = 0;
            //    docligne.AC_RefClient = "";
            //    docligne.DL_FactPoids = 0;
            //    docligne.DL_Escompte = 0;
            //    docligne.DL_PiecePL = "";
            //    docligne.DL_DatePL = DateTime.MinValue.MinGregorianDate();
            //    docligne.DL_QtePL = 0;
            //    docligne.DL_NoColis = "";
            //    docligne.DL_NoLink = 0;
            //    docligne.DL_QteRessource = 0;
            //    docligne.DL_DateAvancement = DateTime.MinValue.MinGregorianDate();
            //    docligne.PF_Num = "";
            //    docligne.DL_PieceOFProd = 0;
            //    docligne.DL_PieceDE = "";
            //    docligne.DL_DateDE = DateTime.Now.Date;
            //    docligne.DL_Operation = "";
            //    docligne.DL_NoSousTotal = 0;
            //    docligne.CA_No = 0;
            //    docligne.DO_DocType = 0;
            //    docligne.cbProt = 0;
            //    docligne.cbCreateur = "ERP1";
            //    docligne.cbModification = DateTime.Now;
            //    docligne.cbReplication = 0;
            //    docligne.cbFlag = 0;
            //    docligne.cbCreation = DateTime.Now;
            //    docligne.cbHashVersion = 1;
            //    docligne.DL_Ligne = (i + 1) * 1000;
            //    docligne.DL_PUDevise = 0;
            //    docligne.DL_MvtStock = 0;

            //    // serach article
            //    var oListeArticle = await _ArticleService.GetArticles(devis.ConnectionName);
            //    var oArticle = oListeArticle.Data.SingleOrDefault(x => x.Reference ==
            //        oLigneDevis[i].Reference);

            //    // DL_NO
            //    var oLastDLNO = await _VenteService.CountDLNO(devis.ConnectionName);
            //    var oDL_no = oLastDLNO[0];


            //    // les valeur inserer 
            //    docligne.CT_Num = devis.Client;
            //    docligne.DO_Piece = Piece;
            //    docligne.DO_Date = devis.Date;
            //    docligne.DO_Ref = devis.Reference;
            //    docligne.CA_Num = devis.Affaire;
            //    docligne.CO_No = oIsExistClient.Data.Select(x => x.CodeCollaborateur).FirstOrDefault();
            //    docligne.DE_No = oIsExistClient.Data.Select(x => x.CodeDepot).FirstOrDefault();
            //    docligne.AR_Ref = oLigneDevis[i].Reference;
            //    docligne.DL_Design = oLigneDevis[i].Designation;
            //    docligne.DL_Qte = oLigneDevis[i].Quantite;
            //    docligne.DL_QteBC = oLigneDevis[i].Quantite;
            //    docligne.DL_QteBL = oLigneDevis[i].Quantite;
            //    docligne.EU_Qte = oLigneDevis[i].Quantite;
            //    docligne.DL_QteBL = oLigneDevis[i].Quantite;
            //    docligne.EU_Enumere = oArticle.Unite;
            //    docligne.DL_No = int.Parse(oDL_no);
            //   // ligne (dl_ligne)



            //}
            


            //#endregion
            return Ok(true);
        }
    }
}
