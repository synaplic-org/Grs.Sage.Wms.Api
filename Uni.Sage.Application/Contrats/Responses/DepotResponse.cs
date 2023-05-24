using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public class DepotResponse
    {
        public int ErpID { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        //public string Complement { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
        public string Region { get; set; }
        public string Telephone { get; set; }
        //public string Telecopie { get; set; }
    }
}
