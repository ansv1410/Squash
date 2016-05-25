using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using Squash.Classes;

namespace Squash.Classes
{
    public class LoggedInPerson
    {
        public Users user { get; set; }
        public Members member { get; set; }
        public Logins logins { get; set; }
        public Companies company { get; set; }
        public MemberFloatable memberfloatable { get; set; }
        public MemberFloatable IsMF()
        {
            Methods method = new Methods();
            
                MemberFloatable mf = new MemberFloatable();
                MySqlConnection conn = method.myConn();
                string query = "SELECT * FROM MemberFloatable WHERE MemberId =" + member.MemberId;
                MySqlDataReader dr = method.myReader(query, conn);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        mf.MemberId = Convert.ToInt16(dr["MemberId"]);
                        mf.NoTimesWeek = Convert.ToInt16(dr["NoTimesWeek"]);
                    }
                }
                else
                {
                    mf.MemberId = member.MemberId;
                    mf.NoTimesWeek = 0;  

                }
                conn.Close();
                conn.Dispose();

                return mf;

        }
    }
}