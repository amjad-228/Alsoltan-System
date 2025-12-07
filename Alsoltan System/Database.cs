using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsoltan_System
{
    public static class Database
    {
        public static string ConnectionString = @"Server=DESKTOP-BIPH3TI;Database=Alsoltan;Trusted_Connection=true;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
