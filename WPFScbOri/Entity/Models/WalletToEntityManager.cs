using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class WalletToEntityManager
    {
        public WalletEntity WalletEntity;

        private static WalletToEntityManager instance;

        public static WalletToEntityManager Instance
            => instance;

        public static WalletToEntityManager GetInstance()
        {
            if (instance == null)
            {
                instance = new WalletToEntityManager();
            }
            return instance;
        }
    }
}
