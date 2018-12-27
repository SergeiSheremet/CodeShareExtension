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
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string AccessToken { get; private set; }
        public AuthType AuthenticationType { get; private set; }

        public AuthWindow()
        {
            InitializeComponent();
            AuthenticationType = AuthType.Login;
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (AuthenticationType == AuthType.Login && LoginTextBox.Text != string.Empty && PasswordTextBox.Text != string.Empty)
            {
                Login = LoginTextBox.Text;
                Password = PasswordTextBox.Text;

                Close();
            }
            else if (TokenTextBox.Text != string.Empty)
            {
                AccessToken = TokenTextBox.Text;

                Close();
            }
        }

        private void ChangeAuthType(object sender, RoutedEventArgs e)
        {
            if (AuthenticationType == AuthType.Login)
            {
                LoginLabel.Visibility = Visibility.Hidden;
                LoginTextBox.Visibility = Visibility.Hidden;
                PasswordLabel.Visibility = Visibility.Hidden;
                PasswordTextBox.Visibility = Visibility.Hidden;

                Height -= 50;

                TokenLabel.Visibility = Visibility.Visible;
                TokenTextBox.Visibility = Visibility.Visible;

                ChangeTypeButton.Content = "Use Login";
                AuthenticationType = AuthType.Token;
            }
            else
            {
                TokenLabel.Visibility = Visibility.Hidden;
                TokenTextBox.Visibility = Visibility.Hidden;

                Height += 50;

                LoginLabel.Visibility = Visibility.Visible;
                LoginTextBox.Visibility = Visibility.Visible;
                PasswordLabel.Visibility = Visibility.Visible;
                PasswordTextBox.Visibility = Visibility.Visible;

                ChangeTypeButton.Content = "Use Access Token";
                AuthenticationType = AuthType.Login;
            }
        }

        public enum AuthType
        {
            Login,
            Token
        }
    }
}
