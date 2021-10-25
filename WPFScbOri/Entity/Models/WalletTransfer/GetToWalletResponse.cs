using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.WalletTransfer
{
    public class GetToWalletResponse
    {
        string walletName;
        ReturnResult returnResult;

        public string WalletName { get => walletName; set => walletName = value; }
        public ReturnResult ReturnResult { get => returnResult; set => returnResult = value; }
    }
}
