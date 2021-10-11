using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class WalletEntityManager
    {
        public WalletEntity WalletEntity;

        private static WalletEntityManager instance;

        public static WalletEntityManager Instance
            => instance;

        public static WalletEntityManager GetInstance()
        {
            if (instance == null)
            {
                instance = new WalletEntityManager();
            }
            return instance;
        }
    }
}
