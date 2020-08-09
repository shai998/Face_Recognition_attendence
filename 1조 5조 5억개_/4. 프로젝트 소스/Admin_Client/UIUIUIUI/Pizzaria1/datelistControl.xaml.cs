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
    /// Attendance.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Attendance : UserControl
    {
        Packet pack = Packet.Instance;
        Client server = Client.Instance2;
        dateinfo io = new dateinfo();

        public Attendance()
        {
            InitializeComponent();
        }

        private void Xcalender_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            io.Clear();
            string Data = this.xcalender.SelectedDate.Value.ToShortDateString();
            string msg = pack.Senddate(Data);
            server.Send(msg);

        }

        public void Disting(string msg)
        {
            try
            {
                string[] info = msg.Split('#');
                string name = info[0];
                string state = "";

                if (info[1] == "True")
                {
                    state = "출석";
                    if (info[2] == "True")
                        state = "지각";
                }
                else if (info[1] == "False")
                {
                    state = "결석";
                }

                string[] list_info = { name, state };
                //프로퍼티 감지

                io.Add(new datelist(list_info[0], list_info[1]));
                ioinfolist_date.ItemsSource = io.AsEnumerable<datelist>();
            }
            catch
            {

            }
        }
        
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(namebox.Text))
                return true;
            else
                return ((item as datelist).Name.IndexOf(namebox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void Namebox_TextChanged(object sender, TextChangedEventArgs e)
        {
             CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ioinfolist_date.ItemsSource);
             view.Filter = UserFilter;
             CollectionViewSource.GetDefaultView(ioinfolist_date.ItemsSource).Refresh();
           

        }
    }
}
