using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Linq;

//DB변경 탐지용
using System.Diagnostics;
using System.Timers;
using System.Data;
using System.Data.SqlClient;

namespace DB_Server
{
    enum LogType { RUN, STOP, CONNECT, DISCONNECT, ERROR }

    class Server
    {

        #region 맴버 필드 및 프로퍼티
        private Socket server; 
        private Socket admin_sock = null;

        private List<Socket> slist = new List<Socket>();
        private ParseData pdata = new ParseData();
        private List<string> str = new List<string>();
        System.Timers.Timer timer = new System.Timers.Timer();


        string Source = @"Data Source = 172.18.79.165; Initial Catalog = Att_test; User ID = Hawi; Password =1234";
        //string Source = @"Data Source = 192.168.0.43; Initial Catalog = Att_test; User ID = Hawi; Password =1234";

        //DB변경 탐지용 
        SqlDependency sd;
       

        public string ServerIp { get; private set; }
        public int ServerPort { get; private set; }
        #endregion

        #region 타이머
        public void Timerset() // 처음에 프로그램 실행시 되는 타이머
        {
            DateTime Startdate = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss"));
            DateTime EndDate = Convert.ToDateTime("23:59:59");

            TimeSpan dateDiff = EndDate - Startdate;

            int diffHour = dateDiff.Hours;
            int diffMin = dateDiff.Minutes;
            int diffSec = dateDiff.Seconds;

            Console.WriteLine(diffHour.ToString() + "시간" + diffMin.ToString() + "분" + diffSec.ToString() + "초" + "이후 INSERT 됩니다");
            
            int Hourcount = 60 * 60 * 1000;
            int Mincount = 60 * 1000;
            int Seccount = 60*1000;
            int TimerInterval = (diffHour * Hourcount + diffMin * Mincount + diffSec * Seccount)+1000;

            
            timer.Interval = TimerInterval;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        public void Timerset2() //24시간 이후 실행되는 타이머
        {
            int Hourcount = 60 * 60 * 1000;
            int TimerInterval = (Hourcount * 24)+1000;

            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Interval = TimerInterval;
            timer1.Elapsed += new ElapsedEventHandler(timer_Elapsed2);
            timer1.Start();
            
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {   
            //Console.WriteLine("1회차");
            Timerset2();
            timer.Close();
            Select_New_Bool();
            Insert_New_Bool();
        }

        public void timer_Elapsed2(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("2회차");
            Select_New_Bool();
            Insert_New_Bool();
        }
        #endregion

        public bool ServerRun(int port)
        {
            //Select_New_Bool();
            //Insert_New_Bool();
            int Times = 0; 
            try
            {
                server = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
                server.Bind(ipep);
                server.Listen(20);

                IPEndPoint ip = (IPEndPoint)server.LocalEndPoint;
                ServerIp = ip.Address.ToString();
                ServerPort = port;
                
                Thread th = new Thread(new ParameterizedThreadStart(ServerThread));
                th.Start(this);
                th.IsBackground = true;

                Console.WriteLine("서버 구동중...");

                #region 타이머로 함수 실행
                if (Times == 0)
                {
                    Timerset();
                    Times++;
                }
                else
                {
                    Timerset2();
                }
                #endregion

                #region DB변경 탐지 실행
                Thread_start1();
                Thread_start2();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Thread_start1()
        {
            try
            {
                SqlDependency.Start(Source);
                Thread Dbdetect = new Thread(new ParameterizedThreadStart(DBThread));
                Dbdetect.Start();
                Console.WriteLine("출결정보 갱신 현황 감지중");
            }
            catch (Exception)
            {
                Console.WriteLine("출결정보 갱신 현황 감지 실패");
            }
        }
        public void Thread_start2()
        {
            try
            {
                SqlDependency.Start(Source);
                Thread Dbdetect2 = new Thread(new ParameterizedThreadStart(DBThread2));
                Dbdetect2.Start();
                Console.WriteLine("출입정보 갱신 현황 감지중");
            }
            catch (Exception)
            {
                Console.WriteLine("출입정보 갱신 현황 감지 실패");
            }
        }

        #region 하루 지나면 DB에 INSERT
        public void Select_New_Bool()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Source;
            conn.Open();

            SqlCommand sc = new SqlCommand(@"select STU_ID from dbo.STUDENT", conn);

            SqlDataReader rd = sc.ExecuteReader();

            while(rd.Read())
            {
                str.Add(rd[0].ToString());
            }
            
            conn.Close();
            Console.WriteLine("DB검색 완료. 입력을 시작합니다.");
        }

        public void Insert_New_Bool()
        {
            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Source;
            conn.Open();

            foreach (string i in str)
            {
                //없으면 INSERT 하고 있으면 아무것도 안함
                SqlCommand sc1 = new SqlCommand(@"INSERT INTO DAILY_ATTEND(STU_ID, ATT_DATE, ATT_BOOL) VALUES(" + i + ",'" + nowdate + "'," + "0)", conn);
                sc1.ExecuteNonQuery();
            }

            conn.Close();
            Console.WriteLine("입력을 완료했습니다");
        }
        #endregion

        #region DB변경 감지
        public void GetLastState()
        {
            
            SqlConnection conn;
            conn = new SqlConnection();
            conn.ConnectionString = Source;
            conn.Open();    //  데이터베이스 연결

            SqlCommand sc = new SqlCommand(@"SELECT STU_ID, ATT_TIME, ATT_LATE, ATT_DATE, ATT_BOOL FROM dbo.DAILY_ATTEND", conn);
            sd = new SqlDependency(sc);

            sd.OnChange += Sd_Onchange;

            SqlDataReader rdr = sc.ExecuteReader();

            rdr.Close();
            conn.Close();
            
        }

        public void GetLastState2()
        {
            
            SqlConnection conn;
            conn = new SqlConnection();
            conn.ConnectionString = Source;
            conn.Open();    //  데이터베이스 연결

            SqlCommand sc = new SqlCommand(@"SELECT STU_ID, INOUT_BOOL, IN_TIME, OUT_TIME, INOUT_DATE FROM dbo.INOUT", conn);
            sd = new SqlDependency(sc);

            sd.OnChange += Sd_Onchange2;

            SqlDataReader rdr = sc.ExecuteReader();

            rdr.Close();
            conn.Close();
            
        }

        private void Sd_Onchange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info.ToString().StartsWith("Invalid"))
            {
                Console.WriteLine("감지 실패");
            }
            else
            {
                //여기에 클라이언트로 보낼 메서드 입력
                string sLog = string.Format("{0}", e.Info.ToString());
                try
                {
                    if(admin_sock != null)
                        pdata.DB_Change(admin_sock);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("클라이언트가 연결되지 않았습니다." + ex);
                }
                
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 에" + sLog + " 되었습니다");
                

                GetLastState();
            }
        }
        private void Sd_Onchange2(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info.ToString().StartsWith("Invalid"))
            {
                Console.WriteLine("감지 실패");
            }
            else
            {
                //여기에 클라이언트로 보낼 메서드 입력
                string sLog = string.Format("{0}", e.Info.ToString());
                try
                {
                    if (admin_sock != null)
                        pdata.DB_Change(admin_sock);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("클라이언트가 연결되지 않았습니다." + ex);
                }

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 에" + sLog + " 되었습니다");


                GetLastState2();
            }
        }
        #endregion

        public void ServerStop()
        {
            try
            {
                server.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send(Socket sock, string msg)
        {
            try
            {
                if (sock.Connected)
                {
                    byte[] data = Encoding.Default.GetBytes(msg);
                    this.SendData(sock, data);
                }
                else
                {
                    LogMessage(LogType.ERROR, sock, "클라이언트미연결");
                }
            }
            catch (Exception ex)
            {
                LogMessage(LogType.ERROR, sock, ex.Message);
            }
        }

        private void LogMessage(LogType type, Socket sock, string str)
        {
            IPEndPoint ip = (IPEndPoint)sock.RemoteEndPoint;

            String temp = String.Empty;

            if (type == LogType.CONNECT)
            {
                temp = String.Format("[클라이언트접속]{0}:{1} 성공",
                      ip.Address, ip.Port);

                Console.WriteLine(temp);
            }
            else if (type == LogType.DISCONNECT)
            {
                temp = String.Format("[클라이언트접속해제]{0}:{1} 성공",
                      ip.Address, ip.Port);

                Console.WriteLine(temp);
            }
            else if (type == LogType.ERROR)
            {
                temp = String.Format("[에러]{0}:{1} {2}",
                      ip.Address, ip.Port, str);

                Console.WriteLine(temp);
            }
        }

        #region(내부 사용) 자체 호출 기능
        private void SendData(Socket sock, byte[] data)
        {
            try
            {
                int total = 0;
                int size = data.Length;
                int left_data = size;
                int send_data = 0;

                // 전송할 데이터의 크기 전달
                byte[] data_size = new byte[4];
                data_size = BitConverter.GetBytes(size);
                send_data = sock.Send(data_size);

                // 실제 데이터 전송
                while (total < size)
                {
                    send_data = sock.Send(data, total, left_data, SocketFlags.None);
                    total += send_data;
                    left_data -= send_data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ReceiveData(Socket sock, ref byte[] data)
        {
            try
            {
                int total = 0;
                int size = 0;
                int left_data = 0;
                int recv_data = 0;

                // 수신할 데이터 크기 알아내기 
                byte[] data_size = new byte[4];
                recv_data = sock.Receive(data_size, 0, 4, SocketFlags.None);
                size = BitConverter.ToInt32(data_size, 0);
                left_data = size;
                data = new byte[size];
                // 실제 데이터 수신
                while (total < size)
                {
                    recv_data = sock.Receive(data, total, left_data, 0);
                    if (recv_data == 0) break;
                    total += recv_data;
                    left_data -= recv_data;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region thread
        private void ServerThread(object data)
        {
            while (true)
            {
                Socket client = server.Accept();
                
                slist.Add(client);      //소켓 저장 => Work Thread부분의 ParseData이후로 이동

                LogMessage(LogType.CONNECT, client, "");   //로그메시지 처리  

                //스레드 생성(소켓 전달)
                Thread tr = new Thread(new ParameterizedThreadStart(WorkThread));
                tr.Start(client);
                tr.IsBackground = true;
            }
        }

        //DB스레드
        private void DBThread(object data)
        {
            try
            {    
                GetLastState();
            }
            catch
            {
            }
        }
        private void DBThread2(object data)
        {
            try
            {
                GetLastState2();
            }
            catch
            {
            }
        }
        

        private void WorkThread(object data)
        {
            Socket sock = (Socket)data;

            byte[] msg = null;
            try
            {
                while (true)
                {
                    //수신
                    ReceiveData(sock, ref msg);   // 수신한 문자열이 있으면 화면에 출력

                    string encode_msg = Encoding.Default.GetString(msg);

                    //분석요청
                    switch(encode_msg)
                    {
                        case "ADMIN":
                            admin_sock = sock;                     break;

                        default: 
                            pdata.PaserByteData(sock, encode_msg); break;
                    }
                    msg = null;
                }
            }
            catch (SocketException ex)
            {
                LogMessage(LogType.ERROR, sock, ex.Message);
                LogMessage(LogType.DISCONNECT, sock, "");

                slist.Remove(sock); //소켓 제거

                if (sock == admin_sock)
                    admin_sock = null;

                sock.Close();
            }
        }
        #endregion         
    }
}
