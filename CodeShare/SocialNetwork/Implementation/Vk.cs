using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeShare.Views;
using EnvDTE;
using Newtonsoft.Json;
using VkLibrary.Core;
using VkLibrary.Core.Auth;

namespace CodeShare.SocialNetwork.Implementation
{
    class Vk : ISocialNetwork
    {
        private readonly Vkontakte _vkLibrary;
       
        public Vk()
        {
            _vkLibrary = new Vkontakte(6793423, apiVersion: "5.80");
        }

        public bool LogIn()
        {
            if (_vkLibrary.AccessToken != null)
            {
                return true;
            }

            var filepath = Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + @"\Resources\login.json";
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepath));

            if (jsonDictionary.ContainsKey("vk"))
            {
                _vkLibrary.AccessToken = AccessToken.FromString(jsonDictionary["vk"]);
                return true;
            }

            var authWindow = new AuthWindow("VK");
            authWindow.ShowDialog();

            if (authWindow.AccessToken != null)
            {
                _vkLibrary.AccessToken = AccessToken.FromString(authWindow.AccessToken);
                jsonDictionary.Add("vk", authWindow.AccessToken);
                File.WriteAllText(filepath, JsonConvert.SerializeObject(jsonDictionary));

                return true;
            }

            return false;
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public async void SendUrl(string url)
        {
            if (!LogIn())
            {
                return;
            }

            var friends = await _vkLibrary.Friends.Get();
            FriendChoose choiceWindow = new FriendChoose { ListBox = {ItemsSource = friends.Items.OrderBy(x => x.FirstName)}};
            choiceWindow.ShowDialog();

            var ok = await _vkLibrary.Messages.Send(userId: choiceWindow.SelectedId, message: url);
        }
    }
}
