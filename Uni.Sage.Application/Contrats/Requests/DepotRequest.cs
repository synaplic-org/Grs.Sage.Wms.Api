using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Application.Contrats.Requests
{
    public  class DepotRequest
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Complement { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Contact { get; set; }
        public string Region { get; set; }
        public string Pays { get; set; }
        public string Email { get; set; }
        //public string Code { get; set; }
        public string Telephone { get; set; }
        public string Telecopie { get; set; }
        public string pConnexionName { get; set; }
    }
}
