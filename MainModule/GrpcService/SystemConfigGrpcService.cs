using Entity.Models;
using Entity.Models.WalletTransfer;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MainModule.GrpcService
{
    public class SystemConfigGrpcService
    {
        private string _baseUrl = "http://localhost:9090/";

        public async Task<List<BankEntity>> GetBankList()
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

                var client = new systemConfigGrpc.GetBankListService.GetBankListServiceClient(channel);

                var request = new systemConfigGrpc.EmptyRequest{};

                var response = await client.GetBankListAsync(request);
                var stringRes = JsonConvert.SerializeObject(response);

                if (response != null)
                {
                    var res = JsonConvert.DeserializeObject<GetBankListResponse>(response.ToString());
                    return res.bankList;

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
