using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CSharpcommon {
    public static class Encrypt {

        public static string EncryptUserPassword(string pToEncrypt) {
            string strEncrypt = Md5Encrypt((AesEncrypt(pToEncrypt, true)));
            return strEncrypt;
        }

        public static string EncryptTradingPassword(string pToEncrypt) {
            string strEncrypt = DesEncrypt(DesEncrypt(DesEncrypt(pToEncrypt, true), true), true);
            strEncrypt = Md5Encrypt(strEncrypt);
            return strEncrypt;
        }

        public static string KEY_64 { get; set; }
        public static string IV_64 { get; set; }
        public static string Iv_128 { get; set; }
        public static string Key_128 { get; set; }
        static SymmetricAlgorithm AesProvider {
            get {
                SymmetricAlgorithm aesProvider = Rijndael.Create();
                Key_128 = "zicjin@gmail.com";
                Iv_128 = "l0i5z1h7e8n4y1u9";
                aesProvider.Key = Encoding.ASCII.GetBytes(Key_128);
                aesProvider.IV = Encoding.ASCII.GetBytes(Iv_128);
                return aesProvider;
            }
        }
        static DESCryptoServiceProvider DesProvider {
            get {
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                KEY_64 = "zicjin";
                IV_64 = "bugaolan";
                desProvider.Key = ASCIIEncoding.ASCII.GetBytes(KEY_64);
                desProvider.IV = ASCIIEncoding.ASCII.GetBytes(IV_64);
                return desProvider;
            }
        }

        public static string Md5Encrypt(string pToEncrypt) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.ASCII.GetBytes(pToEncrypt));
            md5.Clear();
            return BitConverter.ToString(bytes);
        }
        public static string Md5HashEncrypt(string pToEncrypt) {
            byte[] bytesParam = Encoding.Unicode.GetBytes(pToEncrypt); //Encoding.UTF8.GetBytes(pToEncrypt);
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            byte[] bytesMd5 = md5Provider.ComputeHash(bytesParam);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytesMd5) {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        public static string DesEncrypt(string pToEncrypt) {
            return DesEncrypt(pToEncrypt, true);
        }
        public static string DesEncrypt(string pToEncrypt, bool isHex) {
            string str = "";
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, DesProvider.CreateEncryptor(), CryptoStreamMode.Write)) {
                    if (isHex) {
                        byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        StringBuilder ret = new StringBuilder();
                        foreach (byte b in ms.ToArray()) {
                            //Format  as  hex  
                            ret.AppendFormat("{0:X2}", b);
                        }
                        str = ret.ToString();
                    } else {
                        using (StreamWriter sw = new StreamWriter(cs)) {
                            sw.Write(pToEncrypt);
                            sw.Flush();
                            cs.FlushFinalBlock();
                            str = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                        }
                    }
                }
            }
            return str;
        }

        public static string AesEncrypt(string pToEncrypt) {
            return AesEncrypt(pToEncrypt, false);
        }
        public static string AesEncrypt(string pToEncrypt, bool isHex) {
            string str = "";
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, AesProvider.CreateEncryptor(), CryptoStreamMode.Write)) {
                    if (isHex) {
                        byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                        StringBuilder ret = new StringBuilder();
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        foreach (byte b in ms.ToArray()) {
                            ret.AppendFormat("{0:X2}", b);
                        }
                        str = ret.ToString();
                    } else {
                        using (StreamWriter sw = new StreamWriter(cs)) {
                            sw.Write(pToEncrypt);
                            sw.Flush();
                            cs.FlushFinalBlock();


                            str = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }

            }
            return str;
        }

        public static string DesDecrypt(string pToEncrypt) {
            return DesDecrypt(pToEncrypt, false);
        }
        public static string DesDecrypt(string pToDecrypt, bool isHex) {
            string str = "";
            if (isHex) {
                using (MemoryStream ms = new MemoryStream()) {
                    using (CryptoStream cs = new CryptoStream(ms, DesProvider.CreateDecryptor(), CryptoStreamMode.Write)) {

                        byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                        for (int x = 0; x < pToDecrypt.Length / 2; x++) {
                            int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                            inputByteArray[x] = (byte)i;
                        }
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        str = System.Text.Encoding.Default.GetString(ms.ToArray());

                    }
                }
            } else {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(pToDecrypt))) {
                    using (CryptoStream cst = new CryptoStream(ms, DesProvider.CreateDecryptor(), CryptoStreamMode.Read)) {
                        using (StreamReader sr = new StreamReader(cst)) {
                            str = sr.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }

        public static string AesDecrypt(string pToEncrypt) {
            return AesDecrypt(pToEncrypt, false);
        }
        public static string AesDecrypt(string pToDecrypt, bool isHex) {
            string str = "";
            if (isHex) {
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++) {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }

                using (MemoryStream ms = new MemoryStream()) {
                    using (CryptoStream encStream = new CryptoStream(ms, AesProvider.CreateDecryptor(), CryptoStreamMode.Write)) {
                        encStream.Write(inputByteArray, 0, inputByteArray.Length);
                        encStream.FlushFinalBlock();
                        str = System.Text.Encoding.Default.GetString(ms.ToArray());
                    }
                }

            } else {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(pToDecrypt))) {
                    using (CryptoStream encStream = new CryptoStream(ms, AesProvider.CreateDecryptor(), CryptoStreamMode.Read)) {

                        using (StreamReader sr = new StreamReader(encStream)) {
                            str = sr.ReadToEnd();
                        }
                    }
                }
            }
            return str;
        }

    }
}
