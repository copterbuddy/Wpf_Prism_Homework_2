using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teller.Core.Models
{
    public class FundManager
    {
        public Fund Fund;

        public SellFundFact SellFundFact;

        public string CustAccount { get; set; }
        public string CustAge { get; set; }
        public List<string> ListFundCode { get; set; }

        private static FundManager instance;

        public static FundManager Instance
            => instance;

        public static FundManager GetInstance()
        {
            if (instance == null)
            {
                instance = new FundManager();
            }
            return instance;
        }
    }
}
