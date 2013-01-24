using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace sharp_net {

    /// <summary>
    /// 對Excel文件的創建表、讀取、寫入數據操作.
    /// </summary>
    public static class ExcelHelp {

        /// <summary>
        /// 取文件的擴展名
        /// </summary>
        public static string GetExtFileTypeName(string FileName) {
            string sFile = FileName;// myFile.PostedFile.FileName;
            sFile = sFile.Substring(sFile.LastIndexOf("\\") + 1);
            sFile = sFile.Substring(sFile.LastIndexOf(".")).ToLower();
            return sFile;
        }

        /// <summary>
        /// 檢查一個文件是不是2007版本的Excel文件
        /// </summary>
        public static bool IsExcel2007(string FileName) {
            bool r;
            switch (GetExtFileTypeName(FileName)) {
                case ".xls":
                    r = false;
                    break;
                case ".xlsx":
                    r = true;
                    break;
                default:
                    throw new Exception("你要檢查" + FileName + "是2007版本的Excel文件還是之前版本的Excel文件，但是這個文件不是一個有效的Excel文件。");
            }
            return r;
        }

        /// <summary>
        /// Excel文件在服務器上的OLE連接字符串
        /// </summary>
        /// <param name="no_HDR">第一行不是標題：true;第一行是標題：false;</param>
        public static String GetExcelConnectionString(string excelFile, bool no_HDR) {
            string HDR = String.Empty;
            if (no_HDR) HDR = "HDR=NO; ";
            try {
                if (IsExcel2007(excelFile)) {
                    return "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0; " + HDR + "IMEX=1'"; //此连接可以操作.xls与.xlsx文件
                } else {
                    return "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + excelFile + ";Extended Properties='Excel 8.0; " + HDR + "IMEX=1'"; //此连接只能操作Excel2007之前(.xls)文件
                }
            } catch (Exception ee) {
                throw new Exception(ee.Message);
            }
        }

        /// <summary>
        /// Excel文件在服務器上的OLE可写連接字符串
        /// </summary>
        public static String GetExcelConnectionStringByWrite(string excelFile) {
            try {
                if (IsExcel2007(excelFile)) {
                    return "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0;'"; //此连接可以操作.xls与.xlsx文件
                } else {
                    return "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + excelFile + ";Extended Properties='Excel 8.0;'"; //此连接只能操作Excel2007之前(.xls)文件
                }
            } catch (Exception ee) {
                throw new Exception(ee.Message);
            }
        }

        /// <summary>
        /// 根据Excel物理路径获取Excel文件中所有表名
        /// </summary>
        public static List<string> GetExcelSheetNames(string excelFile) {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;
            try {
                string strConn = GetExcelConnectionString(excelFile, false);
                objConn = new OleDbConnection(strConn);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null) {
                    return null;
                }
            } catch (Exception ee) {
                throw new Exception(ee.Message);
            } finally {
                if (objConn != null) {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null) {
                    dt.Dispose();
                }
            }

            String[] excelSheets = new string[1];
            try {
                if (dt == null) {
                    return null;
                }
                excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows) {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
            } catch (Exception ee) {
                throw new Exception(ee.Message);
            } finally {
                if (dt != null) {
                    dt.Dispose();
                }
            }

            List<string> list = new List<string>();
            try {
                foreach (string s in excelSheets) {
                    string str = s;
                    if (str.LastIndexOf('$') > 0) {
                        str = str.Substring(0, str.Length - 1);
                    }
                    list.Add(str);
                }
                return list;
            } catch (Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// 獲取Excel文件中所有Sheet的內容到DataSet，以Sheet名做DataTable名
        /// </summary>
        /// <param name="FileFullPath">Excel物理路径</param>
        /// <param name="no_HDR">第一行不是標題：true;第一行是標題：false;</param>
        /// <returns>DataSet</returns>
        public static DataSet GetExcelToDataSet(string FileFullPath, bool no_HDR) {
            try {
                string strConn = GetExcelConnectionString(FileFullPath, no_HDR);
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataSet ds = new DataSet();
                foreach (string colName in GetExcelSheetNames(FileFullPath)) {
                    OleDbDataAdapter odda = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", colName), conn); //("select * from [Sheet1$]", conn);
                    odda.Fill(ds, colName);
                }
                conn.Close();
                return ds;
            } catch (Exception ee) {
                throw new Exception(ee.Message);
            }
        }

        /// <summary>
        /// 獲取Excel文件中指定Sheet的內容到DataTable
        /// </summary>
        public static DataTable GetExcelToDataSet(string FileFullPath, bool no_HDR, string SheetName) {
            try {
                string strConn = GetExcelConnectionString(FileFullPath, no_HDR);
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataSet ds = new DataSet();
                OleDbDataAdapter odda = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", SheetName), conn); //("select * from [Sheet1$]", conn);
                odda.Fill(ds, SheetName);
                conn.Close();
                return ds.Tables[SheetName];
            } catch (Exception ee) {
                throw new Exception(ee.Message);
            }
        }

        /// <summary>
        /// 刪除過時文件
        /// </summary>
        public static bool DeleteOldFile(string servepath) {
            try {
                FileInfo F = new FileInfo(servepath);
                F.Delete();
                return true;
            } catch (Exception ee) {
                throw new Exception(ee.Message + "刪除" + servepath + "出錯.");
            }
        }

        /// <summary>
        /// 在一個Excel文件中創建Sheet
        /// </summary>
        /// <param name="servepath">Excel物理路径,如果文件不是一個已存在的文件，會自動創建文件</param>
        /// <param name="cols">表頭列表</param>
        public static bool CreateSheet(string servepath, string sheetName, string[] cols) {
            try {
                if (sheetName.Trim() == "") {
                    throw new Exception("需要提供表名。");
                }
                if (cols.Equals(null)) {
                    throw new Exception("創建表需要提供字段列表。");
                }
                using (OleDbConnection conn = new OleDbConnection(GetExcelConnectionStringByWrite(servepath))) {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;
                    if (sheetName.LastIndexOf('$') > 0) {
                        sheetName = sheetName.Substring(sheetName.Length - 1);
                    }
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 3600;
                    StringBuilder sql = new StringBuilder();
                    sql.Append("CREATE TABLE [" + sheetName + "](");
                    foreach (string s in cols) {
                        sql.Append("[" + s + "] text,");
                    }
                    sql = sql.Remove(sql.Length - 1, 1);
                    sql.Append(")");
                    cmd.CommandText = sql.ToString();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            } catch (Exception ee) {
                throw ee;
            }
        }

        /// <summary>
        /// 把一個DataTable寫入到一個或多個Sheet中
        /// </summary>
        /// <param name="servepath">Excel物理路径,如果文件不是一個已存在的文件，會自動創建文件</param>
        /// <param name="sheetName">Sheet Name</param>
        /// <param name="maxrow">Sheet的行數</param>
        public static bool DataTable2Sheet(string servepath, DataTable dt, string sheetName, int maxrow) {
            try {
                if (sheetName.Trim() == "") {
                    throw new Exception("需要提供表名。");
                }
                StringBuilder strSQL = new StringBuilder();
                //看看目標表是否已存在
                List<string> tables = GetExcelSheetNames(servepath);
                if (tables.Contains(sheetName)) {
                    //存在,直接寫入
                    using (OleDbConnection conn = new OleDbConnection(GetExcelConnectionStringByWrite(servepath))) {
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.Connection = conn;
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            StringBuilder strfield = new StringBuilder();
                            StringBuilder strvalue = new StringBuilder();
                            for (int j = 0; j < dt.Columns.Count; j++) {
                                strfield.Append("[" + dt.Columns[j].ColumnName + "]");
                                strvalue.Append("'" + dt.Rows[i][j].ToString() + "'");
                                if (j != dt.Columns.Count - 1) {
                                    strfield.Append(",");
                                    strvalue.Append(",");
                                }
                            }
                            if (maxrow == 0)//不需要限制一個表的行數
                            {
                                cmd.CommandText = strSQL.Append(" insert into [" + sheetName + "]( ")
                                .Append(strfield.ToString()).Append(") values (").Append(strvalue).Append(")").ToString();
                            } else {
                                //加1才可才防止i=0的情況只寫入一行
                                string sheetNameT = sheetName + ((i + 1) / maxrow + (Math.IEEERemainder(i + 1, maxrow) == 0 ? 0 : 1)).ToString();
                                if (!tables.Contains(sheetNameT)) {
                                    tables = GetExcelSheetNames(servepath);
                                    string[] cols = new string[dt.Columns.Count];
                                    for (int ii = 0; ii < dt.Columns.Count; ii++) {
                                        cols[ii] = dt.Columns[ii].ColumnName;
                                    }
                                    if (!(CreateSheet(servepath, sheetNameT, cols))) {
                                        throw new Exception("在" + servepath + "上創建表" + sheetName + "失敗.");
                                    } else {
                                        tables = GetExcelSheetNames(servepath);
                                    }
                                }
                                cmd.CommandText = strSQL.Append(" insert into [" + sheetNameT + "]( ")
                                .Append(strfield.ToString()).Append(") values (").Append(strvalue).Append(")").ToString();
                            }
                            cmd.ExecuteNonQuery();
                            strSQL.Remove(0, strSQL.Length);
                        }

                        conn.Close();
                    }
                } else {
                    //不存在,需要先創建
                    using (OleDbConnection conn = new OleDbConnection(GetExcelConnectionStringByWrite(servepath))) {
                        conn.Open();
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.Connection = conn;
                        //創建表
                        string[] cols = new string[dt.Columns.Count];
                        for (int i = 0; i < dt.Columns.Count; i++) {
                            cols[i] = dt.Columns[i].ColumnName;
                        }

                        //產生寫數據的語句
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            StringBuilder strfield = new StringBuilder();
                            StringBuilder strvalue = new StringBuilder();
                            for (int j = 0; j < dt.Columns.Count; j++) {
                                strfield.Append("[" + dt.Columns[j].ColumnName + "]");
                                strvalue.Append("'" + dt.Rows[i][j].ToString() + "'");
                                if (j != dt.Columns.Count - 1) {
                                    strfield.Append(",");
                                    strvalue.Append(",");
                                }
                            }
                            if (maxrow == 0)//不需要限制一個表的行數
                            {
                                if (!tables.Contains(sheetName)) {
                                    if (!(CreateSheet(servepath, sheetName, cols))) {
                                        throw new Exception("在" + servepath + "上創建表" + sheetName + "失敗.");
                                    } else {
                                        tables = GetExcelSheetNames(servepath);
                                    }
                                }
                                cmd.CommandText = strSQL.Append(" insert into [" + sheetName + "]( ")
                                .Append(strfield.ToString()).Append(") values (").Append(strvalue).Append(")").ToString();
                            } else {
                                //加1才可才防止i=0的情況只寫入一行
                                string sheetNameT = sheetName + ((i + 1) / maxrow + (Math.IEEERemainder(i + 1, maxrow) == 0 ? 0 : 1)).ToString();

                                if (!tables.Contains(sheetNameT)) {
                                    for (int ii = 0; ii < dt.Columns.Count; ii++) {
                                        cols[ii] = dt.Columns[ii].ColumnName;
                                    }
                                    if (!(CreateSheet(servepath, sheetNameT, cols))) {
                                        throw new Exception("在" + servepath + "上創建表" + sheetName + "失敗.");
                                    } else {
                                        tables = GetExcelSheetNames(servepath);
                                    }
                                }
                                cmd.CommandText = strSQL.Append(" insert into [" + sheetNameT + "]( ")
                                .Append(strfield.ToString()).Append(") values (").Append(strvalue).Append(")").ToString();

                                //
                            }
                            cmd.ExecuteNonQuery();
                            strSQL.Remove(0, strSQL.Length);
                        }
                        conn.Close();
                    }
                }
                return true;
            } catch (Exception ee) {
                throw ee;
            }
        }

    }
}