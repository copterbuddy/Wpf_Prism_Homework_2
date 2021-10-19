using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class BankEntityManager
    {
        public BankEntity bankEntity;

        private static BankEntityManager instance;

        public static BankEntityManager Instance
            => instance;

        public static BankEntityManager GetInstance()
        {
            if (instance == null)
            {
                instance = new BankEntityManager();
            }
            return instance;
        }
    }
}
