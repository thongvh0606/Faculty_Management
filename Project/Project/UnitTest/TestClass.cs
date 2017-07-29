using System.Linq;
using NUnit.Framework;
using Project.TblFaculty;

namespace Project.UnitTest {
    [TestFixture]
    public class TestClass {
        [TestCase("(1 row(s) affected).\n", "INSERT INTO KHOA(MAKHOA,TENKHOA) VALUES('1','JS')", TestName = "1")]
        [TestCase("(1 row(s) affected).\n", "UPDATE KHOA SET TENKHOA='JS' WHERE MAKHOA='1'", TestName = "2")]
        [TestCase("(1 row(s) affected).\n", "DELETE FROM KHOA WHERE MAKHOA='1'", TestName = "3")]
        [TestCase("duplicate", "INSERT INTO KHOA(MAKHOA,TENKHOA) VALUES('1','JS')", "INSERT INTO KHOA(MAKHOA,TENKHOA) VALUES('1','JS')", TestName = "4")]
        [TestCase("(1 row(s) affected).\n", "DELETE FROM KHOA WHERE MAKHOA='1'", TestName = "5")]
        public void Execute_SQL_Queries_Return_Error(string expected, params string[] queries) {
            StringAssert.Contains(expected, new FacultyDao().InsertDeleteUpdate(queries.ToList()));
        }


    }
}
