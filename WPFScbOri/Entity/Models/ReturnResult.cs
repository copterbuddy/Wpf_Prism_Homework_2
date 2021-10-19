using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class ReturnResult
    {
        private string result;
        private string resultCode;
        private string resultDescription;
        private string errorRefId;
        private DateTime resultTimestamp;

        public string Result { get => result; set => result = value; }
        public string ResultCode { get => resultCode; set => resultCode = value; }
        public string ResultDescription { get => resultDescription; set => resultDescription = value; }
        public string ErrorRefId { get => errorRefId; set => errorRefId = value; }
        public DateTime ResultTimestamp { get => resultTimestamp; set => resultTimestamp = value; }
    }
}
