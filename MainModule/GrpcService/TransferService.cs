using Entity.DTO;
using Grpc.Core;
using Grpc.Net.Client;
using MainModule.GrpcService.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPFScbOri.Models;

namespace MainModule.GrpcService
{
    public class TransferService
    {
        public async Task<WalletTransactionResponse> PreTransfer(string fromWalletId,string toWalletId,string bankCode,double amount,string memo)
        {
            WalletTransactionResponse res = new();
            try
            {
                //AppContext.SetSwitch("System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
                using var httpClient = new HttpClient(handler);
                var credentials = CallCredentials.FromInterceptor((context, metaData) =>
                {
                    //metaData.Add("Authorization", "Bearer 1234");
                    return Task.CompletedTask;
                });
                using var channel = GrpcChannel.ForAddress(BaseService._baseUrlTransfer, new GrpcChannelOptions()
                {
                    HttpClient = httpClient
                });

                var client = new TransferWalletGrpc.TransferWalletService.TransferWalletServiceClient(channel);

                var request = new TransferWalletGrpc.PreTransferRequest
                {
                    FromWalletId = fromWalletId,
                    ToWalletId = toWalletId,
                    BankCode = bankCode,
                    Amount = amount,
                    Memo = memo,
                    ComName = AccountManager.GetInstance().Account.comname,
                    UserId = AccountManager.GetInstance().Account.id.ToString(),
                };

                var response = await client.PreTansferWalletAsync(request);

                if (response != null)
                {
                    res = JsonConvert.DeserializeObject<WalletTransactionResponse>(response.ToString());

                }
                else
                {
                    res = null;
                }
            }
            catch
            { }
            return res;
        }

        public async Task<WalletTransactionResponse> Transfer(string transactionToken,string citizenId)
        {
            WalletTransactionResponse res = new();
            try
            {
                //AppContext.SetSwitch("System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
                using var httpClient = new HttpClient(handler);
                var credentials = CallCredentials.FromInterceptor((context, metaData) =>
                {
                    //metaData.Add("Authorization", "Bearer 1234");
                    return Task.CompletedTask;
                });
                using var channel = GrpcChannel.ForAddress(BaseService._baseUrlTransfer, new GrpcChannelOptions()
                {
                    HttpClient = httpClient
                });

                var client = new TransferWalletGrpc.TransferWalletService.TransferWalletServiceClient(channel);

                var request = new TransferWalletGrpc.TransferRequest
                {
                    TransactionToken = transactionToken,
                    CitizenId = citizenId,
                    ComName = AccountManager.GetInstance().Account.comname,
                    UserId = AccountManager.GetInstance().Account.id.ToString(),
                };

                var response = await client.TransferWalletAsync(request);

                if (response != null)
                {
                    res = JsonConvert.DeserializeObject<WalletTransactionResponse>(response.ToString());

                }
                else
                {
                    res = null;
                }
            }
            catch
            { }
            return res;
        }
    }
}
