using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace Routes
{
    public class TraceService
    {
        const int timeout = 1000;
        const int maxTTL = 30;
        const int bufferSize = 32;
        public string Hostname { get; private set; }
        public List<Trace> Traces { get; private set; }

        public TraceService(string hostname)
        {
            Hostname = hostname;
            Traces = TraceRoute().ToList();
        }

        private IEnumerable<Trace> TraceRoute()
        {
            byte[] buffer = new byte[bufferSize];
            new Random().NextBytes(buffer);
            Ping pinger = new Ping();

            for (int ttl = 1; ttl <= maxTTL; ttl++)
            {
                PingOptions options = new PingOptions(ttl, true);
                PingReply reply = pinger.Send(Hostname, timeout, buffer, options);
                Trace trace = new Trace(reply);

                switch (reply.Status)
                {
                    case IPStatus.Success:
                        yield return trace;
                        break;
                    case IPStatus.TtlExpired:
                        yield return trace;
                        continue;
                    case IPStatus.TimedOut:
                        continue;
                    default:
                        continue;
                }
                break;
            }
        }
    }
}