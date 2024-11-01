using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bake_Shop
{
    public class SoldList
    {
        public int Trans_Num { get; set; }
        public int Pro_Code { get; set; }
        public string Pro_Name { get; set; }
        public string Pro_Category { get; set; }
        public int Pro_Qty { get; set; }
        public decimal Pro_Price { get; set; }
        public DateTime Trans_Date { get; set; }
    }
}
