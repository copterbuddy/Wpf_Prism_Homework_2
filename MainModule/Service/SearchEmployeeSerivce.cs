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
    public interface ISearchEmployeeSerivce
    {
        List<StringModel> GetBranchText();

        List<StringModel> GetEmpIdText();

        SearchEmployeeResult SearchEmployee(string empId, string password);
    }

    public class SearchEmployeeSerivce : ISearchEmployeeSerivce
    {
        private string _baseUrl = "http://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9000/api";

        public List<StringModel> GetBranchText()
        {
            List<StringModel> result = new List<StringModel>();

            result.Add(new StringModel()
            {
                Text = "พนักงานในสาขา"
            });

            result.Add(new StringModel()
            {
                Text = "พนักงานต่างสาขา"
            });

            return result;
        }

        public List<StringModel> GetEmpIdText()
        {
            List<StringModel> result = new List<StringModel>();

            result.Add(new StringModel()
            {
                Text = "11358"
            });

            result.Add(new StringModel()
            {
                Text = "12966"
            });

            return result;
        }

        public SearchEmployeeResult SearchEmployee(string empId, string password)
        {
            SearchEmployeeResult result = new SearchEmployeeResult();
            try
            {
                string url = _baseUrl + "/secs";
                string fullurl = url + "/" + empId + "/" + password;
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
