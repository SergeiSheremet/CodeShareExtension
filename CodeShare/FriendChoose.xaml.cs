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
using VkLibrary.Core.Types.Users;

namespace CodeShare
{
    /// <summary>
    /// Interaction logic for FriendChoose.xaml
    /// </summary>
    public partial class FriendChoose : Window
    {
        public int? SelectedId { get; private set; }

        public FriendChoose()
        {
            InitializeComponent();
        }

        private void Send(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItems.Count > 0)
            {
                var user = ListBox.SelectedItem as UserFull;
                SelectedId = user?.Id;

                Close();
            }
        }
    }
}
