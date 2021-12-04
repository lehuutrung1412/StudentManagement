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
        private const string CookieDownloadFile = "accountToken=" + Token;

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

        public void DownloadFileAsync(string link, string fileName)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, CookieDownloadFile);
            webClient.DownloadFileAsync(new Uri(link), fileName);
        }

        public async Task<byte[]> DownloadFileAsByteAsync(string link)
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add(HttpRequestHeader.Cookie, CookieDownloadFile);
            return await webClient.DownloadDataTaskAsync(new Uri(link));
        }
    }
}
