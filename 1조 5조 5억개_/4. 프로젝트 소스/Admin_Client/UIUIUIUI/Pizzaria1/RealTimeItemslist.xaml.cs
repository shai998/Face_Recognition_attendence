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
    /// RealTime.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RealTime : UserControl 
    {
        

        public RealTime()
        {
            InitializeComponent();
        }

        public void populateItems(string sp)
        {
            string[] sp1 = sp.Split('#');
            RealTimeItem[] listItems = new RealTimeItem[1];
            
            for (int i = 0; i < listItems.Length; i++)
            {
                listItems[i] = new RealTimeItem();
                listItems[i].sName = sp1[0];
                if (sp1[1] == "True")
                {
                    listItems[i].yCheck = true;
                }
                else
                    listItems[i].nCheck = false;

                if (Itemslist.Items.Count < 0)
                {
                    Itemslist.Items.Clear();
                }
                else
                {
                    Itemslist.Items.Add(listItems[i]);
                }
            }
        }
    }
}
