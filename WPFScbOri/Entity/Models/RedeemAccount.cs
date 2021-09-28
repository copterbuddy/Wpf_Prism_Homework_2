using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScbOri.Models
{
    public enum AccStatus
    {
        ACTIVE,
        INACTIVE
    }
    public class RedeemAccount
    {
        public String id { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String branchCode { get; set; }
        public String comname { get; set; }
        public AccStatus status { get; set; }

        public RedeemAccount(String id, String name, String surname, String branchCode, String comname, AccStatus status)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.branchCode = branchCode;
            this.comname = comname;
            this.status = status;
        }
        public class ListRedeemAccount
        {
            public List<RedeemAccount> Accounts { get; set; }
        }
    }
}
