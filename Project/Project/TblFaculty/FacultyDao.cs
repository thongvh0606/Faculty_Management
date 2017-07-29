using System.Collections.Generic;
using System.Data.SqlClient;

namespace Project.TblFaculty {
    public class FacultyDao {
        public List<Faculty> GetAllData() {
            var con = new DBUtils.DBUtils().MakeConnection();
            try {
                con.Open(); //open connection before executing query
                var cmd = new SqlCommand("SELECT MAKHOA,TENKHOA FROM KHOA", con);//create sql command
                var rd = cmd.ExecuteReader();   //execute query
                var list = new List<Faculty>();
                while (rd.Read()) {                                                 //
                    list.Add(new Faculty(rd.GetString(rd.GetOrdinal("MAKHOA")),     //add a row value into list 
                        rd.GetString(rd.GetOrdinal("TENKHOA"))));                   //
                }                                                                   //
                con.Close();//close connection after the query has been executed
                return list;
            } catch (SqlException) {
                return null;
            }
        }

        public string InsertDeleteUpdate(List<string> queries) {
            var con = new DBUtils.DBUtils().MakeConnection();
            try {
                var log = "";
                con.Open(); //open connection before executing queries
                foreach (var query in queries) {
                    try {
                        var cmd = new SqlCommand(query, con);//create sql command
                        cmd.ExecuteNonQuery();  //execute query
                    } catch (SqlException ex) {
                        log += ex.Message + "\n";
                    }
                }
                con.Close();//close connection after all queries have been executed
                return log;
            } catch (SqlException e) {
                return e.Message;
            }
        }


    }
}
