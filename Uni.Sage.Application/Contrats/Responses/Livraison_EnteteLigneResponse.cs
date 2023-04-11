﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Sage.Shared.Communication;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class Livraison_EnteteLigneResponse 
    {
        public string Piece { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string CodeArticle { get; set; }
        public string Designation { get; set; }
        public decimal Quantite { get; set; }
        public decimal Remise { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal MontantHT { get; set; }
        public decimal MontantTTC { get; set; }
        

    }
}
