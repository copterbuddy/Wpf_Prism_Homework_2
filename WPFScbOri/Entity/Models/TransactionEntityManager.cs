using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class TransactionEntityManager
    {
        public TransactionEntity TransactionEntity;

        private static TransactionEntityManager instance;

        public static TransactionEntityManager Instance
            => instance;

        public static TransactionEntityManager GetInstance()
        {
            if (instance == null)
            {
                instance = new TransactionEntityManager();
            }
            return instance;
        }
    }
}
