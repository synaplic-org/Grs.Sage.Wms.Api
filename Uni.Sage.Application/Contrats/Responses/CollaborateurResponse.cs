using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public  class CollaborateurResponse
    {
		

		public int ErpID { get; set; }
		public string Name { get; set; }
		public string Prenom { get; set; }
		public string Fonction { get; set; }
		public string Adress { get; set; }
		public string Complement { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Telephone { get; set; }
	    public string Email { get; set; }
		public bool IsEnabled { get; set; }

    }
}
