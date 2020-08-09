using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzaria1
{
    class ChatMessageListItemViewModel
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public string Message { get; set; }

        public bool SendByMe { get; set; }
    }
}
