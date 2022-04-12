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

namespace TotallyObedientMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private int cols = 8;
        private int rows = 64;

        private List<Memory> memory;

        private Label[] labelArr = new Label[512];

        public MainWindow()
        {
            InitializeComponent();
            memory = new List<Memory>();

        }
    }
}
