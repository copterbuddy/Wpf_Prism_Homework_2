using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFScbOri.Models;

namespace WPFScbOri.Service
{
    public interface ISearchCustomerService
    {
        ObservableCollection<CustomerDetail> SeachCustomer(string _searchType, string _searchValue);
    }

    public class SearchCustomerService : ISearchCustomerService
    {
        string endpoint = "http://gf-uat-service-lb-716780687.ap-southeast-1.elb.amazonaws.com:9000";

        public ObservableCollection<CustomerDetail> SeachCustomer(string _searchType, string _searchValue)
        {
            ObservableCollection<CustomerDetail> customers = new ObservableCollection<CustomerDetail>();
            try
            {
                var client = new RestClient(new Uri($"{endpoint}/api/customers"));
                var request = new RestRequest(Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                object param = new { searchType = _searchType, searchValue = _searchValue };
                request.AddJsonBody(param);
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var customer = JsonConvert.DeserializeObject<CustomerDetail>(response.Content);

                    if (customer != null)
                    {
                        customer.CitizenIdCardImagePath = GetImageSource("citizenId_image");
                        customer.SignedSignatureImagePath = GetImageSource("sign_image");
                        customers.Add(customer);
                    }
                }
                return customers;

            }
            catch (Exception ex)
            {
                return customers;
            }
        }

        public ObservableCollection<CustomerDetail> SeachCustomer_Mockup(string _searchType, string _searchValue)
        {
            return new ObservableCollection<CustomerDetail>()
            {
                new CustomerDetail
                {
                    AccName = "นายวีรชัย ทองศิริ หรือนางสุรัตน์ ทองศิริ",
                    Age = "35",
                    Branch = "0025",
                    CitizenID = "3102300299147",
                    Segment = "",
                    CustId = "000015663527",
                    Ltf = true,
                    Sensitive = false,
                    Payment = "ลงนามผู้เดียว",
                    JuristicID = "",
                    CitizenIdCardImagePath = GetImageSource("citizenId_image"),
                    SignedSignatureImagePath = GetImageSource("sign_image"),
                }
            };
        }

        private ImageSource GetImageSource(string filename)
        {
            try
            {
                return new BitmapImage(new Uri($"pack://application:,,,/WPFScbOri;Component/Images/{filename}.png"));
            }
            catch
            {
                return null;
            }
        }
    }
}
