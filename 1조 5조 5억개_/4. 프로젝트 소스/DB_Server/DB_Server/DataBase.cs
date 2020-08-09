using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Server
{
    public class DataBase
    {
        private SqlConnection conn;
        private string Source;
 
        public DataBase()
        {
            conn = new SqlConnection();
            Source = @"Data Source = 172.18.79.165; Initial Catalog = Att_test; User ID = Hawi; Password =1234";
        }

        #region 연결 관련
        public bool DB_Conn()
        {
            try
            {
                conn.ConnectionString = Source;
                conn.Open();    //  데이터베이스 연결

                Console.WriteLine("데이터베이스 연결 성공...");

                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("데이터베이스 연결 실패...");
                return false;
            }
        }

        public void DB_Disconn()
        {
            if (conn != null)
            {
                conn.Close();       //  연결 해제
                Console.WriteLine("데이터베이스 연결 해제");
            }
        }
        #endregion

        #region DB쿼리 보내기

        public string Select_Data_NBTT(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //STU_NAME
                    msg += reader[1] + "#";  //INOUT_BOOL
                    msg += reader[2] + "#";  //IN_TIME
                    msg += reader[3] + "#";  //OUT_TIME
                    msg += reader[4] + "/";  //INOUT_DATE
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_ID(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "/";  //STU_ID
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_BD(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //ATT_BOOL
                    msg += reader[1] + "/";  //ATT_DATE
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_NB(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //STU_NAME
                    msg += reader[1] + "/";  //INOUT_BOOL
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_SD(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //STU_ID
                    msg += reader[1] + "#";  //STU_NAME
                    msg += reader[2] + "#";  //STU_DEP
                    msg += reader[3] + "#";  //ATT_BOOL
                    msg += reader[4] + "/";  //ATT_LATE
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_SA(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //STU_NAME
                    msg += reader[1] + "#";  //ATT_BOOL
                    msg += reader[2] + "/";  //ATT_LATE
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_MT(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "/";  //ATT_TIME
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_N(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "/";  //STU_NAME
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }

        public string Count_Stu(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0]; //Student Count
                }
                reader.Close();
                command.Dispose();

                return msg;
            }
            catch 
            {
                return null;
            }
        }
        public string Count_Si(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] +"#"; //Student Count
                    msg += reader[1]; //Inout Count
                }
                reader.Close();
                command.Dispose();

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Count_Cl(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0]; //Late Count
                }
                reader.Close();
                command.Dispose();

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Count_IO(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#"; //IN_TIME COUNT
                    msg += reader[1];       //OUT_TIME COUNT

                }
                reader.Close();
                command.Dispose();

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Count_ML(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0];       //ATT_LATE COUNT
                }
                reader.Close();
                command.Dispose();

                return msg;
            }
            catch
            {
                return null;
            }
        }

        public string Select_Data_A(string Query)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //ATT_ID
                    msg += reader[1] + "#"; //STU_ID
                    msg += reader[2] + "#"; //ATT_TIME
                    msg += reader[3] + "#"; //ATT_LATE
                    msg += reader[4] + "#"; //ATT_DATE
                    msg += reader[5] + "/"; //ATT_BOOL
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_I(string Query)
        {
            try
                {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //INOUT_ID
                    msg += reader[1] + "#"; //STU_ID
                    msg += reader[2] + "#"; //INOUT_BOOL
                    msg += reader[3] + "#"; //IN_TIME
                    msg += reader[4] + "/"; //OUT_TIME
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        public string Select_Data_S(string Query)
        {
            try
            { 
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //STU_ID
                    msg += reader[1] + "#"; //STU_NAME
                    msg += reader[2] + "/"; //STU_DEP
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }

        public void Update_Data(string Query)
        {
            string comtext = Query;

            SqlCommand command = new SqlCommand(comtext, conn);
            command.ExecuteNonQuery();
        }
        public void Insert_Data(string Query)
        {
            string comtext = Query;

            SqlCommand command = new SqlCommand(comtext, conn);
            command.ExecuteNonQuery();
        }
        public void Delete_Data(string Query)
        {
            string comtext = Query;

            SqlCommand command = new SqlCommand(comtext, conn);
            command.ExecuteNonQuery();
        }

        public string Db_Changed_DP(string Query) //DAILY_ATTEND 테이블 보내기.(출석갱신)
        {
            try
            {
                string comtext = Query;
                string msg = "";

                SqlCommand command = new SqlCommand(comtext, conn);
                SqlDataReader reader = command.ExecuteReader(); // 2.Select 전용

                while (reader.Read() == true)
                {
                    msg += reader[0] + "#";  //STU_ID
                    msg += reader[1] + "#";  //STU_NAME
                    msg += reader[2] + "#";  //STU_DEP
                    msg += reader[3] + "#";  //ATT_BOOL
                    msg += reader[4] + "/";  //ATT_LATE
                }
                reader.Close();
                command.Dispose();

                msg = msg.Substring(0, msg.Length - 1);

                return msg;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}