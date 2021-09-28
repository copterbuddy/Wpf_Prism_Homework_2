using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScbOri.Models;

namespace WPFScbOri.Service
{
    public interface ISellFundMainService
    {
        SearchEmployeeResult CheckIcLicense(string userId);
    }

    public class SellFundMainService : ISellFundMainService
    {
        private string _baseUrl = "http://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:8889/api";

        public SearchEmployeeResult CheckIcLicense(string userId)
        {
            SearchEmployeeResult result = new SearchEmployeeResult();
            try
            {
                string url = _baseUrl + "/secs";
                string fullurl = url + "/" + userId;
                var client = new RestClient(new Uri(fullurl));
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                IRestResponse response = client.Execute(request);
                if (!string.IsNullOrEmpty(response.Content))
                {
                    result = JsonConvert.DeserializeObject<SearchEmployeeResult>(response.Content);
                    result.HttpStatus = response.StatusCode;
                }
                else
                {
                    result.HttpStatus = response.StatusCode;
                }
                return result;
            }
            catch (Exception e)
            {
                result.HttpStatus = System.Net.HttpStatusCode.InternalServerError;
                return result;
            }
        }
    }
}
