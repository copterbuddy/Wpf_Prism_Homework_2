using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScbOri.Models
{
    public enum SaleType
    {
        None,
        Unit,
        Baht
    }

    public enum ErrorVolumeType
    {
        None = 0,
        Ok = 1,
        AccountError = 2,
        LTFError = 3,
        SSFError = 4,
        RMFError = 5,
        AccountAndLTFError = 6,
        AccountAndSSFError = 7,
        AccountAndRMFError = 8
    }
}
