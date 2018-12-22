using System;
using System.IO;
using CodeShare.Model;
using Newtonsoft.Json;
using PastebinAPI;

namespace CodeShare.Hosting.Implementation
{
    class PasteBin : ICodeHosting
    {
        private User _user;

        public PasteBin()
        {
            Pastebin.DevKey = "d440d7f8682b5a9b5671d6472a247e1c";
        }

        public void LogIn(string username, string password)
        {
            _user = Pastebin.LoginAsync(username, password).Result;
        }

        public void LogOut()
        {
            _user = null;
            //TODO: clear json
        }

        public string CreatePaste(TextViewSelection textSelection)
        {
            if (_user == null)
            {
                var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(File.ReadAllText(Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + @"\Resources\login.json"));
                LogIn(loginInfo.Username, loginInfo.Password);
            }

            // TODO: language
            //Paste paste = _user.CreatePasteAsync(code, title, Language.Default, Visibility.Public, Expiration.Never).Result;
            return null;//paste.Url;
        }
    }
}
