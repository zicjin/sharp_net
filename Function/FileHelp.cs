using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace yMatouFlow.Web.Commen {
    /// <summary>
    /// 打包压缩文件
    /// </summary>
    public class FileHelp {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="dirPath">压缩目录（只压缩PDF文件）</param>
        /// <param name="zipFilePath">目标目录（如果为空，则以压缩目录为准）</param>
        /// <param name="error">异常信息</param>
        /// <returns></returns>
        public static bool ZipFile(string dirPath, string zipFilePath, out string error) {
            error = "";
            if (string.IsNullOrEmpty(dirPath) || !Directory.Exists(dirPath)) {
                error = "dirPath Error";
                return false;
            }

            if (string.IsNullOrEmpty(zipFilePath)) {
                if (dirPath.EndsWith("\\")) {
                    dirPath = dirPath.Substring(0, dirPath.Length - 1);
                }
                zipFilePath = dirPath + ".zip";
            }

            try {
                string[] fileNames = Directory.GetFiles(dirPath);
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath))) {
                    s.SetLevel(9);
                    byte[] buffer = new byte[4096];
                    foreach (string file in fileNames) {
                        if (Path.GetExtension(file).ToLower().Equals(".pdf")) {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                            entry.DateTime = DateTime.Now;
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file)) {
                                int sourceBytes;
                                do {
                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                    s.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);
                            }
                        }
                    }
                    s.Flush();
                    s.Close();
                }
            } catch (Exception ex) {
                error = ex.Message.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">目标目录</param>
        /// <param name="fileExt">要删除的文件类型</param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool DeleteFiles(string path, string fileExt, out string err) {
            err = "";
            DirectoryInfo fold = new DirectoryInfo(path);
            if (fold.Exists) {
                FileInfo[] files = fold.GetFiles("*." + fileExt);
                foreach (FileInfo f in files) {
                    f.Delete();
                }
                return true;
            } else {
                err = "Path not Exists";
                return false;
            }
        }
    }
}