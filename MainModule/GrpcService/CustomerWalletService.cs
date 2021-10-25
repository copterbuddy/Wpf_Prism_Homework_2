using Entity.Models;
using Entity.Models.WalletTransfer;
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

namespace MainModule.GrpcService
{
    public class CustomerWalletService
    {
        public async Task<WalletListEntity> GetFromWallet(string custId)
        {
            WalletListEntity res = new();
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

                var client = new CustomerWalletGrpc.CustomerWalletService.CustomerWalletServiceClient(channel);

                var request = new CustomerWalletGrpc.GetFromWalletRequest
                {
                    CustId = custId,
                };

                var response = await client.GetFromWalletAsync(request);

                if (response != null)
                {
                    res = JsonConvert.DeserializeObject<WalletListEntity>(response.ToString());

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

        public async Task<string> GetToWallet(string walletId)
        {
            string res = "";
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

                var client = new CustomerWalletGrpc.CustomerWalletService.CustomerWalletServiceClient(channel);

                var request = new CustomerWalletGrpc.GetToWalletRequest
                {
                    WalletId = walletId,
                };

                var response = await client.GetToWalletAsync(request);

                if (response != null)
                {
                    GetToWalletResponse getToWalletResponse = new();
                    getToWalletResponse = JsonConvert.DeserializeObject<GetToWalletResponse>(response.ToString());
                    res = getToWalletResponse.WalletName;

                }
                else
                {
                    res = "";
                }
            }
            catch
            { }
            return res;
        }
    }
}
