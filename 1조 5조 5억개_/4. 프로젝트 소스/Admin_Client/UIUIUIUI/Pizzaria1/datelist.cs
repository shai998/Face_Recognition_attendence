using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pizzaria1
{
    public class datelist : INotifyPropertyChanged
    {
        string name;
        string state;
        public datelist() { }
        public datelist(string _name, string _state)
        {
            name = _name;
            state = _state;
        }
        public override string ToString()
        {
            return name.ToString();
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string State
        {
            get
            {
                return state.ToString();
            }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }

        }
        

        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p));
            }
        }


        #region INotifyPropertyChanged 멤버

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
    public class dateinfo : ObservableCollection<datelist>
    {
        public dateinfo() { }
        public void Addlist(datelist s)
        {
            Add(s);
        }

    }


}
