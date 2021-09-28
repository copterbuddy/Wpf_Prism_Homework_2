using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teller.Core.Models;
using WPFScbOri.Models;

namespace Teller.SellFund.Models
{
    public class SellFundTableModel
    {
        #region *** Public Member ***
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public decimal? Volume
        {
            get { return this._volume; }
            set { this._volume = value; }
        }

        public decimal? Price
        {
            get { return this._price; }
            set { this._price = value; }
        }

        public decimal PrintVolume
        {
            get { return this._printVolume; }
            set { this._printVolume = value; }
        }

        public string VolumeText
        {
            get { return this._volumeText; }
            set { this._volumeText = value; }
        }

        public decimal? ErrorVolume
        {
            get { return this._errorVolume; }
            set { this._errorVolume = value; }
        }

        public string Status
        {
            get
            {
                if (string.IsNullOrEmpty(Id))
                {
                    return string.Empty;
                }
                else
                {
                    if (Convert.ToDecimal(ErrorVolume) <= 0 && ErrorType == ErrorVolumeType.Ok)
                    {
                        return "ปกติ";
                    }
                    else
                    {
                        return "ต้องอนุมัติ";
                    }
                }
            }
        }

        public Fund Fund
        {
            get { return this._fund; }
            set { this._fund = value; }
        }

        public SellFundFact SellFundFact
        {
            get { return this._sellFundFact; }
            set { this._sellFundFact = value; }
        }

        public ErrorVolumeType ErrorType
        {
            get { return this._errorType; }
            set { this._errorType = value; }
        }

        public SaleType SaleType
        {
            get { return this._saleType; }
            set { this._saleType = value; }
        }

        public int Index
        {
            get { return this._index; }
            set { this._index = value; }
        }

        public bool IsButtonTable
        {
            get { return this._isButtonTable; }
            set { this._isButtonTable = value; }
        }
        #endregion

        #region *** Private Member ***
        private string _id;

        private string _name;

        private decimal? _volume;

        private decimal? _price;

        private decimal _printVolume;

        private string _volumeText;

        private decimal? _errorVolume;

        private Fund _fund;

        private SellFundFact _sellFundFact;

        private ErrorVolumeType _errorType;

        private SaleType _saleType;

        private int _index;

        private bool _isButtonTable;
        #endregion
    }
}
