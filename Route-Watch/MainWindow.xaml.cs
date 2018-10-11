using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Routes;

namespace Route_Watch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TraceService> Traces;
        private bool Running;

        public MainWindow()
        {
            InitializeComponent();
            InitalizeWindow();
        }

        private void InitalizeWindow()
        {
            Running = true;
            Traces = new List<TraceService>();
            var foo = new Thread(() => Updater("drudgereport.com"));
            foo.Start();
        }
        // TODO: data bind 
        private void Updater(string url)
        {
            while(Running)
            {
                if (Traces.Count > 30)
                    Traces.RemoveAt(0);

                var trace = new TraceService(url);
                Traces.Add(trace);

                Dispatcher.Invoke(() => lvDataView.Items.Clear());

                foreach (var tr in trace.Traces)
                {
                    tr.Ping();
                    Dispatcher.Invoke(() => lvDataView.Items.Add(tr.ListView()));
                }
                Thread.Sleep(5000);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Running = false;
        }
    }
}
