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
        private readonly HttpClient _client;
        private string _accessToken;

        public GithubGists()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Mozilla", "5.0"));
        }

        public bool LogIn()
        {
            if (_accessToken != null)
            {
                return true;
            }

            var filepath = Environment.CurrentDirectory.Replace(@"\bin\Debug", "") + @"\Resources\login.json";
            var jsonDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filepath));

            if (jsonDictionary.ContainsKey("gists"))
            {
                _accessToken = jsonDictionary["gists"];
                return true;
            }

            var authWindow = new AuthWindow("Gists");
            authWindow.ShowDialog();

            if (authWindow.AccessToken != null)
            {
                _accessToken = authWindow.AccessToken;
                jsonDictionary.Add("gists", authWindow.AccessToken);
                File.WriteAllText(filepath, JsonConvert.SerializeObject(jsonDictionary));

                return true;
            }

            return false;
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public string CreatePaste(TextViewSelection textSelection)
        {
            if (!LogIn())
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
    }
}
