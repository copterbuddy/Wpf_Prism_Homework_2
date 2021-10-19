using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.WalletTransfer
{
    public class GetBankListResponse
    {
        public List<BankEntity> bankList;
        public ReturnResult returnResult;
    }
}
