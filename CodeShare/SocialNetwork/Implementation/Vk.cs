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
        private readonly string _storagePath;
        public bool IsAuthorized { get; private set; }

        public Vk()
        {
            _storagePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + @"\Resources\login.json";
            _vkLibrary = new Vkontakte(6793423, apiVersion: "5.80");

            _vkLibrary.AccessToken = AccessToken.FromString(CheckStorage());
            IsAuthorized = _vkLibrary.AccessToken.Token != null;
        }

        public bool LogIn()
        {
            var authWindow = new AuthWindow("VK");
            authWindow.ShowDialog();

            if (authWindow.AccessToken != null)
            {
                _vkLibrary.AccessToken = AccessToken.FromString(authWindow.AccessToken);

                var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_storagePath));
                jsonDictionary.Add("vk", authWindow.AccessToken);
                File.WriteAllText(_storagePath, JsonConvert.SerializeObject(jsonDictionary));

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
            if (!IsAuthorized && !LogIn())
            {
                return;
            }

            var friends = await _vkLibrary.Friends.Get();
            FriendChoose choiceWindow = new FriendChoose { ListBox = {ItemsSource = friends.Items.OrderBy(x => x.FirstName)}};
            choiceWindow.ShowDialog();

            var ok = await _vkLibrary.Messages.Send(userId: choiceWindow.SelectedId, message: url);
        }

        private string CheckStorage()
        {
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_storagePath));

            return jsonDictionary.ContainsKey("vk") ? jsonDictionary["vk"] : null;
        }
    }
}
