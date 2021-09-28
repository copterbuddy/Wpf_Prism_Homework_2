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
using Teller.Core.Models;
using WPFScbOri.Models;

namespace LoginModule.GrpcService
{
    class APIServices
    {
        private string _baseUrl = "https://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9090/";

        //public async Task<List<Account>> GetAccountList()
        //{
        //    try
        //    {
        //        //AppContext.SetSwitch("System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);
        //        var handler = new HttpClientHandler()
        //        {
        //            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
        //        };
        //        using var httpClient = new HttpClient(handler);
        //        var credentials = CallCredentials.FromInterceptor((context, metaData) =>
        //        {
        //            //metaData.Add("Authorization", "Bearer 1234");
        //            return Task.CompletedTask;
        //        });
        //        using var channel = GrpcChannel.ForAddress(_baseUrl, new GrpcChannelOptions()
        //        {
        //            HttpClient = httpClient
        //        });

        //        var client = new AccountGrpc.AccountService.AccountServiceClient(channel);

        //        var request = new AccountGrpc.ListRedeemAccountRequest
        //        {
        //        };

        //        var response = await client.listRedeemAccountsAsync(request);

        //        if (response.Accounts != null)
        //        {
        //            List<Account> FundList = JsonConvert.DeserializeObject<List<Account>>(response.Accounts.ToString());
        //            foreach (var item in FundList)
        //            {
        //                if (String.IsNullOrEmpty(item.name)) item.name = "ชื่อว่างเปล่า";
        //                if (String.IsNullOrEmpty(item.surname)) item.surname = "นามสกุลว่างเปล่า";
        //                if (String.IsNullOrEmpty(item.branchCode)) item.branchCode = "99999";
        //            }
        //            return FundList;

        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}

    }
}
