using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CodeShare.Model;
using CodeShare.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.Auth;

namespace CodeShare.Hosting.Implementation
{
    class GithubGists : ICodeHosting
    {
        private readonly string _storagePath;
        private readonly HttpClient _client;
        private string _accessToken;
        public bool IsAuthorized { get; private set; }

        public GithubGists()
        {
            _storagePath = Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + @"\Resources\login.json";

            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Mozilla", "5.0"));

            _accessToken = CheckStorage();
            IsAuthorized = _accessToken != null;
        }

        public bool LogIn()
        {
            var authWindow = new AuthWindow("Gists");
            authWindow.ShowDialog();

            if (authWindow.AccessToken != null)
            {
                _accessToken = authWindow.AccessToken;

                var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_storagePath));
                jsonDictionary.Add("gists", authWindow.AccessToken);
                File.WriteAllText(_storagePath, JsonConvert.SerializeObject(jsonDictionary));

                IsAuthorized = true;
                return IsAuthorized;
            }

            return IsAuthorized;
        }

        public void LogOut()
        {
            _accessToken = null;
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_storagePath));

            if (jsonDictionary.ContainsKey("vk"))
            {
                jsonDictionary.Remove("vk");
                File.WriteAllText(_storagePath, JsonConvert.SerializeObject(jsonDictionary));
            }

            IsAuthorized = false;
        }

        public string CreatePaste(TextViewSelection textSelection)
        {
            if (!IsAuthorized && !LogIn())
            {
                return null;
            }

            var data = new GistsRequestData
            {
                Description = $"From Line {textSelection.StartPosition.Line} Column {textSelection.StartPosition.Column} to Line {textSelection.EndPosition.Line} Column {textSelection.EndPosition.Line}",
                Files = new Dictionary<string, GistsRequestData.GistsFileObject>
                {
                    [textSelection.Filename] = new GistsRequestData.GistsFileObject
                    {
                        Content = textSelection.Text
                    }
                }
            };

            var jsonString = JsonConvert.SerializeObject(data);

            using (var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json"))
            {
                var response = _client.PostAsync($"https://api.github.com/gists?access_token={_accessToken}", httpContent).Result;
                var responseAsString = response.Content.ReadAsStringAsync().Result;
                var url = JObject.Parse(responseAsString)["html_url"].ToString();
                return url;
            }
        }

        private string CheckStorage()
        {
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(_storagePath));

            return jsonDictionary.ContainsKey("gists") ? jsonDictionary["gists"] : null;
        }
    }
}
