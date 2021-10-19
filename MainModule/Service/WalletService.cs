using Entity.DTO;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModule.Service
{
    public class WalletService
    {
        public string GetToWallet(string walletId,string bankCode)
        {
            return "นางรุ่งรัตน์ ภูมิพร";

        }

        public WalletTransactionResponse WalletPreTransfer(string fromWalletId, string toWalletId, decimal amount)
        {
            WalletTransactionResponse walletPreTransferResponse = new();

            TransactionEntity transactionEntity = new();
            transactionEntity.FromWallet = fromWalletId;
            transactionEntity.ToWallet = toWalletId;
            transactionEntity.Amount = amount;
            transactionEntity.Fee3Amount = 0;
            transactionEntity.WalletNameTo = "นางรุ่งรัตน์ ภูมิพร";
            transactionEntity.BankCode = "002";
            transactionEntity.TimeStamp = DateTime.Now;
            transactionEntity.TranId = 10;
            transactionEntity.TranType = 95;
            transactionEntity.TranCode = "SMTR0001";
            transactionEntity.TranBankFrom = "SMTR";
            transactionEntity.TranBankTo = "BBL";

            walletPreTransferResponse.transactionEntity = transactionEntity;

            return walletPreTransferResponse;

        }

        public WalletTransactionResponse WalletTransfer()
        {
            //transCode   string SMTR00001
            //fromWalletName  string นายกิตติ โชค(บัญชี1)
            //fromWalletId    string  950102002557004
            //toWalletName    string นางรุ่งรัตน์ ภูมิพร
            //toWalletId  string  950101000010911
            //amount  decimal 100
            //fee3Amount  decimal 0
            //bankCode    string  950
            //memo    string โอนให้เพื่อน
            //timeStamp DateTime    2021 - 08 - 25T19: 01:03.0338789Z
            //   tranId  int 106788
            //tranType    int transfer_gf
            //tranCode    string SMTR0001
            //tranBankFrom    string SMTR
            //tranBankTo  string BBL



            WalletTransactionResponse walletTransactionResponse = new();

            TransactionEntity transactionEntity = new();
            transactionEntity.FromWallet = "1211212121212";
            transactionEntity.ToWallet = "424545454545455";
            transactionEntity.Amount = 50;
            transactionEntity.Fee3Amount = 0;
            transactionEntity.WalletNameTo = "นางรุ่งรัตน์ ภูมิพร";
            transactionEntity.BankCode = "002";
            transactionEntity.TimeStamp = DateTime.Now;
            transactionEntity.TranId = 10;
            transactionEntity.TranType = 95;
            transactionEntity.TranCode = "SMTR0001";
            transactionEntity.TranBankFrom = "SMTR";
            transactionEntity.TranBankTo = "BBL";

            walletTransactionResponse.transactionEntity = transactionEntity;

            return walletTransactionResponse;

        }
    }
}
