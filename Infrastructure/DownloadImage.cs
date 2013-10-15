using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net.Infrastructure {
    public static class DownloadImage {
        public static void Execute(string uri, string filePath) {
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
    }
}
