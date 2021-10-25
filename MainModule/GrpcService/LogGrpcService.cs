using Grpc.Core;
using Grpc.Net.Client;
using MainModule.GrpcService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPFScbOri.Models;

namespace MainModule.GrpcService
{
    public class LogGrpcService
    {

        public async void AddActivityLog(int actType,string actDetail, string pageCode, string pageName)
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
                using var channel = GrpcChannel.ForAddress(BaseService._baseUrlTransfer, new GrpcChannelOptions()
                {
                    HttpClient = httpClient
                });

                var client = new LogGrpc.LogGrpc.LogGrpcClient(channel);

                var request = new LogGrpc.LogRequest
                {
                    ActType = actType,
                    ActDetail = actDetail,
                    UserId = AccountManager.GetInstance().Account.id.ToString(),
                    Comname = AccountManager.GetInstance().Account.comname,
                    PageCode = pageCode,
                    PageName = pageName,
                };

                _ = await client.AddActivityLogAsync(request);
            }
            catch
            {}
        }
    }
}
