using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sharp_net {

    public class WebRequestRobot {
        const string userAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36";
        public void DownloadImage(string uri, string filePath) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if (response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) {
                // 抽屉的bug http://img1.chouti.com/group5/M03/74/6C/wKgCEVJXEiDoGigtAA7ki98-vN4272.jpg
                // response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase)

                // if the remote file was found, download oit
                string fileName = Path.GetFileName(filePath);
                string path = filePath.Replace(fileName, "");
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(filePath)) {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
        }

        private void InjectWeiboCookie(HttpWebRequest request) {
            request.CookieContainer = new CookieContainer();
            string cookieRaw = ConfigurationManager.AppSettings["WeiboCookie"];
            string[] cookiestrs = cookieRaw.Split(';');
            foreach (string cookiestr in cookiestrs) {
                string[] cookiekv = cookiestr.Trim().Split('=');
                var cookie = new Cookie(cookiekv[0], cookiekv[1], "/", ".weibo.com");
                request.CookieContainer.Add(cookie);
            }
        }
        public async Task<string> WeiboShortUrl(string url) {
            var wsuRequest = WebRequest.Create("http://weibo.com/aj/mblog/video?_wv=5&url=" + url) as HttpWebRequest;
            if (wsuRequest == null)
                return string.Empty;// ignore file
            wsuRequest.UserAgent = userAgent;
            InjectWeiboCookie(wsuRequest);
            try {
                using (var wsuResponse = await wsuRequest.GetResponseAsync() as HttpWebResponse) {
                    if (wsuResponse.StatusCode != HttpStatusCode.OK)
                        return string.Empty;
                    string responseData = new StreamReader(wsuResponse.GetResponseStream()).ReadToEnd();
                    var wsuObj = JsonConvert.DeserializeObject<WeiboShartUrlResponse>(responseData);
                    return wsuObj.data.url;
                }
            } catch (Exception) {
                return string.Empty;
            }
        }

        //http://www.cnblogs.com/e241138/archive/2012/12/16/2820054.html
        public async Task<string> AnalysisVideoUrl(string url) {
            string weiboShortUrl = await WeiboShortUrl(url);
            if (string.IsNullOrEmpty(weiboShortUrl))
                return string.Empty;

            string videoUrl = string.Format("http://api.weibo.com/widget/show.jsonp?vers=3&lang=zh-cn&short_url={0}&template_name=embed&source=2292547934", weiboShortUrl);
            var videoRequest = WebRequest.Create(videoUrl) as HttpWebRequest;
            if (videoRequest == null)
                return string.Empty;// ignore file
            videoRequest.UserAgent = userAgent;
            InjectWeiboCookie(videoRequest);

            try {
                using (var videoResponse = await videoRequest.GetResponseAsync() as HttpWebResponse) {
                    if (videoResponse.StatusCode != HttpStatusCode.OK) return string.Empty;
                    string responseData = new StreamReader(videoResponse.GetResponseStream()).ReadToEnd();
                    var videoObj = JsonConvert.DeserializeObject<WeiboVideoResponse>(responseData);
                    return videoObj.result;
                }
            } catch (WebException) {
                return string.Empty;
            }
        }

        public async Task<string> GetWebPageTitle(string url) {
            var request = HttpWebRequest.Create(url) as HttpWebRequest;

            // If the request wasn't an HTTP request (like a file), ignore it
            if (request == null) return string.Empty;

            // Use the user's credentials
            request.UseDefaultCredentials = true;

            // Obtain a response from the server, if there was an error, return nothing
            try {
                using (var response = await request.GetResponseAsync() as HttpWebResponse) {
                    if (new List<string>(response.Headers.AllKeys).Contains("Content-Type")) {
                        if (response.Headers["Content-Type"].StartsWith("text/html")) {
                            string responseData;
                            if(response.ContentType.Contains("GBK"))
                                responseData = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312")).ReadToEnd();
                            else
                                responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();
                            Regex regex = new Regex(@"(?<=<title.*>)([\s\S]*)(?=</title>)", RegexOptions.IgnoreCase);
                            return regex.Match(responseData).Value.Trim();
                        }
                    }
                }
            } catch (WebException) {
                return string.Empty;
            }
            return string.Empty;
        }

        private class WeiboShartUrlResponse {
            public string code { get; set; }
            public string msg { get; set; }
            public WeiboShartUrlData data { get; set; }
        }

        private class WeiboShartUrlData{
            public string url { get; set; }
            public string title { get; set; }
        }

        private class WeiboVideoResponse {
            public string result { get; set; }
        }
    }

}