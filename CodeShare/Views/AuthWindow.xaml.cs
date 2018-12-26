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

namespace CodeShare.Views
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public string AccessToken { get; private set; }

        public AuthWindow(string resource)
        {
            InitializeComponent();
            ResourceLabel.Content = resource;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (TokenTextBox.Text != string.Empty)
            {
                AccessToken = TokenTextBox.Text;

                Close();
            }
        }
    }
}
