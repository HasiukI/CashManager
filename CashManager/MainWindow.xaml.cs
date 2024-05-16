using CashManager.ViewModel;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace CashManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

   

       

        private void ChangeGraph(object sender, MouseButtonEventArgs e)
        {

            if ((sender as Border).Name == "Stovb_Bord")
            {
                diagramStovb.Visibility = Visibility.Visible;
                diagramCircle.Visibility = Visibility.Hidden;

            }
            else
            {
                diagramStovb.Visibility = Visibility.Hidden;
                diagramCircle.Visibility = Visibility.Visible;
            }
        }


       
    }
}