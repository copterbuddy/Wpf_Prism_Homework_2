using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teller.SellFund.Models
{
    public class FundRequest
    {
        private String accountNumber { get; set; }
        private String fundGroup { get; set; }
        private String fundCode { get; set; }

        public FundRequest(String accountNumber, String fundGroup, String fundCode)
        {
            this.accountNumber = accountNumber;
            this.fundGroup = fundGroup;
            this.fundCode = fundCode;
        }
    }
}
