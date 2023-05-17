using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni.Sage.Domain.Entities
{
    public  class CollaborateurResponse
    {
		

		public int CodeCollaborateur { get; set; }
		public string Nom { get; set; }
		public string Prenom { get; set; }
		public string Matricule { get; set; }
		public string Fonction { get; set; }
		public string Adresse { get; set; }
		public string Complement { get; set; }
		public string CodePostal { get; set; }
		public string Ville { get; set; }
		public string Service { get; set; }
		public string Telephone { get; set; }
		public string TelPortable { get; set; }
	    public string Email { get; set; }
	}
}
