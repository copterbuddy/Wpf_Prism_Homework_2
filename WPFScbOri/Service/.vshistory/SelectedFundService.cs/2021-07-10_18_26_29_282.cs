using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Teller.Core.Models;
using WPFScbOri.Models;

namespace WPFScbOri.Service
{
    public interface ISelectedFundService
    {
        List<Fund> GetFundList();
        List<Fund> GetFundList(string CustAccount, string fundGroup, string fundCode);
        List<string> GetFundGroupList();
        List<RedeemAccount> SetAccountOption();
    }

    public class SelectedFundService : ISelectedFundService
    {
        private string _baseUrl = "http://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:8889/";

        public List<RedeemAccount> SetAccountOption()
        {
            try
            {
                var client = new RestClient(new Uri(_baseUrl + "/api/accounts/000015663527"));
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                IRestResponse response = client.Execute(request);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<RedeemAccount> ListAccount = new List<RedeemAccount>();
                    ListAccount = JsonConvert.DeserializeObject<List<RedeemAccount>>(response.Content);
                    return ListAccount;
                }
                else
                {
                    return null;
                }
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        public List<string> GetFundGroupList()
        {
            try
            {
                var client = new RestClient(new Uri(_baseUrl + "api/fundGroups"));
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                IRestResponse response = client.Execute(request);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<string> FundGroupsList = JsonConvert.DeserializeObject<List<string>>(response.Content);
                    FundGroupsList.Insert(0, "ทั้งหมด");
                    return FundGroupsList;
                }
                else
                {
                    return null;
                }
            }
            catch (WebException ex)
            {
                return null;
            }
        }

        public List<Fund> GetFundList()
        {
            try
            {
                IRestResponse response = null;
                RestClient client = null;
                RestRequest request = null;

                client = new RestClient(new Uri(_baseUrl + "/api/funds"));
                request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                response = client.Execute(request);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<Fund> FundList = JsonConvert.DeserializeObject<List<Fund>>(response.Content);
                    return FundList;
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

        public List<Fund> GetFundList(string CustAccount, string fundGroup, string fundCode)
        {
            try
            {
                IRestResponse response = null;
                RestClient client = null;
                RestRequest request = null;

                client = new RestClient(new Uri(_baseUrl + "api/funds"));
                request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                Object param = new { accountNumber = CustAccount, fundGroup = fundGroup, fundCode = fundCode };
                request.AddJsonBody(param);
                response = client.Execute(request);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    List<Fund> FundList = JsonConvert.DeserializeObject<List<Fund>>(response.Content);
                    return FundList;
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
