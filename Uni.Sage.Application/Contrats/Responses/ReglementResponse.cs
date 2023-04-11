﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class ReglementResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CodeClient { get; set; }
        public string Client { get; set; }
        public string Reference { get; set; }
        public int Mode { get; set; }
        public decimal Montant{ get; set; }



    }
}
