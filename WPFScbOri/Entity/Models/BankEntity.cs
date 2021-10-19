using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Entity.Models
{
    public class BankEntity
    {
        private string bankCode;
        private string bankName;
        private string bankNameEN;
        private string bankAbbr;
        private string bankImage;
        private ImageSource imagePath;

        public string BankCode { get => bankCode; set => bankCode = value; }
        public string BankName { get => bankName; set => bankName = value; }
        public string BankNameEN { get => bankNameEN; set => bankNameEN = value; }
        public string BankAbbr { get => bankAbbr; set => bankAbbr = value; }
        public ImageSource ImagePath { get => BitmapFromBase64(BankImage); set => imagePath = value; }
        public string BankImage { get => bankImage; set => bankImage = value; }

        public BitmapSource BitmapFromBase64(string b64string)
        {
            var bytes = Convert.FromBase64String(b64string);

            using (var stream = new MemoryStream(bytes))
            {
                return BitmapFrame.Create(stream,
                    BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }
    }

    public class BankListEntity
    {
        private List<BankEntity> bankList;

        public BankListEntity()
        {
            BankList = new();
        }

        public BankListEntity(BankListEntity bankListEntity)
        {
            BankList = new();
            this.BankList = bankListEntity.BankList;
        }

        public List<BankEntity> BankList { get => bankList; set => bankList = value; }

        
    }
}
