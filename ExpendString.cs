using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace zic_dotnet {

    public static class ExpendString {

        public static string EmailLinkBtn(this string email) {
            if (email.Split('@')[1] == "gmail.com") {
                return "<a href='http://" + email.Split('@')[1] + "'>" + email + "</a>";
            } else {
                return "<a href='http://mail." + email.Split('@')[1] + "'>" + email + "</a>";
            }
        }

        public static int LengthCar(this string input) {
            if (string.IsNullOrEmpty(input))
                return 0;
            int n = 0;
            foreach (char c in input) {
                if ((int)c > 256)
                    n += 2;
                else
                    n++;
            }
            return n;
        }

        public static string Interception(this string input, int startIndex, int length) {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            int len = input.Length;
            if (startIndex >= len)
                return string.Empty;

            bool flag = false;
            int i = startIndex, j = 0;
            for (; i < len; i++) {
                if (j >= length) {
                    flag = true;
                    break;
                }

                if ((int)input[i] > 256)
                    j += 2;
                else
                    j++;
            }

            string returnValue = input.Substring(startIndex, i - startIndex);
            if (flag)
                returnValue += "...";
            return returnValue;
        }

        public static string AppPath(this string input) {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, input);
        }

        public static int[] ConvertToIntArry(this string[] input) {
            return input.ToList().ConvertAll(new Converter<string, int>(Convert.ToInt32)).ToArray();
        }

        public static string[] SplitBystr(this string value, string limit, char? templimit) {
            if (templimit.HasValue) {
                value = value.Replace(limit, templimit.Value.ToString());
                return value.Split(templimit.Value);
            } else {
                value = value.Replace(limit, "`");
                return value.Split('`');
            }
        }

        public static string AppendSplit(this string value, string split, string append) {
            if (String.IsNullOrEmpty(value))
                return String.Empty;
            value.Replace(split, "");
            if (String.IsNullOrEmpty(append))
                return value;
            append.Replace(split, "");
            if(append.StartsWith(split))
                return value + append;
            return value + split + append;
        }

        public static string NoHtml(this string value) {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            value = Regex.Replace(value, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"-->", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"<!--.*", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"<", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @">", "", RegexOptions.IgnoreCase);
            value = Regex.Replace(value, @"\r\n", "", RegexOptions.IgnoreCase);
            return HttpUtility.HtmlEncode(value).Trim();
        }
    }
}