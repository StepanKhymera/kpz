using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Policy
    {

        public int PolicyID { get; set; }
        public string Customer_Name { get; set; }
        public string Employee_Name { get; set; }
        public string Insurance_Type { get; set; }
        public DateTime Policy_Start_Date { get; set; }
        public DateTime Policy_Expiration_Date { get; set; }
        public Decimal Anual_Fee { get; set; }
        public string Info_About { get; set; }
        public Decimal Coverage { get; set; }
    }
}
