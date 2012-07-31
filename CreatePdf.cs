using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace yMatouFlow.Unite {
    /// <summary>
    /// 生成PDF
    /// </summary>
    public interface iFlowPDF {
        bool AddContent(string filePath, 
            string date, 
            string originalNum, 
            string bhNum, 
            string weightLb, 
            string weightKg, 
            string senderName, 
            string senderAddress, 
            string senderPhone, 
            string receiverName, 
            string receiveAddress, 
            string receiverZip, 
            string receiverPhone, 
            string goodsName, 
            string goodsPrice,
            ref string referror);
    }

    public class YaoFlowPDF : iFlowPDF {
        public bool AddContent(string filePath, string date, string originalNum, string bhNum, string weightLb, string weightKg, string senderName, string senderAddress, string senderPhone, string receiverName, string receiveAddress, string receiverZip, string receiverPhone, string goodsName, string goodsPrice, ref string referror) {
            Document document = new Document();
            try {
                string fileName = null;
                DirectoryInfo dr = new DirectoryInfo(filePath);
                if (!dr.Exists) {
                    dr.Create();
                }
                fileName = filePath + "\\" + bhNum + ".pdf";
                string applicationPath = System.Web.HttpContext.Current.Server.MapPath("\\");
                PdfReader pdfReader = new PdfReader(applicationPath + "\\Assist\\pdfData\\pdftemplate.pdf");
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(fileName, FileMode.Create));
                //四种字体
                BaseFont chFont = BaseFont.CreateFont("c:\\windows\\fonts\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                BaseFont barCodeFont = BaseFont.CreateFont(applicationPath + "\\Assist\\pdfData\\HC39M.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                BaseFont blackFont = BaseFont.CreateFont(applicationPath + "\\Assist\\pdfData\\calibrib.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                PdfContentByte over = pdfStamper.GetOverContent(1);

                over.BeginText();

                over.SetFontAndSize(blackFont, 10);  //英文黑粗
                //日期
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, date, 90, 719, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, date, 90, 362, 0);
                //编号
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, bhNum, 298, 734, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, bhNum, 298, 378, 0);
                //发货人姓名
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderName, 90, 691, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderName, 90, 334, 0);
                //发货人地址
                if (senderAddress.Length > 40) {
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderAddress.Substring(0, 40), 88, 676, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderAddress.Substring(40), 88, 662, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderAddress.Substring(0, 40), 88, 320, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderAddress.Substring(40), 88, 306, 0);
                }
                else {
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderAddress, 88, 676, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderAddress, 88, 320, 0);
                }
                //收货人电话
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiverPhone, 458, 691, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiverPhone, 458, 334, 0);
                //收货人邮编
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiverZip, 466, 634, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiverZip, 466, 277, 0);
                //价格
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, goodsPrice, 109, 531, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, goodsPrice, 109, 174, 0);
                //发货人电话
                over.SetFontAndSize(blackFont, 8);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderPhone, 220, 691, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, senderPhone, 220, 334, 0);
                //货物重量
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, weightKg, 87, 588, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, weightKg, 87, 233, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, weightLb, 87, 579, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, weightLb, 87, 223, 0);
                //条形码
                over.SetFontAndSize(barCodeFont, 12);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "*" + bhNum + "*", 390, 739, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "*" + bhNum + "*", 390, 384, 0);
                //备注
                over.SetFontAndSize(chFont, 8);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, originalNum, 476, 439, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, originalNum, 476, 82, 0);
                //收件人姓名
                over.SetFontAndSize(chFont, 11);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiverName, 343, 691, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiverName, 343, 335, 0);
                //收件人地址
                int chNum = 40;
                if (IsChineseLetter(receiveAddress, 0) && IsChineseLetter(receiveAddress, 1)) {
                    chNum = 20;
                }
                if (receiveAddress.Length > chNum) {
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiveAddress.Substring(0, chNum), 344, 676, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiveAddress.Substring(chNum), 344, 663, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiveAddress.Substring(0, chNum), 344, 320, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiveAddress.Substring(chNum), 344, 306, 0);
                }
                else {
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiveAddress, 344, 676, 0);
                    over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, receiveAddress, 344, 320, 0);
                }
                //名称
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, goodsName, 43, 600, 0);
                over.ShowTextAligned(PdfContentByte.ALIGN_LEFT, goodsName, 43, 243, 0);

                over.EndText();
                pdfStamper.Close();
                return true;
            }
            catch (Exception ex) {
                referror += ex.Message.ToString();
                return false;
            }
            finally {
                document.Close();
            }
        }

        private bool IsChineseLetter(string input, int index) {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "") {
                code = Char.ConvertToUtf32(input, index);
                if (code >= chfrom && code <= chend) {
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        }
    }
}