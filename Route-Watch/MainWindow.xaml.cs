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
using System.Collections.ObjectModel;
using Routes;

namespace Route_Watch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private List<TraceService> Traces;
        private bool Running;
        private List<TraceService> TraceService;
        private ObservableCollection<Trace> Traces;

        public MainWindow()
        {
            InitializeComponent();
            InitalizeWindow();
        }

        private void InitalizeWindow()
        {
            Running = true;
            Traces = new ObservableCollection<Trace>();
            TraceService = new List<TraceService>();
            var foo = new Thread(() => Updater("drudgereport.com"));
            foo.Start();
            lvDataView.ItemsSource = Traces;
        }
        // TODO: data bind 
        private void Updater(string url)
        {
            while(Running)
            {
                if (Traces.Count > 30)
                    Traces.RemoveAt(0);

                var trace = new TraceService(url);
                
                // Dispatcher.Invoke(() => lvDataView.Items.Clear());

                foreach (Trace tr in trace.Traces)
                {
                    tr.Ping();
                    Dispatcher.Invoke(() => Traces.Add(tr));
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
