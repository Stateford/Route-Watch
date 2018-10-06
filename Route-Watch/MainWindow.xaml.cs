using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();
            InitalizeWindow();
        }

        private void InitalizeWindow()
        {
            Traces = new List<TraceService>();
        }
    }
}
