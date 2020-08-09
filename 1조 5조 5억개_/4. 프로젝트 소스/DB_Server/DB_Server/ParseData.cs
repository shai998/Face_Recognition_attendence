using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DB_Server
{
    class ParseData
    {
        DataBase db = new DataBase();

        #region 수신패킷 분석 및 처리

        #region Parsebydata
        //public void PaserByteData(Socket sock, byte[] data)
        public void PaserByteData(Socket sock, string msg)
        {
            //string msg = Encoding.UTF8.GetString(data);
            //string msg = Encoding.Default.GetString(data);
            //Console.WriteLine(msg);

            string[] token = msg.Split('@');
            switch (token[0].Trim())
            {
                case "SELECT_NBTT": Select_Data_NBTT(sock, token[1]); break;
                case "SELECT_ID": Select_Data_ID(sock, token[1]); break;
                case "SELECT_BD": Select_Data_BD(sock, token[1]); break;
                case "SELECT_NB": Select_Data_NB(sock, token[1]); break;
                case "SELECT_SD": Select_Data_SD(sock, token[1]); break;
                case "SELECT_SA": Select_Data_SA(sock, token[1]); break;
                case "SELECT_MT": Select_Data_MT(sock, token[1]); break;
                case "SELECT_N": Select_Data_N(sock, token[1]); break;

                case "COUNT_STU": Count_Data_STU(sock, token[1]); break;
                case "COUNT_SI": Count_Data_SI(sock, token[1]); break;
                case "COUNT_CL": Count_Data_CL(sock, token[1]); break;
                case "COUNT_IO": Count_Data_IO(sock, token[1]); break;
                case "COUNT_ML": Count_Data_ML(sock, token[1]); break;
                
                case "SELECT_A": Select_Data_A(sock, token[1]); break;
                case "SELECT_I": Select_Data_I(sock, token[1]); break;
                case "SELECT_S": Select_Data_S(sock, token[1]); break; 

                case "UPDATE": Update_Data(sock, token[1]); break;
                case "INSERT": Insert_Data(sock, token[1]); break;
                case "DELETE": Delete_Data(sock, token[1]); break;

                case "DB_CHANGED_DP": Db_Changed_DP(sock, token[1]); break;

                default: break;
            }
        }
        #endregion

        #region Select
        void Select_Data_NBTT(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_NBTT(msg);

            ackmessage = "SELECT_NBTT_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_ID(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_ID(msg);
            
            ackmessage = "SELECT_ID_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_BD(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_BD(msg);

            ackmessage = "SELECT_BD_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_NB(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_BD(msg);

            ackmessage = "SELECT_NB_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_SD(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_SD(msg);

            ackmessage = "SELECT_SD_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_SA(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_SA(msg);

            ackmessage = "SELECT_SA_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_MT(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_MT(msg);

            ackmessage = "SELECT_MT_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_N(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_N(msg);

            ackmessage = "SELECT_N_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        #endregion

        #region Count
        void Count_Data_STU(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Count_Stu(msg);

            ackmessage = "COUNT_STU_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Count_Data_SI(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Count_Si(msg);

            ackmessage = "COUNT_SI_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Count_Data_CL(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Count_Cl(msg);

            ackmessage = "COUNT_CL_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Count_Data_IO(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Count_IO(msg);

            ackmessage = "COUNT_IO_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Count_Data_ML(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Count_ML(msg);

            ackmessage = "COUNT_ML_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        #endregion

        #region Select A/I/S
        void Select_Data_A(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine( "[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_A(msg);
            //string ackmessage = "1";

            ackmessage = "SELECT_A_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_I(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_I(msg);
            
            ackmessage = "SELECT_I_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        void Select_Data_S(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Select_Data_S(msg);

            ackmessage = "SELECT_S_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }
        #endregion

        #region Update, Insert, Delete
        void Update_Data(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            db.Update_Data(msg);

            db.DB_Disconn();
        }
        void Insert_Data(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            db.Insert_Data(msg);

            db.DB_Disconn();
        }
        void Delete_Data(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            db.Delete_Data(msg);

            db.DB_Disconn();
        }
        #endregion

        void Db_Changed_DP(Socket sock, string msg)
        {
            Server server = new Server();

            Console.WriteLine("[수신]" + msg);

            db.DB_Conn();

            string ackmessage = db.Db_Changed_DP(msg);

            ackmessage = "DB_CHANGED_DP_ACK@" + ackmessage;

            Console.WriteLine("[송신]" + ackmessage);

            server.Send(sock, ackmessage);

            db.DB_Disconn();
        }

        #region DB change
        public void DB_Change(Socket sock)
        {
            Server server = new Server();

            string ackmessage = "DB_CHANGED@";

            server.Send(sock, ackmessage);

            Console.WriteLine("[송신]" + ackmessage);

        }
        #endregion

        #endregion
    }
}
