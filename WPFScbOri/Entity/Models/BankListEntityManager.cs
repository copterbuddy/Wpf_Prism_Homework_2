using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class BankListEntityManager
    {
        public BankListEntity bankListEntity;

        private static BankListEntityManager instance;

        public static BankListEntityManager Instance
            => instance;

        public static BankListEntityManager GetInstance()
        {
            if (instance == null)
            {
                instance = new BankListEntityManager();
                
            }
            if (instance.bankListEntity == null)
            {
                instance.bankListEntity = new();
            }
            return instance;
        }
    }
}
