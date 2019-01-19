using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMouseServer
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPServerAssinc.ServerStart();
            Console.WriteLine("Em execução");
            
            while (true);
        }
    }
}
