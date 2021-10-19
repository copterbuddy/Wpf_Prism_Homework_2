using Entity.DTO;
using Entity.Models;
using MainModule.Service;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModule.Business.WalletTransfer
{
    public class WalletPreTransfer
    {
        public WalletPreTransfer() { }

        public DialogResult PreTransfer(string toWallet,string fromWallet,string strAmount)
        {
            DialogResult dialogResult = null;
            bool isProcess = true;
            WalletTransactionResponse transactionResponse = null;

            if (isProcess)
            {
                //Call Api Pre Transfer
                WalletService walletService = new();
                transactionResponse = walletService.WalletPreTransfer("950102002557004", "950101000010911", 100);

                if (transactionResponse == null)
                {
                    isProcess = false;
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                }
            }

            if (isProcess)
            {
                //Call Api ActLog


                //Return
                dialogResult = new DialogResult(ButtonResult.OK);
                TransactionEntityManager.GetInstance().TransactionEntity = transactionResponse.transactionEntity;
            }
            


            return dialogResult;
        }

        public DialogResult Transfer()
        {
            DialogResult dialogResult = null;
            bool isProcess = true;
            WalletTransactionResponse transactionResponse = null;
            TransactionEntityManager.GetInstance().TransactionEntity = null;

            if (isProcess)
            {
                //Call Api Pre Transfer
                WalletService walletService = new();
                transactionResponse = walletService.WalletTransfer();

                if (transactionResponse == null)
                {
                    isProcess = false;
                    dialogResult = new DialogResult(ButtonResult.Ignore);
                }
            }

            if (isProcess)
            {
                //Call Api ActLog


                //Return
                dialogResult = new DialogResult(ButtonResult.OK);
                TransactionEntityManager.GetInstance().TransactionEntity = transactionResponse.transactionEntity;
            }

            return dialogResult;
        }
    }
}
