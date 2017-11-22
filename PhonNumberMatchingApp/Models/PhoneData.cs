using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhonNumberMatchingApp.Models
{
    public class PhoneData
    {
        public string PhoneNumber { get; set; }
        public string PhoneCode { get; set; }
        public string NetworkName { get; set; }
        public string CircleName { get; set; }
        public string City { get; set; }
        

    }
    public class PhoneCode
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        

    }

}