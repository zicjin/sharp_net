using System.Net;

namespace zic_dotnet {

    public static class RestClient {

        //http://msdn.microsoft.com/zh-cn/library/system.net.webclient(v=VS.100).aspx
        public static WebClient client { get; set; }

        static RestClient() {
            client = new WebClient { Encoding = System.Text.Encoding.UTF8 };
            client.Headers.Add("user-agent", ".NET/4.0");
            client.Headers.Add("content-type", "application/json; charset=utf-8");
        }

        public static string Get(string uri) {
            return client.DownloadString(uri);
        }

        public static string Post(string uri, string data) {
            return client.UploadString(uri, "POST", data);
        }

        public static string DELETE(string uri, string data) {
            return client.UploadString(uri, "DELETE", data);
        }

        public static string PUT(string uri, string data) {
            return client.UploadString(uri, "PUT", data);
        }
    }
}