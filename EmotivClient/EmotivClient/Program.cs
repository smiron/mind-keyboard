using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EmotivClient
{
    class Program
    {
        static void Main(string[] args)
        {

            var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var ipep = new IPEndPoint(IPAddress.Loopback, 9008);

            s.Bind(ipep);

            var data = new byte[1024];

            while (true)
            {
                var result = s.Receive(data);

                if (result > 0)
                {
                    var stringData = Encoding.ASCII.GetString(data, 0, result);

                    string[] parameters = stringData.Split(',');

                    Console.WriteLine(" GyroX={0}, GyroY={1} ", parameters[2], parameters[3]);
                }

            }
        }
    }
}
