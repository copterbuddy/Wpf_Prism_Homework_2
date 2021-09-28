using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teller.Core.Models
{
    public class Fund
    {
        public String fundGroup { get; set; }
        public String fundGroupName { get; set; }
        public String fundCode { get; set; }
        public String fundName { get; set; }
        public bool isComplex { get; set; }
        public Decimal nav { get; set; }
        public Decimal gainAmount { get; set; }
        public Decimal gainPercentage { get; set; }
        public Decimal totalUnit { get; set; } //จำนวนหน่วยทั้งหมดที่มี
        public Decimal totalBaht { get; set; } //จำนวนเงินทั้งหมดที่มี
        public Decimal minimumSellableUnit { get; set; } //จำนวนหน่วยขั้นต่ำทั้งหมดที่ขายได้
        public Decimal minimumSellableBaht { get; set; } //จำนวนเงินขั้นต่ำทั้งหมดที่ขายได้
        public Decimal minimumUnit { get; set; } //จำนวนหน่วยคงเหลือขั้นต่ำทั้งหมด
        public Decimal minimumBaht { get; set; } //จำนวนเงินคงเหลือขั้นต่ำทั้งหมด
        public Decimal allowRedeem { get; set; }
        public String allowRedeemDisplay { get; set; }
        public Decimal percentageFee { get; set; }
        public bool profitStatus { get; set; }
        public String totalGain { get; set; }
        public Fund() { }

        public Fund(String fundGroup, String fundGroupName, String fundCode, String fundName, bool isComplex, Decimal nav, Decimal gainAmount, Decimal gainPercentage,
    Decimal totalUnit, Decimal totalBaht, Decimal minimumSellableUnit, Decimal minimumSellableBaht, Decimal minimumUnit, Decimal minimumBaht,
    Decimal allowRedeem, String allowRedeemDisplay, Decimal percentageFee, bool profitStatus, String totalGain)
        {
            this.fundGroup = fundGroup;
            this.fundGroupName = fundGroupName;
            this.fundCode = fundCode;
            this.fundName = fundName;
            this.isComplex = isComplex;
            this.nav = nav;
            this.gainAmount = gainAmount;
            this.gainPercentage = gainPercentage;
            this.totalUnit = totalUnit;
            this.totalBaht = totalBaht;
            this.minimumSellableUnit = minimumSellableUnit;
            this.minimumSellableBaht = minimumSellableBaht;
            this.minimumUnit = minimumUnit;
            this.minimumBaht = minimumBaht;
            this.allowRedeem = allowRedeem;
            this.allowRedeemDisplay = allowRedeemDisplay;
            this.percentageFee = percentageFee;
            this.profitStatus = profitStatus;
            this.totalGain = totalGain;
        }
    }

    public class FundList
    {
        public List<Fund> Funds { get; set; }
    }
}
