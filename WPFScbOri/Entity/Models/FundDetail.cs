using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teller.SellFund.Models
{
    public class FundDetail
    {  
        public string FundType { get; set; }
        public string FundName { get; set; }
        public string FundCode { get; set; }
        public string ReFundType { get; set; }
        public decimal Amount { get; set; }
        public string AmountString { get; set; }
        public string TransactionStatus { get; set; }
    }
}
