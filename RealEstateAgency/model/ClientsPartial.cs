using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.model
{
    partial class Clients
    {
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName} {MiddleName}";
            }
        }
        public string NormalFormatPhoneNumber
        {
            get
            {
                string phone = Phone.ToString();
                if (Phone != null)
                {
                    phone = $"+{phone[0]}({phone[1]}{phone[2]}{phone[3]}){phone[4]}{phone[5]}{phone[6]}-{phone[7]}{phone[8]}-{phone[9]}{phone[10]}";
                }
                return phone;
            }
        }
    }
}
