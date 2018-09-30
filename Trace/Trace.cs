using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace Trace
{
    public class Trace
    {
        private const int timeout = 1000;
        private const int maxTTL = 30;
        private const int bufferSize = 32;

        public PingReply IPRoutes { get; private set; }
        public string Hostname { get; private set; }

        public long roundTripTime { get; private set; }

        public Trace(PingReply reply)
        {
            IPRoutes = reply;
            try
            {
                Hostname = Dns.GetHostEntry(reply.Address).HostName;
            }
            catch { }
        }

        public void Ping()
        {
            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);

            using (Ping pinger = new Ping())
            {
                PingOptions options = new PingOptions(maxTTL, true);
                var tempReply = pinger.Send(IPRoutes.Address, timeout, buffer, options);

                if(tempReply.Status == IPStatus.TimedOut)
                    this.roundTripTime = -1;
                else
                    this.roundTripTime = tempReply.RoundtripTime;
            }

            
        }
    }


        
}
