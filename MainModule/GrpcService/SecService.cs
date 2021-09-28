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
    public class SecService
    {
        private string _baseUrl = "https://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9090/";

        public async Task<SearchEmployeeResult> Sec(string _empId)
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

                var client = new SecGrpc.SecService.SecServiceClient(channel);

                var request = new SecGrpc.CheckIcLicenseRequest
                {
                    EmpID = _empId,
                };

                var response = await client.checkIcLicenseAsync(request);

                if (response != null)
                {
                    var res = JsonConvert.DeserializeObject<SearchEmployeeResult>(response.ToString());
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

        public async Task<SearchEmployeeResult> AuthenticateIcLicense(string _empId,string _password)
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

                var client = new SecGrpc.SecService.SecServiceClient(channel);

                var request = new SecGrpc.AuthenticateIcLicenseRequest
                {
                    EmpID = _empId,
                    Password = _password,
                };

                var response = await client.authenticateIcLicenseAsync(request);

                if (response != null)
                {
                    var res = JsonConvert.DeserializeObject<SearchEmployeeResult>(response.ToString());
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
    }
}
