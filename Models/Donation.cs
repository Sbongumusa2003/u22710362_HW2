using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22710362_HW2.Models
{
	public class Donation
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public decimal Amount { get; set; }
		public DateTime DonationDate { get; set; }
	}
}