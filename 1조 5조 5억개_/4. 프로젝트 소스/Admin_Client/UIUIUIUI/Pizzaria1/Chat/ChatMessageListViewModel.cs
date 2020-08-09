
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzaria1
{
    class ChatMessageListViewModel : ObservableCollection<ChatMessageListItemViewModel>
    {
        public ChatMessageListViewModel()
        {

            Add(new ChatMessageListItemViewModel
            {
                Message = "환영합니다. \n검색할 이름을 입력해주세요",
                SendByMe = false
            });
        }
    }
}
