using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Responses
{
    public class ClientArticleResponse
    {
        public string CodeClient { get; set; }
        public string Reference { get; set; }
        public string Designation { get; set; }
        public string CodeFamille { get; set; }
        public string Famille { get; set; }
        public string CodeCentrale { get; set; }  
        public string FamilleCentrale { get; set; }
        public float Prix { get; set; }
        public string Type { get; set; }
        public string Remise { get; set; }
        public short EnSommiel { get; set; }
        public int SuiviStock { get; set; }
    }
}