using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Windows.Threading;

namespace Pizzaria1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {

        double orginalWidth, originalHeight;

        ScaleTransform scale = new ScaleTransform();

        #region 서버관련
        //const string ip = "172.18.79.165";
        const string ip = "203.234.10.63";
        const int port = 1234;
        Client server = Client.Instance2;
        Packet pack = Packet.Instance;
        #endregion

        #region 자식창
        RealTime rtime = new RealTime();
        UserControlEscolha Uces = new UserControlEscolha();
        AccessControl acc_con = new AccessControl();
        Attendance att = new Attendance();
        UserControlInicio Ucic = new UserControlInicio();
        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
            server.ParentInfo(this);
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
            //서버연결
            if (server.Connect(ip, port) == true)
            {
                
            }
            else
            {
                MessageBox.Show("서버연결실패");
            }
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            orginalWidth = this.Width;
            originalHeight = this.Height;
            if (this.WindowState == WindowState.Maximized)
            {
                ChangeSize(this.ActualWidth, this.ActualHeight);
            }
            this.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
            string admin = "ADMIN";
            server.Send(admin);
            PackSendzip();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            server.ClientStop();
        }
        
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(Ucic);
                    GridPrincipal.Children.Add(rtime);
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(Uces);
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(att);
                    break;

                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(acc_con);
                    break;

                default:
                    break;
            }
        }

        public void PackSendzip()
        {
            string msg = pack.SendIC();
            server.Send(msg);
            string msg1 = pack.SendNLA();
            server.Send(msg1);
            string msg2 = pack.SendNB();
            server.Send(msg2);
            string msg3 = pack.SendAll();
            server.Send(msg3);
            string msg4 = pack.SendName();
            server.Send(msg4);
            string msg5 = pack.SendIO();
            server.Send(msg5);
            string msg6 = pack.SendChat();
            server.Send(msg6);
        }

        public void PaserByteDate(byte[] msg)
        {
            string str = Encoding.UTF8.GetString(msg);           //서버가 보낸 메세지를 문자열화 시킴
            string[] token = str.Split('@');
            switch (token[0].Trim())
            {
                case "DB_CHANGED": dbChacged(token[1]); break;
                case "COUNT_SI_ACK": Count_SI(token[1]); break;
                case "COUNT_CL_ACK": Count_CL(token[1]); break;
                case "SELECT_NB_ACK": Select_I_NB(token[1]); break;
                case "SELECT_SD_ACK": Select_SD(token[1]); break;
                case "SELECT_NBTT_ACK": Select_I_NAME(token[1]); break; 
                case "SELECT_SA_ACK": Select_SA(token[1]); break;
                case "COUNT_IO_ACK": Count_IO(token[1]); break;
                case "SELECT_N_ACK": Ucic.Select_Name(token[1]); break;
                case "COUNT_ML_ACK": Ucic.Select_MemL(token[1]); break;
                case "SELECT_MT_ACK": Ucic.Select_MemT(token[1]); break;
            }
        }

        #region RECV
        void dbChacged(string msg)
        {
            PackSendzip();
        }
        
        void Count_SI(string msg)
        {
            try
            {
                string[] sp = msg.Split('#');
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    string s = sp[0];
                    string i = sp[1];
                    nStu.Text = i + "/" + s;
                }));
            }
            catch { }
        }

        void Count_IO(string msg)
        {
            string[] sp = msg.Split('#');
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                string s = sp[1];
                string i = sp[0];
                IOcount.Text = i + "/" + s;
            }));
        }

        void Count_CL(string msg)
        {
            string[] sp = msg.Split('#');
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                string s = sp[0];
                nlate.Text = s;
            }));


        }

        void Select_SD(string msg)
        {
            string[] sp = msg.Split('/');
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                Uces.Itemslist.Items.Clear();
                for (int i = 0; i < sp.Length; i++)
                {
                    Uces.populateItems(sp[i]);
                }
            }));
        }

        void Select_I_NB(string msg)
        {
            string[] sp = msg.Split('/');

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                rtime.Itemslist.Items.Clear();
                for (int i = 0; i < sp.Length; i++)
                {
                    rtime.populateItems(sp[i]);
                }
            }));
        }

        public void Select_I_NAME(string msg)
        {
            string[] sp = msg.Split('/');
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                
                for (int i = 0; i < sp.Length; i++)
                {
                    bool Clear = false;
                    if (acc_con != null)
                    {
                        if (i == 0)
                        {
                            Clear = true;
                        }
                        acc_con.Disting(sp[i], Clear);
                    }

                }
            }));
        }

        public void Select_SA(string msg)
        {
            string[] sp = msg.Split('/');
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                for (int i = 0; i < sp.Length; i++)
                {
                    if (att != null)
                    {
                        att.Disting(sp[i]);
                    }
                }
            }));
        }
        #endregion
        
        #region UICon
        void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (100 + (60 * index)), 0, 0);
        }

        private void ChangeSize(double width, double height)
        {
            scale.ScaleX = width / orginalWidth;
            scale.ScaleY = height / originalHeight;

            FrameworkElement rootElement = this.Content as FrameworkElement;
            rootElement.LayoutTransform = scale;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //최소화 버튼
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //창크기 조정 버튼
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btnImg.Content == FindResource("Max"))
            {
                btnImg.Content = FindResource("Mini");
                WindowState = WindowState.Maximized;
            }

            else
            {
                btnImg.Content = FindResource("Max");
                WindowState = WindowState.Normal;
            }
        }

        //창닫기 버튼
        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}