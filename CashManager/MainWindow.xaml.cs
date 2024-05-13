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

   

        private void OpenOtherMenuCategory(object sender, MouseButtonEventArgs e)
        {
            otherMenuCategoryPopup.IsOpen = true;
        }

        private void otherMenuCategoryPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            otherMenuCategoryPopup.IsOpen = false;
        }

       

        private void otherMenuCashPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            otherMenuCashPopup.IsOpen = false;
        }

        private void OpenOtherMenuCash(object sender, MouseButtonEventArgs e)
        {
            otherMenuCashPopup.IsOpen = true;
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if((sender as Hyperlink).Name== "danaHyper")
            {
                Process.Start(new ProcessStartInfo("mailto:antoniukdana0608@gmail.com") { UseShellExecute = true });
            }
            else
            {
                Process.Start(new ProcessStartInfo("mailto:hasiukiv@gmail.com") { UseShellExecute = true });
            }
          
        }

       
    }
}