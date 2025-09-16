using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22710362_HW2.Models
{
	public class Pet
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int PetTypeID { get; set; }
		public string PetTypeName { get; set; }
		public int BreedID { get; set; }
		public string BreedName { get; set; }
		public string LocationID { get; set; }
		public string LocationName { get;set; }
		public int Age { get; set; }
		public decimal Weight { get; set; }
		public string Gender { get; set; }
		public string PetStory { get; set; }
		public string  Status { get; set; }
		public int PostedByUserID { get; set; }
		public string PostedByFirstName { get; set; }
		public string PostedByLastName { get; set; }
		public int? AdoptedByUserID { get; set; }
		public string AdoptedByFirstName { get; set; }
		public string AdoptedByLastName { get; set; }
		public string B64Image { get; set; }
	}
}