using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.WalletTransfer
{
    public class CustomerEntity
    {
        string custId;
        string citizenId;
        string branch;
        string title;
        string name;
        string lastname;
        string segmant;
        bool jointAccountStatus;
        bool sensitiveAccount;
        string citizenImage;
        string signImage;

        public string CustId { get => custId; set => custId = value; }
        public string CitizenId { get => citizenId; set => citizenId = value; }
        public string Branch { get => branch; set => branch = value; }
        public string Title { get => title; set => title = value; }
        public string Name { get => name; set => name = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Segmant { get => segmant; set => segmant = value; }
        public bool JointAccountStatus { get => jointAccountStatus; set => jointAccountStatus = value; }
        public bool SensitiveAccount { get => sensitiveAccount; set => sensitiveAccount = value; }
        public string CitizenImage { get => citizenImage; set => citizenImage = value; }
        public string SignImage { get => signImage; set => signImage = value; }
    }
}
