using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Teller.Core.Models;
using WPFScbOri.Models;

namespace MainModule.GrpcService
{
    public class FundService
    {
        private string _baseUrl = "https://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9090/";

        public async Task<List<Fund>> Funds()
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

                var client = new FundGrpc.FundService.FundServiceClient(channel);

                var request = new FundGrpc.EmptyRequest
                {
                    
                };

                var response = await client.fundsAsync(request);
                var result = response.ToString();

                if (response != null)
                {
                    var tempRes = JsonConvert.DeserializeObject<FundList>(result);
                    var res = tempRes.Funds;
                    return res;

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

        public async Task<List<Fund>> ListFundsByAccountNumber(string _accountNumber,string _fundGroup,string _fundCode)
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

                var client = new FundGrpc.FundService.FundServiceClient(channel);

                var request = new FundGrpc.FundListRequest
                {
                    AccountNumber = _accountNumber,
                    FundGroup = _fundGroup,
                    FundCode = _fundCode,
                };

                var response = await client.listFundsByAccountNumberAsync(request);

                if (response != null)
                {
                    var res = JsonConvert.DeserializeObject<FundList>(response.ToString());
                    return res.Funds;

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

        public async Task<List<string>> ListFundGroups()
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

                var client = new FundGrpc.FundService.FundServiceClient(channel);

                var request = new FundGrpc.EmptyRequest
                {
                };

                var response = await client.listFundGroupsAsync(request);

                if (response != null)
                {
                    var res = JsonConvert.DeserializeObject<ListString>(response.ToString());
                    List<string> result = res.ListFundGroups;
                    result.Insert(0, "ทั้งหมด");
                    return result;

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
        public class ListString
        {
            public List<string> ListFundGroups { get; set; }
        }
    }
}
