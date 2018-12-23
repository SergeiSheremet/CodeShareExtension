using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CodeShare.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodeShare.Hosting.Implementation
{
    class GithubGists : ICodeHosting
    {
        private readonly HttpClient _client;
        private const string AccessToken =
            "62b04b1eae6379425796e4ca2fea282eb49e2aea";

        public GithubGists()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("Mozilla", "5.0"));
        }

        public void LogIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public string CreatePaste(TextViewSelection textSelection)
        {
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
                var response = _client.PostAsync($"https://api.github.com/gists?access_token={AccessToken}", httpContent).Result;
                var responseAsString = response.Content.ReadAsStringAsync().Result;
                var url = JObject.Parse(responseAsString)["html_url"].ToString();
                return url;
            }
        }
    }
}
