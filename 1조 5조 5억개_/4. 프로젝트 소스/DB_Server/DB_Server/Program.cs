using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server();

            s.ServerRun(1234);

            var command = Console.ReadLine();
           
            if (command == "종료")
            {
                s.ServerStop();
            }
        }
    }
}
