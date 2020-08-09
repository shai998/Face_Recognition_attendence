using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pizzaria1
{
    public class Iolist : INotifyPropertyChanged
    {
        string name;
        string state;
        string time;
        string date;
        public Iolist() { }
        public Iolist(string _name, string _state, string _time, string _date)
        {
            name = _name;
            state = _state;
            time = _time;
            date = _date;
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
        public string Time
        {
            get
            {
                return time.ToString();
            }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }

        }
        public string Date
        {
            get
            {
                return date.ToString();
            }
            set
            {
                time = value;
                OnPropertyChanged("Date");
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
    public class Ioinfo : ObservableCollection<Iolist>
    {
        public Ioinfo() { }
        public void Addlist(Iolist s)
        {
            Add(s);
        }

    }
}
