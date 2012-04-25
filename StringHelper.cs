using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web;
using System.IO;

namespace zic_dotnet {
    public static class StringHelper {
        public static string NoHtml(string htmlstring) {
            if (string.IsNullOrEmpty(htmlstring)) {
                return string.Empty;
            }
            //删除脚本
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"<", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @">", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"\r\n", "", RegexOptions.IgnoreCase);
            htmlstring = HttpUtility.HtmlEncode(htmlstring).Trim();
            return htmlstring;
        }

        public static string GenerateRandomString(int length) {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < length; i++) {
                char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(25 * random.NextDouble() + 75)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
