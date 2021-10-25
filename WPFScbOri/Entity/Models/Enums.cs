using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Enums
    {
        public enum CurrentMainRegion
        {
            None = 0,
            WelcomeRegion = 1,
            OtherServiceRegion = 2,
            TransferRegion = 3,

        }

        public enum ActivityType
        {
            SearchCustomer = 1,
            SelectCustomer = 2,
            CheckSign = 3,
            PreTransfer = 4,
            CompleteTransfer = 5,
        }

        public enum PageCode
        {
            PAGE001 = 1,
            PAGE002 = 2,
            PAGE003 = 3,
        }
        public enum PageName
        {
            TRANSFER_PAGE = 1,
            PRETRANSFER_PAGE = 2,
            COMPLETETRANSFER_PAGE = 3,
        }
    }
}
