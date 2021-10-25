using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.WalletTransfer
{
    public class SearchCustomerResponse
    {
        public List<CustomerEntity> customerEntity { get; set; }
        public ReturnResult returnResult { get; set; }
    }
}
