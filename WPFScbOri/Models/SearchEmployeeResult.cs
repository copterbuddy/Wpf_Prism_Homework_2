using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPFScbOri.Models
{
    public class SearchEmployeeResult
    {
        #region *** Public Member ***
        public bool BranchEmp
        {
            get { return this._branchEmp; }
            set { this._branchEmp = value; }
        }

        public string EmpId
        {
            get { return this._empId; }
            set { this._empId = value; }
        }

        public string SecId
        {
            get { return this._secId; }
            set { this._secId = value; }
        }

        public bool IcLicense
        {
            get { return this._icLicense; }
            set { this._icLicense = value; }
        }

        public string IcType
        {
            get { return this._icType; }
            set { this._icType = value; }
        }

        public string RmId
        {
            get { return this._rmId; }
            set { this._rmId = value; }
        }

        public string EmpFirstName
        {
            get { return this._empFirstName; }
            set { this._empFirstName = value; }
        }

        public string EmpLastName
        {
            get { return this._empLastName; }
            set { this._empLastName = value; }
        }

        public List<string> ListObjective
        {
            get { return this._listObjective; }
            set { this._listObjective = value; }
        }

        public HttpStatusCode HttpStatus
        {
            get { return this._httpStatus; }
            set { this._httpStatus = value; }
        }
        #endregion

        #region *** Private Member ***
        private bool _branchEmp;

        private string _empId;

        private string _secId;

        private bool _icLicense;

        private string _icType;

        private string _rmId;

        private string _empFirstName;

        private string _empLastName;

        private List<string> _listObjective;

        public HttpStatusCode _httpStatus;
        #endregion
    }
}
