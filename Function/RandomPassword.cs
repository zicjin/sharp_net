using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace sharp_net {
    //Reference http://www.consultsarath.com/contents/articles/KB000011-snippet-generate-random-strong-password-string-using-cSharp.aspx
    public static class RandomPassword {

        static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
        static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
        static string numerics = "1234567890";
        static string special = "@#$";

        //Simple Random Password:
        private static string allowChars = alphaCaps + alphaLow + numerics;
        public static string GeneratePassword(int length) {
            Random r = new Random((int)DateTime.Now.Ticks);
            String generatedPassword = "";
            for (int i = 0; i < length; i++) {
                double rand = r.NextDouble();
                if (i == 0) {
                    generatedPassword += alphaCaps.ToCharArray()[(int)Math.Floor(rand * alphaCaps.Length)];
                } else {
                    generatedPassword += allowChars.ToCharArray()[(int)Math.Floor(rand * allowChars.Length)];
                }
            }
            return generatedPassword;
        }

        //Strong Password Generation:
        private static string strongAllowChars = alphaCaps + alphaLow + numerics + special;
        public static string GenerateStrongPassword(int length) {
            Random r = new Random((int)DateTime.Now.Ticks);
            String generatedPassword = "";
            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            pLower = getRandomPosition(r, ref posArray);
            pUpper = getRandomPosition(r, ref posArray);
            pNumber = getRandomPosition(r, ref posArray);
            pSpecial = getRandomPosition(r, ref posArray);
            for (int i = 0; i < length; i++) {
                if (i == pLower)
                    generatedPassword += getRandomChar(r, alphaCaps);
                else if (i == pUpper)
                    generatedPassword += getRandomChar(r, alphaLow);
                else if (i == pNumber)
                    generatedPassword += getRandomChar(r, numerics);
                else if (i == pSpecial)
                    generatedPassword += getRandomChar(r, special);
                else
                    generatedPassword += getRandomChar(r, strongAllowChars);
            }
            return generatedPassword;
        }

        private static string getRandomChar(Random r, string fullString) {
            return fullString.ToCharArray()[(int)Math.Floor(r.NextDouble() * fullString.Length)].ToString();
        }

        private static int getRandomPosition(Random r, ref string posArray) {
            int pos;
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble()
                                           * posArray.Length)].ToString();
            pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }

        //Password By Extension(be must 8 char, alphaLow and number):
        public static string GenerateStrongPassword() {
            return Path.ChangeExtension(Path.GetRandomFileName(), null);
        }
    }
}
