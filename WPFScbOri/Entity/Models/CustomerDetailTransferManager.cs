using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScbOri.Models;

namespace Entity.Models
{
    public class CustomerDetailTransferManager
    {
        public CustomerDetail customerDetail;

        private static CustomerDetailTransferManager instance;

        public static CustomerDetailTransferManager Instance
            => instance;

        public static CustomerDetailTransferManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CustomerDetailTransferManager();
            }
            return instance;
        }
    }
}
