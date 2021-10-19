using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class WalletTransactionResponse
    {
        public TransactionEntity transactionEntity { get; set; }
        public ReturnResult returnResult { get; set; }
    }
}
