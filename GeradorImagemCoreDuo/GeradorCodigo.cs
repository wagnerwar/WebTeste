using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BarcodeLib;
using QRCoder;
namespace GeradorImagemCoreDuo
{
    public class GeradorCodigo
    {
        public byte[] GerarCodigoBarras(String texto)
        {
            byte[] byteArray = null;
            try
            {
                Barcode b = new Barcode();
                Image img = b.Encode(TYPE.CODE39, texto, Color.Black, Color.White, 250, 100);
                using(MemoryStream m = new MemoryStream())
                {
                    img.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = m.ToArray();
                }
                return byteArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public byte[] GerarCodigoQR(String texto)
        {
            byte[] byteArray = null;
            try
            {
                QRCodeGenerator generator = new QRCodeGenerator();
                QRCodeData codeData = generator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(codeData);
                Bitmap bitmap = qrCode.GetGraphic(15);
                using (MemoryStream m = new MemoryStream())
                {
                    bitmap.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                    byteArray = m.ToArray();
                }
                return byteArray;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }
    }
}
