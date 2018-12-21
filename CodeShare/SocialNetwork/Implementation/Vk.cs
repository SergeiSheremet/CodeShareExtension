using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Auth;

namespace CodeShare.SocialNetwork.Implementation
{
    class Vk : ISocialNetwork
    {
        private readonly Vkontakte _vkLibrary;
        private readonly string _accessTokenString =
            "ee424c4bcc0208b1cdf71ca5d5def9749d302e42da78e6ba44b04c2f355a2e927ea83afd4c0c35831adde";

        public Vk()
        {
            _vkLibrary = new Vkontakte(6793423) {AccessToken = AccessToken.FromString(_accessTokenString)};
        }

        public void LogIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public void SendUrl(string url)
        {
            var ok = _vkLibrary.Messages.Send(userId: 142477265, message: url).Result;
        }
    }
}
