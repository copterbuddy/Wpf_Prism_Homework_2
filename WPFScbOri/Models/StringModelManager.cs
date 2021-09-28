using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScbOri.Models
{
    public class StringModelManager
    {
        public StringModel Text;

        private static StringModelManager instance;

        public static StringModelManager Instance
            => instance;

        public static StringModelManager GetInstance()
        {
            if (instance == null)
            {
                instance = new StringModelManager();
            }
            return instance;
        }
    }
}
