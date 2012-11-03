using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace sharp_net {
    public class ImageUnit {
        public static byte[] ResizeImageFile(byte[] imageFile, int targetSize) {
            using (Image oldImage = Image.FromStream(new MemoryStream(imageFile))) {
                Size newSize = CalculateDimensions(oldImage.Size, targetSize);
                using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb)) {
                    using (Graphics canvas = Graphics.FromImage(newImage)) {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                        MemoryStream m = new MemoryStream();
                        newImage.Save(m, ImageFormat.Jpeg);
                        return m.GetBuffer();
                    }
                }
            }
        }

        private static Size CalculateDimensions(Size oldSize, int targetSize) {
            Size newSize = new Size();
            if (oldSize.Height > oldSize.Width) {
                newSize.Width = (int)(oldSize.Width * ((float)targetSize / (float)oldSize.Height));
                newSize.Height = targetSize;
            } else {
                newSize.Width = targetSize;
                newSize.Height = (int)(oldSize.Height * ((float)targetSize / (float)oldSize.Width));
            }
            return newSize;
        }
    }
}
