using System.Data.SqlClient;

namespace Project.DBUtils {
    public class DBUtils {
        public SqlConnection MakeConnection() {
            var con = new SqlConnection(@"Persist Security Info=true;"   //
                                        + @"Integrated Security=true;"   //
                                        + @"Initial Catalog=QLSVien;"    //create connection to database
                                        + @"server=localhost;"           //
                                        + @"Connection Timeout=3");      //
            return con;
        }
    }
}
