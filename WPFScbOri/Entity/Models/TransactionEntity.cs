using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class TransactionEntity
    {
        private string transCode;
        private string fromWalletId;
        private string fromWalletName;
        private string toWalletId;
        private string toWalletName;
        private decimal amount;
        private decimal fee3Amount;
        private string walletNameTo;
        private string bankCode;//ToWalletBankCode
        private DateTime timeStamp;
        private string memo;
        private string transactionToken;


        
        public string AmountDisplay { 
            get {
                if (Amount.ToString().IndexOf('.') < 0)
                {
                    return Amount.ToString() + ".00";
                }
                else
                {
                    return Amount.ToString();
                }
            }
    }

        public string Fee3AmountDisplay { 
            get {
                if (Fee3Amount.ToString().IndexOf('.') < 0)
                {
                    return Fee3Amount.ToString() + ".00";
                }
                else
                {
                    return Fee3Amount.ToString();
                }
            }
        }
        public decimal TotalAmount
        {
            get
            {
                return Amount + Fee3Amount;
            }
        }
        public string TotalAmountDisplay
        {
            get
            {
                if (Fee3Amount.ToString().IndexOf('.') < 0)
                {
                    return (Amount + Fee3Amount).ToString() + ".00";
                }
                else
                {
                    return Fee3Amount.ToString();
                }
            }
        }
        public string BankName
        {
            get
            {
                var name = BankEntityManager.GetInstance().bankEntity.BankName;
                return name;
            }
        }

        public string DateDisplay
        {
            get
            {
                string response = "";
                int thaiYear = new ThaiBuddhistCalendar().GetYear(TimeStamp);
                int thaiMonth = new ThaiBuddhistCalendar().GetMonth(TimeStamp);
                int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(TimeStamp);
                int thaiHour = new ThaiBuddhistCalendar().GetHour(TimeStamp);
                int thaiMinute = new ThaiBuddhistCalendar().GetMinute(TimeStamp);
                int thaiSecond = new ThaiBuddhistCalendar().GetSecond(TimeStamp);

                response = thaiDay.ToString() + " " + GetFullMonthName(thaiMonth) + " " + thaiYear.ToString();

                return response;
            }
        }

        public string TimeDisplay
        {
            get
            {
                string response = "";

                int thaiYear = new ThaiBuddhistCalendar().GetYear(TimeStamp);
                int thaiMonth = new ThaiBuddhistCalendar().GetMonth(TimeStamp);
                int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(TimeStamp);
                int thaiHour = new ThaiBuddhistCalendar().GetHour(TimeStamp);
                int thaiMinute = new ThaiBuddhistCalendar().GetMinute(TimeStamp);
                int thaiSecond = new ThaiBuddhistCalendar().GetSecond(TimeStamp);

                response = thaiHour.ToString() + ":" + thaiMinute.ToString() + " Bangkok, Thailand (GMT +7:00)";
                return response;
            }
        }

        public string DateDisplayWithShortMonth
        {
            get
            {
                string response = "";
                int thaiYear = new ThaiBuddhistCalendar().GetYear(TimeStamp);
                int thaiMonth = new ThaiBuddhistCalendar().GetMonth(TimeStamp);
                int thaiDay = new ThaiBuddhistCalendar().GetDayOfMonth(TimeStamp);
                int thaiHour = new ThaiBuddhistCalendar().GetHour(TimeStamp);
                int thaiMinute = new ThaiBuddhistCalendar().GetMinute(TimeStamp);
                int thaiSecond = new ThaiBuddhistCalendar().GetSecond(TimeStamp);

                response = thaiDay.ToString() + " " + GetShortMonthName(thaiMonth) + " " + thaiYear.ToString();

                return response;
            }
        }

        private string GetFullMonthName(int month)
        {
            string res = "";
            switch (month)
            {
                case 1:
                    res = "มกราคม";
                    break;
                case 2:
                    res = "กุมภาพันธ์";
                    break;
                case 3:
                    res = "มีนาคม";
                    break;
                case 4:
                    res = "เมษายน";
                    break;
                case 5:
                    res = "พฤษภาคม";
                    break;
                case 6:
                    res = "มิถุนายน";
                    break;
                case 7:
                    res = "กรกฎาคม";
                    break;
                case 8:
                    res = "สิงหาคม";
                    break;
                case 9:
                    res = "กันยายน";
                    break;
                case 10:
                    res = "ตุลาคม";
                    break;
                case 11:
                    res = "พฤศจิกายน";
                    break;
                case 12:
                    res = "ธันวาคม";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        private string GetShortMonthName(int month)
        {
            string res = "";
            switch (month)
            {
                case 1:
                    res = "ม.ค.";
                    break;
                case 2:
                    res = "ก.พ.";
                    break;
                case 3:
                    res = "มี.ค.";
                    break;
                case 4:
                    res = "เม.ย.";
                    break;
                case 5:
                    res = "พ.ค.";
                    break;
                case 6:
                    res = "มี.ค.";
                    break;
                case 7:
                    res = "ก.ค.";
                    break;
                case 8:
                    res = "ส.ค.";
                    break;
                case 9:
                    res = "ก.ย.";
                    break;
                case 10:
                    res = "ต.ค.";
                    break;
                case 11:
                    res = "พ.ย.";
                    break;
                case 12:
                    res = "ธ.ค.";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        public string TransCode { get => transCode; set => transCode = value; }
        public string FromWalletId { get => fromWalletId; set => fromWalletId = value; }
        public string FromWalletName { get => fromWalletName; set => fromWalletName = value; }
        public string ToWalletId { get => toWalletId; set => toWalletId = value; }
        public string ToWalletName { get => toWalletName; set => toWalletName = value; }
        public decimal Amount { get => amount; set => amount = value; }
        public decimal Fee3Amount { get => fee3Amount; set => fee3Amount = value; }
        public string WalletNameTo { get => walletNameTo; set => walletNameTo = value; }
        public string BankCode { get => bankCode; set => bankCode = value; }
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
        public string Memo { get => memo; set => memo = value; }
        public string TransactionToken { get => transactionToken; set => transactionToken = value; }

    }
}
