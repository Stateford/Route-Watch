using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace pingTest
{
    class ConsoleMain
    { 

        public static IEnumerable<PingReply> GetTraceRoute_reply(string hostname)
        {
            // following are the defaults for the "traceroute" command in unix.
            const int timeout = 10000;
            const int maxTTL = 30;
            const int bufferSize = 32;

            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);
            Ping pinger = new Ping();

            for (int ttl = 1; ttl <= maxTTL; ttl++)
            {
                PingOptions options = new PingOptions(ttl, true);
                PingReply reply = pinger.Send(hostname, timeout, buffer, options);


                switch (reply.Status)
                {
                    case IPStatus.Success:
                        yield return reply;
                        break;
                    case IPStatus.TtlExpired:
                        yield return reply;
                        continue;
                    case IPStatus.TimedOut:
                        continue;
                    default:
                        break;
                }

                
                break;
            }
        }

        static void Main(string[] args)
        {

            

            IEnumerable<PingReply> y = GetTraceRoute_reply("google.com");
            foreach (var i in y)
            {
                Console.WriteLine("Time: {0}", i.RoundtripTime);
                Console.WriteLine("Status: " + i.Status.ToString());
                try
                {
                    Console.WriteLine(Dns.GetHostEntry(i.Address).HostName.ToString());
                }
                catch { }
                Console.WriteLine(i.Address.ToString());
                Console.WriteLine("--------------------------");
            }

        }
    }
}
