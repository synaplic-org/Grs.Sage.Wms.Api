using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public  class ClientResponse
    {
        public string ErpID { get; set; }
        public string Name { get; set; }
        public int PartyType { get; set; }
        public string CollectiveAccount { get; set; }
        public string Tarif { get; set; }
        public string Contact { get; set; }

        public string Telephone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string Identifiant { get; set; }
        public int CollaboratorID { get; set; }
        public int DefaultWareHouseErpID { get; set; }
        public bool IsEnabled { get; set; }
    }
}
