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
    /// Interação lógica para UserControlEscolha.xam
    /// </summary>
    public partial class UserControlEscolha : UserControl
    {
        

        public UserControlEscolha()
        {
            InitializeComponent();
        }

        public void populateItems(string sp)
        {
            StuItem[] listItems = new StuItem[1];
            string[] sp1 = sp.Split('#');
            try
            {
                for (int i = 0; i < listItems.Length; i++)
                {
                    listItems[i] = new StuItem();
                    listItems[i].pId = int.Parse(sp1[0]);
                    listItems[i].pName = sp1[1];
                    listItems[i].pDep = sp1[2];
                    if (sp1[3] == "True")
                    {
                        listItems[i].pAtb = true;
                    }
                    else
                    {
                        listItems[i].pAtb = false;
                    }

                    if (sp1[4] == "True")
                    {
                        listItems[i].pAtl = true;
                    }

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
            catch { }
            
        }
    }
}
