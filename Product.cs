using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bake_Shop
{
    public class Product
    {
        public int P_Code { get; set; }
        public string P_Name { get; set;}
        public string Category { get; set; }
        public decimal Unit_Price { get; set; }
        public int Qty { get; set; }
    }
}
