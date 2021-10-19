using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class TransactionEntity
    {
        private string toWallet;//id
        private string fromWallet;//id
        private decimal amount;
        private string amountDisplay;
        private string fee3Code;
        private decimal fee3Amount;
        private string fee3AmountDisplay;
        private string walletNameTo;
        private string bankCode;//ToWalletBankCode
        private string bankName;
        private DateTime timeStamp;
        private int tranId;
        private int tranType;
        private string tranCode;
        private string tranBankFrom;
        private string tranBankTo;


        public string ToWallet { get => toWallet; set => toWallet = value; }
        public string FromWallet { get => fromWallet; set => fromWallet = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public string Fee3Code { get => fee3Code; set => fee3Code = value; }
        public decimal Fee3Amount { get => fee3Amount; set => fee3Amount = value; }
        public string WalletNameTo { get => walletNameTo; set => walletNameTo = value; }
        public string BankCode { get => bankCode; set => bankCode = value; }
        public string BankName
        {
            get
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
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
        public int TranId { get => tranId; set => tranId = value; }
        public int TranType { get => tranType; set => tranType = value; }
        public string TranCode { get => tranCode; set => tranCode = value; }
        public string TranBankFrom { get => tranBankFrom; set => tranBankFrom = value; }
        public string TranBankTo { get => tranBankTo; set => tranBankTo = value; }
        public string AmountDisplay { 
            get {
                return amount.ToString();
            }
    }

        public string Fee3AmountDisplay { 
            get {
                return fee3Amount.ToString();
            }
        }
    }
}
