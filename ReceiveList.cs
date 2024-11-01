using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bake_Shop
{
    public class ReceiveList
    {
        public int TransNo { get; set; }
        public DateTime Transaction_Date { get; set; }
        public int Pcode { get; set; }
        public string Pname { get; set; }
        public decimal Pprice { get; set; }

        public int R_qty { get; set; }
        public decimal R_price { get; set; }    
        public decimal Tot_Price { get; set; }
        public string Descri { get; set; }
        
    }
}

