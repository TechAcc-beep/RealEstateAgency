using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.model
{
    partial class Agents
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
                return $"+{phone[0]}({phone[1]}{phone[2]}{phone[3]}){phone[4]}{phone[5]}{phone[6]}-{phone[7]}{phone[8]}-{phone[9]}{phone[10]}";
            }
        }
    }
}
