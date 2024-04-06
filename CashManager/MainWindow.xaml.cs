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

        private void ListBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
                // Запуск анімації для gridCashCreate
                var storyboard = FindResource("ShowCreateCash") as Storyboard;
                if (storyboard != null)
                {
                    Storyboard.SetTarget(storyboard, gridCashCreate);
                    storyboard.Begin();
                }
            
        }
    }
}