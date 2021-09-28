using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teller.Core.Models
{
    public class SellFundFact
    {
        public bool LTF_Fact { get; set; }
        public bool RMF_Fact { get; set; }
        public bool SSF_Fact { get; set; }
        public bool Inactive_Fact { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal OverAmount { get; set; }
        public decimal AllowRedeem { get; set; }
        public string SaleType { get; set; }
        public string RecieveType { get; set; }
        public string RecieveAccount { get; set; }
        public SellFundFact() { }
        public SellFundFact(bool LTF_Fact, bool RMF_Fact, bool SSF_Fact, bool Inactive_Fact, decimal SaleAmount, decimal OverAmount, decimal AllowRedeem, string SaleType)
        {
            this.LTF_Fact = LTF_Fact;
            this.RMF_Fact = RMF_Fact;
            this.SSF_Fact = SSF_Fact;
            this.Inactive_Fact = Inactive_Fact;
            this.SaleAmount = SaleAmount;
            this.OverAmount = OverAmount;
            this.AllowRedeem = AllowRedeem;
            this.SaleType = SaleType;
        }
    }
}
