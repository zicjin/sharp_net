using System;
using System.IO;
using System.Linq;

namespace zic_dotnet {

    public static class ExpendString {

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
    }
}