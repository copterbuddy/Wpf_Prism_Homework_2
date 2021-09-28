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

namespace MainModule.GrpcService
{
    public class CustomerService
    {
        private string _baseUrl = "https://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9090/";

        public async Task<List<CustomerDetail>> SeachCustomer(string _searchType, string _searchValue)
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

                var client = new CustomerGrpc.CustomerService.CustomerServiceClient(channel);

                var request = new CustomerGrpc.ListCustomerInfoRequest
                {
                    SearchType = _searchType,
                    SearchValue = _searchValue,
                };

                var response = await client.listCustomerInfoAsync(request);

                if (response != null)
                {
                    CustomerDetail res = JsonConvert.DeserializeObject<CustomerDetail>(response.ToString());

                    List<CustomerDetail> listCust = new();
                    listCust.Add(res);
                    return listCust;

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
