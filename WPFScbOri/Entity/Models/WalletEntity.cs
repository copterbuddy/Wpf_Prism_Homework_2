using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Entity.Models
{
    public class WalletEntity
    {
        private string walletId;
        private string walletName;
        private ImageSource walletlImage;
        private decimal balance;
        private string bankCode;
        private string bankName;

        public string WalletId { get => walletId; set => walletId = value; }
        public ImageSource WalletlImage { get => walletlImage; set => walletlImage = value; }
        public string WalletName { get => walletName; set => walletName = value; }
        public decimal Balance { get => balance; set => balance = value; }
        public string BankCode { get => bankCode; set => bankCode = value; }
        public string BankName { get
            {
                var bankList = BankListEntityManager.GetInstance().bankListEntity;
                if (bankList != null && bankList.BankList != null && bankList.BankList.Count > 0)
                {
                    foreach (var item in bankList.BankList)
                    {
                        if (item.BankCode == BankCode)
                        {
                            bankName = item.BankName;
                        }
                    }
                }
                return bankName;
            }
        }

        public WalletEntity() { }

        public WalletEntity(string walletId,string walletName,ImageSource walletlImage,decimal balance,string bankCode)
        {
            this.walletId = walletId;
            this.walletName = walletName;
            this.walletName = walletName;
            this.walletlImage = walletlImage;
            this.balance = balance;
            this.bankCode = bankCode;
        }
    }

    public class WalletListEntity
    {
        public WalletListEntity()
        {
            WalletList = new();
        }

        private List<WalletEntity> walletList;

        public List<WalletEntity> WalletList { get => walletList; set => walletList = value; }
    }
}
