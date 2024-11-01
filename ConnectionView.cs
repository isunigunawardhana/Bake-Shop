using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bake_Shop
{
    public static class ConnectionView
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
    }
}
