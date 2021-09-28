using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFScbOri.Models
{
    public class CustomerDetail
    {
        public string CustId { get; set; }
        public string Branch { get; set; }
        public bool Ltf { get; set; }
        public string IsLTF => Ltf ? "Y" : "N";
        public string AccName { get; set; }
        public string Age { get; set; }
        public string Segment { get; set; }
        public bool JointAccount { get; set; }
        public string IsJointAccount => JointAccount ? "Y" : "N";
        public bool Sensitive { get; set; }
        public string IsSensitive => Sensitive ? "Y" : "N";
        public string Payment { get; set; }
        public string PassportID { get; set; }
        public string JuristicID { get; set; }
        public string CitizenID { get; set; }
        public string IdCardPath { get; set; }
        public string SignaturePath { get; set; }

        public ImageSource CitizenIdCardImagePath { get; set; }
        public ImageSource SignedSignatureImagePath { get; set; }

        bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        int height;
        public int Height
        {
            get => height;
            set => height = value;
        }

        public ICommand ToggleInnerInformationCommand { get; set; }
        public ICommand SelectItemCommand { get; set; }
    }
}
