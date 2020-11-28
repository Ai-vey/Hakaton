using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using WpfApp1.Pages;





namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.MainPage());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)

                MainFrame.GoBack();
        }

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void MainFrame_ContentRendered_1(object sender, EventArgs e)
        {
            var page = (sender as Frame).Content as Page;

            if (page is Pages.MainPage)
            {
                BtnBack.Visibility = Visibility.Collapsed;

            }
            else
            {
                BtnBack.Visibility = Visibility.Visible;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
    
    
}
