using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22710362_HW2.Models
{
	public class PetRecordsVM
	{
		public List<Pet> Pets { get; set; }
		public List<User> Users { get; set; }
		public List<PetType> PetTypes { get; set; }
		public List<PetBreed> PetBreeds { get; set; }
		public List<Location> Locations { get; set; }
		public List<Donation> Donations { get; set; }
		public decimal TotalDonations { get; set; }
		public decimal DonationGoal { get; set; }
		public int TotalAdoptions { get; set; }
		public PetRecordsVM()
		{
			DonationGoal = 25000.00m;
		}
	}
}