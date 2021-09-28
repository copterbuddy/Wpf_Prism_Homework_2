using Entity.Models;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPFScbOri.Models;
using static WPFScbOri.Models.RedeemAccount;

namespace MainModule.GrpcService
{
    public class AccountService
    {
        private string _baseUrl = "https://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9090/";

        public async Task<List<RedeemAccount>> ListRedeemAccount(string _accountNo)
        {
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
                using var channel = GrpcChannel.ForAddress(_baseUrl, new GrpcChannelOptions()
                {
                    HttpClient = httpClient
                });

                var client = new AccountGrpc.AccountService.AccountServiceClient(channel);

                var request = new AccountGrpc.ListRedeemAccountRequest
                {
                    AccountNo = _accountNo,
                };

                var response = await client.listRedeemAccountsAsync(request);

                if (response != null)
                {
                    var res = JsonConvert.DeserializeObject<ListRedeemAccount>(response.ToString());
                    return res.Accounts;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
