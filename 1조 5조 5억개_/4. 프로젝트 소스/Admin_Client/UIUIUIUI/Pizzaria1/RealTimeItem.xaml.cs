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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Pizzaria1
{
    /// <summary>
    /// RealTimeItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RealTimeItem : UserControl
    {
        public RealTimeItem()
        {
            InitializeComponent();
        }

        #region 프로퍼티
        private string _name;
        private bool _ycheck;
        private bool _ncheck;

        [Category("Custom Props")]
        public string sName
        {
            get { return _name; }
            set { _name = value; namebox.Text = value; }
        }

        [Category("Custom Props")]
        public bool yCheck
        {
            get { return _ycheck; }

            set
            {
                _ycheck = value;
                if (value == true)
                {
                    Check.Content = FindResource("Checkgreen");
                    Delete.Content = FindResource("Delete");
                }
            }

        }

        [Category("Custom Props")]
        public bool nCheck
        {
            get { return _ncheck; }
            set
            {
                _ncheck = value;
                if (value == true)
                    Delete.Content = FindResource("Deletered");
            }
        }
        #endregion
    }
}
