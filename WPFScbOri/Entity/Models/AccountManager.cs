using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScbOri.Models
{
    public class AccountManager
    {
        public Account Account;

        private static AccountManager instance;

        public static AccountManager Instance
            => instance;

        public static AccountManager GetInstance()
        {
            if (instance == null)
            {
                instance = new AccountManager();
            }
            return instance;
        }
    }
}
