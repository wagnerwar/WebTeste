using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.QrCode;
using System.Drawing;
using System.IO;

namespace GeradorImagem
{
    public class Graphic
    {
        public byte[] gerarImagem( ZXing.Rendering.PixelData pixelData)
        {
            byte[] byteArray = null;
            try
            {
                // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
                // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
                using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    using (var ms = new MemoryStream())
                    {
                        var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        try
                        {
                            // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }
                        // save to stream as PNG
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byteArray = ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return byteArray;
        }
    }
}
