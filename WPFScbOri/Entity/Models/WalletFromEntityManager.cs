using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class WalletFromEntityManager
    {
        public WalletEntity WalletEntity;

        private static WalletFromEntityManager instance;

        public static WalletFromEntityManager Instance
            => instance;

        public static WalletFromEntityManager GetInstance()
        {
            if (instance == null)
            {
                instance = new WalletFromEntityManager();
            }
            return instance;
        }
    }
}
