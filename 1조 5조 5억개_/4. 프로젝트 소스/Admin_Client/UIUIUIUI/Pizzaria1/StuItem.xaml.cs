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

namespace Pizzaria1
{
    /// <summary>
    /// StuItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StuItem : UserControl
    {
        public StuItem()
        {
            InitializeComponent();
        }

        #region 프로퍼티
        private int _id;
        private string _name;
        private string _dep;
        private bool _atb;
        private bool _atl;

        [Category("Custom Props")]
        public int pId
        {
            get { return _id; }
            set { _id = value; xID.Text = value.ToString(); }
        }
        [Category("Custom Props")]
        public string pName
        {
            get { return _name; }
            set { _name = value; xName.Text = value; }
        }
        [Category("Custom Props")]
        public string pDep
        {
            get { return _dep; }
            set { _dep = value; xDep.Text = value; }
        }
        [Category("Custom Props")]
        public bool pAtb
        {
            get { return _atb; }

            set
            {
                _atb = value;
                if (value == true)
                {
                    xCO.Content = FindResource("Yes");
                }
                else
                {
                    xCX.Content = FindResource("Nope");
                }
            }
        }
        [Category("Custom Props")]
        public bool pAtl
        {
            get { return _atl; }

            set
            {
                _atl = value;
                if (value == true)
                {
                    xCO.Content = FindResource("VVV");
                    xCL.Content = FindResource("Late");
                }
            }
        }
        #endregion
    }
}
