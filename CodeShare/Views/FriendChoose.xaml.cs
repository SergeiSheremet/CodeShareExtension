using System.Windows;
using VkLibrary.Core.Types.Users;

namespace CodeShare.Views
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
