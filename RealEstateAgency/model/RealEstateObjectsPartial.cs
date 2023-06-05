using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.model
{
    partial class RealEstateObjects
    {
        public string TypeAndAddress
        {
            get
            {
                return $"{TypeRealEstateObjects.Title}: {Address.Title}";
            }
        }
        public string Type
        {
            get
            {
                return $"{TypeRealEstateObjects.Title}";
            }
        }
    }
}
