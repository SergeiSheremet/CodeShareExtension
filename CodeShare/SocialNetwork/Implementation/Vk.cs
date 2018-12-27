using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using VkLibrary.Core;
using VkLibrary.Core.Auth;

namespace CodeShare.SocialNetwork.Implementation
{
    class Vk : ISocialNetwork
    {
        private readonly Vkontakte _vkLibrary;
        private const string AccessToken = 
            "ee424c4bcc0208b1cdf71ca5d5def9749d302e42da78e6ba44b04c2f355a2e927ea83afd4c0c35831adde";

        public Vk()
        {
            _vkLibrary = new Vkontakte(6793423, apiVersion: "5.80") {AccessToken = VkLibrary.Core.Auth.AccessToken.FromString(AccessToken)};
        }

        public void LogIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public async void SendUrl(string url)
        {
            var friends = await _vkLibrary.Friends.Get();
            FriendChoose choiceWindow = new FriendChoose {ListBox = {ItemsSource = friends.Items.OrderBy(x => x.FirstName)}};
            choiceWindow.ShowDialog();
            //TODO: Test if window is closed
            var ok = await _vkLibrary.Messages.Send(userId: choiceWindow.SelectedId, message: url);
        }
    }
}
