using Entity.Models.WalletTransfer;
using Grpc.Core;
using Grpc.Net.Client;
using MainModule.GrpcService.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
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

        public async Task<List<CustomerDetail>> SeachCustomerTransfer(string _searchType, string _searchValue)
        {
            #region Mock Data
            //List<CustomerDetail> listCust = new List<CustomerDetail>();

            //{
            //    CustomerDetail cust = new()
            //    {
            //        CustId = "000015663527",
            //        CitizenID = "3100202827767",
            //        Branch = "0014",
            //        AccName = "นายทดสอบ โปรแกรม",
            //        Age = "40",
            //        Payment = "ลงนามผู้เดียวเทส",
            //        CitizenIdCardImagePath = null,
            //        SignedSignatureImagePath = null,
            //    };
            //    listCust.Add(cust);
            //}

            //{
            //    CustomerDetail cust = new()
            //    {
            //        CustId = "000015663527",
            //        CitizenID = "3100202827767",
            //        Branch = "0014",
            //        AccName = "นายทดสอบ โปรแกรม",
            //        Age = "40",
            //        Payment = "ลงนามผู้เดียวเทส",
            //        CitizenIdCardImagePath = null,
            //        SignedSignatureImagePath = null,
            //    };
            //    listCust.Add(cust);
            //}

            //return listCust;
            #endregion

            #region Prod
            try
            {
                if (_searchType == null || _searchValue == null)
                {
                    return new List<CustomerDetail>();
                }

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

                var client = new CustomerWalletGrpc.CustomerWalletService.CustomerWalletServiceClient(channel);

                var request = new CustomerWalletGrpc.SearchCustomerRequest
                {
                    SearchType = GetSearchType(_searchType),
                    SearchText = _searchValue,
                    ComName = AccountManager.GetInstance().Account.comname,
                    UserId = AccountManager.GetInstance().Account.id.ToString(),
                };

                var response = await client.SearchCustomerAsync(request);

                if (response != null)
                {
                    SearchCustomerResponse searchCustomerResponse = JsonConvert.DeserializeObject<SearchCustomerResponse>(response.ToString());

                    List<CustomerDetail> listCust = new();

                    if (searchCustomerResponse != null && searchCustomerResponse.customerEntity != null && searchCustomerResponse.customerEntity.Count > 0)
                    {
                        foreach (var item in searchCustomerResponse.customerEntity)
                        {
                            CustomerDetail customer = new();
                            customer.CustId = item.CustId;
                            customer.CitizenID = item.CitizenId;
                            customer.Branch = item.Branch;
                            customer.AccName = item.Title + " " + item.Name + " " + item.Lastname;
                            customer.Segment = item.Segmant;
                            customer.JointAccount = item.JointAccountStatus;
                            customer.Sensitive = item.SensitiveAccount;
                            customer.IdCardPath = item.CitizenImage;
                            customer.SignaturePath = item.SignImage;
                            customer.MobileNo = item.MobileNo;
                            customer.Address = item.Address;

                            if (!string.IsNullOrEmpty(customer.IdCardPath))
                            {
                                customer.CitizenIdCardImagePath = BitmapFromBase64(customer.IdCardPath);
                            }

                            if (!string.IsNullOrEmpty(customer.SignaturePath))
                            {
                                customer.SignedSignatureImagePath = BitmapFromBase64(customer.SignaturePath);
                            }

                            listCust.Add(customer);
                        }
                    }

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
            #endregion

        }

        public string GetSearchType(string _searchType)
        {
            string searchType = "";
            switch (_searchType)
            {
                case "บัตรประชาชน":
                    searchType = "1";
                    break;
                case "พาสปอร์ต":
                    searchType = "2";
                    break;
                case "ชื่อ - นามสกุล":
                    searchType = "3";
                    break;
                default:
                    break;
            }
            return searchType;
        }

        public BitmapSource BitmapFromBase64(string b64string)
        {
            var bytes = Convert.FromBase64String(b64string);

            using (var stream = new MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream,
                    BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }


    }
}
