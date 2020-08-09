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

namespace Pizzaria1
{
    /// <summary>
    /// AccessControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AccessControl : UserControl
    {
        Packet pack = Packet.Instance;
        Ioinfo io = new Ioinfo();
        public AccessControl() : base()
        {
            InitializeComponent();
        }

        public void Disting(string msg, bool Clear)
        {
            if (Clear == true)
            {
                io.Clear();
            }
            string[] info = msg.Split('#');
            string name = info[0];
            string state = "";
            string time = "";
            string date = info[4];

            if (info[1] == "True")
            {
                state = "입";
                time = info[2]; //들어온시간이라면 들어온 시간을 나간시간이라면 나간 시간을...
            }
            else if (info[1] == "False")
            {
                state = "출";
                time = info[3];
            }
            string[] list_info = { name, state, time, date };
            //프로퍼티 감지

            io.Add(new Iolist(list_info[0], list_info[1], list_info[2], list_info[3]));
            ioinfolist.ItemsSource = io.AsEnumerable<Iolist>();
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(namebox.Text))
                return true;
            else
                return ((item as Iolist).Name.IndexOf(namebox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void Namebox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ioinfolist.ItemsSource);
            view.Filter = UserFilter;
            CollectionViewSource.GetDefaultView(ioinfolist.ItemsSource).Refresh();

        }
    }
}
