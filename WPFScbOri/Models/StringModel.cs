using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScbOri.Models
{
    public class StringModel
    {
        #region *** Public Member ***
        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }
        #endregion

        #region *** Private Member ***
        private string _text;
        #endregion
    }
}
