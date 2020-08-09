using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Pizzaria1
{
    class Client
    {
        private Socket sock;
        private MainWindow mw;

        public void ParentInfo(MainWindow m)
        {
            mw = m;
        }
        #region 싱글톤
        public static Client Instance2 { get; private set; }

        static Client()
        {
            Instance2 = new Client();
        }

        private Client()
        {

        }
        #endregion
        #region 연결, 연결해제
        //서버연결
        public bool Connect(string ip, int port)
        {
            try
            {

                sock = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);

                sock.Connect(ipep);
                //Send("ADMIN");


                Thread th = new Thread(new ParameterizedThreadStart(RecvThread));   //쓰래드생성
                th.Start(this);
                th.IsBackground = true;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        //서버연결 해제
        public void ClientStop()
        {
            try
            {
                sock.Close();       //클라이언트 소켓을 닫음
                MessageBox.Show("접속을 해제합니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        //스레드(받는용)
        public void RecvThread(object data)
        {
            while (true)
            {
                byte[] msg = null;

                ReceiveData(sock, ref msg);   // 수신한 문자열이 있으면 화면에 출력

                mw.PaserByteDate(msg);


            }
        }

        private void ReceiveData(Socket s, ref byte[] data)
        {

            try
            {
                int total = 0;
                int size = 0;
                int left_data = 0;
                int recv_data = 0;

                // 수신할 데이터 크기 알아내기 
                byte[] data_size = new byte[4];
                recv_data = s.Receive(data_size, 0, 4, SocketFlags.None);
                size = BitConverter.ToInt32(data_size, 0);
                left_data = size;

                data = new byte[size];

                // 실제 데이터 수신
                while (total < size)
                {
                    recv_data = s.Receive(data, total, left_data, 0);
                    if (recv_data == 0) break;
                    total += recv_data;
                    left_data -= recv_data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Send(string chat)
        {
            try
            {
                if (sock.Connected)
                {
                    SendData(Encoding.Default.GetBytes(chat));
                }
                else
                {
                    MessageBox.Show("전송실패");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendData(byte[] data)
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
                MessageBox.Show(ex.Message);
            }
        }
    
    }
}
