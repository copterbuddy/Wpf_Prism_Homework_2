using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class TransactionEntity
    {
        private string transCode;
        private string fromWalletId;
        private string fromWalletName;
        private string toWalletId;
        private string toWalletName;
        private decimal amount;
        private decimal fee3Amount;
        private string walletNameTo;
        private string bankCode;//ToWalletBankCode
        private DateTime timeStamp;
        private string memo;
        private string transactionToken;


        
        public string AmountDisplay { 
            get {
                if (Amount.ToString().IndexOf('.') < 0)
                {
                    return Amount.ToString() + ".00";
                }
                else
                {
                    return Amount.ToString();
                }
            }
    }

        public string Fee3AmountDisplay { 
            get {
                if (Fee3Amount.ToString().IndexOf('.') < 0)
                {
                    return Fee3Amount.ToString() + ".00";
                }
                else
                {
                    return Fee3Amount.ToString();
                }
            }
        }
        public string BankName
        {
            get
            {
                var name = BankEntityManager.GetInstance().bankEntity.BankName;
                return name;
            }
        }

        public string TransCode { get => transCode; set => transCode = value; }
        public string FromWalletId { get => fromWalletId; set => fromWalletId = value; }
        public string FromWalletName { get => fromWalletName; set => fromWalletName = value; }
        public string ToWalletId { get => toWalletId; set => toWalletId = value; }
        public string ToWalletName { get => toWalletName; set => toWalletName = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public decimal Fee3Amount { get => fee3Amount; set => fee3Amount = value; }
        public string WalletNameTo { get => walletNameTo; set => walletNameTo = value; }
        public string BankCode { get => bankCode; set => bankCode = value; }
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
        public string Memo { get => memo; set => memo = value; }
        public string TransactionToken { get => transactionToken; set => transactionToken = value; }

    }
}
