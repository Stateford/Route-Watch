using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace Routes
{
    public class Trace
    {
        private const int timeout = 1000;
        private const int maxTTL = 30;
        private const int bufferSize = 32;
        private long minPing = 0;
        private long maxPing = 0;
        private long totalPing = 0;
        private int attemptedPings = 0;
        private int successfulPings = 0;
        private int failedPings = 0;

        public PingReply IPRoutes { get; private set; }
        public string Hostname { get; private set; }
        public string IPAddress
        {
            get { return IPRoutes.Address.ToString(); }
        }

        public double AveragePing
        {
            get
            {
                if (successfulPings <= 0)
                    return 0;

                return totalPing / successfulPings;
            }
        }

        public double PacketLoss
        {
            get { return (double)(failedPings / attemptedPings); }
        }

        public int Errors
        {
            get { return failedPings; }
        }

        public long RoundTripTime { get; private set; }

        public Trace(PingReply reply)
        {
            IPRoutes = reply;
            try
            {
                Hostname = Dns.GetHostEntry(reply.Address).HostName;
            }
            catch
            {
                Hostname = "***";
            }
        }

        public void Ping()
        {
            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);

            using (Ping pinger = new Ping())
            {
                PingOptions options = new PingOptions(maxTTL, true);
                var tempReply = pinger.Send(IPRoutes.Address, timeout, buffer, options);

                attemptedPings++;

                if(tempReply.Status == IPStatus.TimedOut)
                {
                    RoundTripTime = -1;
                    failedPings++;
                }
                else
                {
                    RoundTripTime = tempReply.RoundtripTime;

                    if (maxPing < tempReply.RoundtripTime)
                        maxPing = tempReply.RoundtripTime;

                    if (minPing != 0 && minPing > tempReply.RoundtripTime)
                        minPing = tempReply.RoundtripTime;

                    totalPing += tempReply.RoundtripTime;
                    successfulPings++;
                }
            }
        }


    }      
}
