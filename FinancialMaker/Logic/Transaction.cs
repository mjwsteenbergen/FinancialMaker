using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialMaker.Logic
{
    public class Transaction
    {
        public double Amount { get; set; }
        public DateTime date { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
    }
        
}
