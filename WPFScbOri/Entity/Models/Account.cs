using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Account
    {
        public long id { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String branchCode { get; set; }
        public String comname { get; set; }
        public String fullname { get; set; }
        public String fullinfo { get; set; }

        public String status { get; set; }

        public Account(long id, String name, String surname, String branchCode, String comname, String status)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.branchCode = branchCode;
            this.comname = comname;
            this.status = status;
        }
    }
}
