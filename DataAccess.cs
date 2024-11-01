using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Bake_Shop
{
    public static class DataAccess
    {
        public static List<Product> GetAll()
        {
            var list = new List<Product>();

            {
                using (IDbConnection db = new SqlConnection(ConnectionView.ConnectionString))
                {
                    if (db.State == ConnectionState.Closed)
                    {
                        db.Open();
                       return db.Query<Product>("select P_Code,P_Name,Category,Unit_Price,Qty from ProductInfo", commandType: CommandType.Text).ToList();
                    }
                }
                return list;
            }
        }
    }
}
