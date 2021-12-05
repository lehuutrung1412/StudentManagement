using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using StudentManagement.Utils;
using System.Net;

namespace StudentManagement.Services
{
    public class FileUploader
    {
        private static FileUploader s_instance;

        public static FileUploader Instance => s_instance ?? (s_instance = new FileUploader());

        private HttpClient _client;

        private const string UrlGetServer = "https://api.gofile.io/getServer";
        private const string UrlUpload = ".gofile.io/uploadFile";
        private const string Token = "mNLWYdMzHz6QIDEIaDKy4hQs5do0ee48";
        private const string FolderId = "aff21df2-9b10-4be4-aaab-9075c6cadb2a";
        private const string TempToken = "PH3cde6ousy5TsCzYeOnMOvOt3yXe2TG";

        public FileUploader()
        {
            _client = new HttpClient();
        }

        public async Task<string> UploadAsync(string file)
        {
            var server = await GetServerAsync();
            if (server != null)
            {
                var url = "https://" + server + UrlUpload;
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(File.ReadAllBytes(file)), "file", VietnameseStringNormalizer.Instance.Normalize(Path.GetFileName(file)));
                form.Add(new StringContent(Token), "token");
                form.Add(new StringContent(FolderId), "folderId");
                var response = await _client.PostAsync(url, form);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    return (string)json["data"]["directLink"];
                }
            }
            return null;
        }

        public async Task<string> GetServerAsync()
        {
            var response = await _client.GetAsync(UrlGetServer);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                return (string)json["data"]["server"];
            }
            return null;
        }

        public async Task<string> GetTokenAsync()
        {
            var server = await GetServerAsync();
            if (server != null)
            {
                var url = "https://" + server + ".gofile.io/createAccount";
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    return (string)json["data"]["token"];
                }
            }
            return null;
        }

        public async Task<bool> IsAccountWorkingAsync(string token)
        {
            if (token != null)
            {
                var url = "https://api.gofile.io/getAccountDetails?token=" + token;
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    return (string)json["status"] == "ok";
                }
            }
            return false;
        }

        public async Task<bool> RefreshAccountAsync(string token)
        {
            if (token != null)
            {
                var url = "https://api.gofile.io/getContent?contentId=" + FolderId + "&token=" + token + "&websiteToken=websiteToken";
                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> GetCookieAsync(string token)
        {
            if (await IsAccountWorkingAsync(token))
            {
                await RefreshAccountAsync(token);
                return "accountToken=" + token;
            }
            else
            {
                var newToken = await GetTokenAsync();
                await RefreshAccountAsync(newToken);
                return "accountToken=" + newToken;
            }
        }

        public async Task DownloadFileAsync(string link, string fileName)
        {
            var url = new Uri(link);
            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, await GetCookieAsync(TempToken));
            await webClient.DownloadFileTaskAsync(url, fileName);
        }

        public async Task<byte[]> DownloadFileAsByteAsync(string link)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, await GetCookieAsync(TempToken));
            return await webClient.DownloadDataTaskAsync(new Uri(link));
        }
    }
}
