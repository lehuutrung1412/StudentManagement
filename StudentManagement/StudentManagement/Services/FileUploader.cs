using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Services
{
    public class FileUploader
    {
        private static FileUploader s_instance;

        public static FileUploader Instance => s_instance ?? (s_instance = new FileUploader());

        private HttpClient _client;

        private const string UrlGetServer = "https://api.gofile.io/getServer";
        private const string UrlUpload = ".gofile.io/uploadFile";

        public FileUploader()
        {
            _client = new HttpClient();
        } 

        public string Upload(string file)
        {
            var server = GetServer();
            if (server != null)
            {
                var url = "https://" + server + UrlUpload;
                //Dictionary<string, string> parameters = new Dictionary<string, string>();
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new ByteArrayContent(File.ReadAllBytes(file)), "file", Path.GetFileName(file));
                HttpResponseMessage response = _client.PostAsync(url, form).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    JObject json = JObject.Parse(content);
                    return (string)json["data"]["directLink"];
                }
            }
            return null;
        }

        public string GetServer()
        {
            HttpResponseMessage response = _client.GetAsync(UrlGetServer).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(content);
                return (string)json["data"]["server"];
            }
            return null;
        }
    }
}
