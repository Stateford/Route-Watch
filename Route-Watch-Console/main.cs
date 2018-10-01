using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using Trace;
using System.Threading;





namespace pingTest
{
    class ConsoleMain
    { 

        static void Main(string[] args)
        {


            TraceService x = new TraceService("drudgereport.com");

            foreach(var trace in x.Traces)
            {
                Console.WriteLine($"{trace.IPRoutes.Address.ToString()}  {trace.IPRoutes.RoundtripTime}ms  {trace.Hostname}");
            }

            while(true)
            {
                Console.WriteLine("-------------------");
                foreach (var trace in x.Traces)
                {
                    trace.Ping();
                    Console.WriteLine($"{trace.IPRoutes.Address.ToString()}  {trace.roundTripTime}ms  {trace.Hostname}");
                }

                System.Threading.Thread.Sleep(1000);
            }

            

        }
    }
}
