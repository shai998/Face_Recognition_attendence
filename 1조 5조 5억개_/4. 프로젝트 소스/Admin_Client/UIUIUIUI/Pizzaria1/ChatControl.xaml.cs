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
using System.Threading;
using Moda.Korean.TwitterKoreanProcessorCS;

namespace Pizzaria1
{
    /// <summary>
    /// Interação lógica para UserControlInicio.xam
    /// </summary>
    public partial class UserControlInicio : UserControl
    {
        List<string> cn = new List<string>();
        List<DateTime> mt = new List<DateTime>();
        
        Packet pack = Packet.Instance;
        Client server = Client.Instance2;
        string aaa1;
        string memlate = null;
        ChatMessageListItemViewModel chat = null;
        int hh = 0;
        int mm = 0;
        int ss = 0;

        public UserControlInicio()
        {
            InitializeComponent();
            chat = (ChatMessageListItemViewModel)FindResource("chat");
        }

        private void NameData(string name)
        {
            string Data = name;
            string msg = pack.SendName(Data);
            server.Send(msg);
        }
        private void TimeData(string name)
        {
            string Data = name;
            string msg = pack.SendTime(Data);
            server.Send(msg);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChatMessageListViewModel chatlist = (ChatMessageListViewModel)FindResource("chatlist");

            if (string.IsNullOrEmpty(MessageText.Text))
                return;

            ChatMessageListItemViewModel newchat = new ChatMessageListItemViewModel()
            {
                Message = MessageText.Text,
                SendByMe = true
            };

            chatlist.Add(newchat);

            
            for(int i =0; i < cn.Count; i++)
            {
                if (MessageText.Text == cn[i])
                {
                    aaa1 = MessageText.Text;

                    string result = TokenTosTringSample(MessageText.Text);

                    string msg = Splitsth_name(result);

                    ChatMessageListItemViewModel newchat2 = new ChatMessageListItemViewModel()
                    {
                        Message = msg,
                        SendByMe = false
                    };

                    chatlist.Add(newchat2);

                    MessageText.Clear();

                    break;
                }

            }

            if (aaa1 == null)
            {
                string result = TokenTosTringSample(MessageText.Text);

                string msg = Splitsth(result);

                ChatMessageListItemViewModel newchat2 = new ChatMessageListItemViewModel()
                {
                    Message = msg,
                    SendByMe = false
                };

                chatlist.Add(newchat2);

                MessageText.Clear();
            }
            else
            {
                string result = TokenTosTringSample(MessageText.Text);

                string msg = Splitsth2(result);
                //aaa1 = null;
                //memlate = null;
                hh = 0;
                mm = 0;
                ss = 0;
                ChatMessageListItemViewModel newchat2 = new ChatMessageListItemViewModel()
                {
                    Message = msg,
                    SendByMe = false
                };

                chatlist.Add(newchat2);

                MessageText.Clear();
            }
        }

        #region 형태소 나누기
        public static string TokenTosTringSample(string saysth)
        {
            string result;

            if (saysth != "종료")
            {
                var tokens = TwitterKoreanProcessorCS.Tokenize(saysth);
                var results = TwitterKoreanProcessorCS.TokensToStrings(tokens);

                result = string.Join(" / ", results);
            }
            else
            {
                result = "안녕히 가세요";
            }
            return result;
        }
        #endregion

        #region 단어 스플릿(시나리오)

        static string whoinfo = "어떤정보를 알려드릴까요?";

        //시나리오 하나씩 넣으면 됩니다
        public string Splitsth(string result)
        {
            string[] saying = result.Split(new char[] { '/' });



            return "정확한 이름을 입력해주세요!";
            //Console.WriteLine(i + "번째 단어는" + result1[i]);
            
        }
        public void Select_MemL(string msg)
        {
            try
            {
                string[] sp = msg.Split('/');

                for (int i = 0; i < sp.Length; i++)
                {
                    memlate = sp[i];

                }

            }
            catch { }
        }
        

        //mt[hh:mm:ss]
        public void Select_MemT(string msg)
        {
            try 
            {
                string[] sp = msg.Split('/');

                for (int i = 0; i < sp.Length; i++)
                {
                    try
                    {
                        mt.Add(DateTime.Parse(sp[i]));
                    }
                    catch { }

                }



                for (int t = 0; t < mt.Count; t++)
                {
                    try
                    {
                    
                        //spt[hh] spt[mm] spt[ss]
                        string sss = mt[t].ToString();
                        string[] spt = sss.Split(' ');
                        string[] sp1 = spt[2].Split(':');
                        hh += int.Parse(sp1[0]) * 3600;
                        mm += int.Parse(sp1[1]) * 60;
                        ss += int.Parse(sp1[2]);
                    }
                    catch { }

                }
                int avr = (hh + mm + ss) / mt.Count;

                TimeSpan timespan = TimeSpan.FromSeconds(avr);
                hh = timespan.Hours;
                mm = timespan.Minutes;
                ss = timespan.Seconds;
            }
            catch { }
        }

        public string Splitsth2(string result)
        {
            string[] saying = result.Split(new char[] { '/' });

            //Console.WriteLine(i + "번째 단어는" + result1[i]);

            if (saying.Contains("1 ") || saying.Contains("1") || saying.Contains(" 1"))
            {
                NameData(aaa1);

                Thread.Sleep(500);
                return aaa1 + "는 이번주에 " + memlate + "회 지각했어요";
            }

            else if (saying.Contains("2 ") || saying.Contains("2") || saying.Contains(" 2"))
            {
                TimeData(aaa1);

                Thread.Sleep(1000);
                return aaa1 + "의 평균 등교시간은" + hh +":"+ mm +":"+ ss + "입니다.";
            }

            else if (saying.Contains("안녕 ") || saying.Contains("안녕") || saying.Contains(" 안녕"))
            {
                return aaa1 + "님 안녕하세요.";
            }

            else if (saying.Contains("잘가 ") || saying.Contains("잘가") || saying.Contains(" 잘가"))
            {
                return aaa1 + "야 잘가.";
            }

            else
            {
                return "ex) 1. 일주일간 지각 횟수 \n     2. 일주일간 평균 등교시간";
            }
            
        }

        public string Splitsth_name(string result)
        {
            string[] saying = result.Split(new char[] { '/' });

            //Console.WriteLine(i + "번째 단어는" + result1[i]);

            if (saying.Contains(aaa1))
            {
                return whoinfo;
            }
            else
            {
                return "무슨 말인지 잘 모르겠는데요";
            }
        }


        public void Select_Name(string msg)
        {
            try
            {
                string[] sp = msg.Split('/');

                for (int i = 0; i < sp.Length; i++)
                {
                    string s = sp[i];
                    cn.Add(s);
                }

            }
            catch { }
        }
        #endregion




    }
}
