using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Claim
    {
        public int ClaimID { get; set; }
        public string Client_name { get; set; }
        public string Insurance_type { get; set; }
        public DateTime date_of_claim { get; set; }
        public int amount_of_claim { get; set; }
        public string status_of_claim { get; set; }
    }
}
