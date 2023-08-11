using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TesteWeb.Libs
{
    public class GeradorGrafico
    {
        
        
        public Bitmap AgruparImagensTresColunas(
            List<Image> imagens, 
            int larguraImagem, 
            int alturaImagem, 
            int limiteCelulasLinha, 
            int limiteDivisorLinha)
        {
            try
            {
                List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
                System.Drawing.Bitmap finalImage = null;
                try
                {
                    int width = 0;
                    int height = 0;
                    int offset = 5;

                    int px = 0;
                    int py = 0;
                    int cx = larguraImagem;
                    int cy = alturaImagem;

                    List<Point> posicoes = new List<Point>();
                    int contador = 0;
                    foreach (Image image in imagens)
                    {
                        System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);
                        if(contador <= limiteCelulasLinha)
                        {
                            width += (bitmap.Width + offset);
                            height = bitmap.Height + offset;
                        }
                        else if(contador % limiteDivisorLinha == 0)
                        {
                            height += ( bitmap.Height + offset );
                        }
                        images.Add(bitmap);
                        contador++;
                    }

                    finalImage = new System.Drawing.Bitmap(width, height);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                    {
                        g.Clear(System.Drawing.Color.White);
                        

                        // Distribuir pontos com base na quantidade de imagens -- ???
                        List<int> pontosX = new List<int>();
                        List<int> pontosY = new List<int>();

                        // Tabela
                        if(limiteCelulasLinha > 0 && limiteDivisorLinha > 0)
                        {
                            for (var y = 0; y <= limiteCelulasLinha; y++)
                            {
                                if (y == 0)
                                {
                                    pontosX.Add(px);
                                }
                                else
                                {
                                    pontosX.Add(px + (cx + offset) * (y));
                                }
                            }
                            for (var y = 0; y < limiteDivisorLinha; y++)
                            {
                                if (y == 0)
                                {
                                    pontosY.Add(py);
                                }
                                else
                                {
                                    pontosY.Add(py + (cy) * (y));
                                }
                            }
                        }else if(limiteCelulasLinha == 0 && limiteDivisorLinha == 1)
                        {
                            // Bloco
                            for (var y = 0; y <= limiteCelulasLinha; y++)
                            {
                                if (y == 0)
                                {
                                    pontosX.Add(px);
                                }
                                else
                                {
                                    pontosX.Add(px + (cx + offset) * (y));
                                }
                            }
                            for (var y = 0; y < imagens.Count(); y++)
                            {
                                if (y == 0)
                                {
                                    pontosY.Add(py);
                                }
                                else
                                {
                                    pontosY.Add(py + (cy) * (y));
                                }
                            }
                        }                        
                                               
                        List<Point> posicoesCalculados = new List<Point>();
                        foreach(var xx in pontosX)
                        {
                            foreach(var yy in pontosY)
                            {
                                posicoesCalculados.Add(new Point(xx, yy));
                            }
                        }

                        int x = 0;
                        foreach (Bitmap b in images)
                        {
                            g.DrawImage(b, posicoesCalculados[x]);
                            x++;
                        }
                    }
                    return finalImage;
                }
                catch (Exception ex)
                {
                    if (finalImage != null)
                        finalImage.Dispose();

                    throw ex;
                }
                finally
                {

                    foreach (System.Drawing.Bitmap image in images)
                    {
                        image.Dispose();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public string GerarImagemTotalizadorGrid(
            string total,
            int largura,
            int altura,
            IDictionary<string, object> PropriedadesAdicionais = null)
        {
            byte[] bytes = null;
            string base64String = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            var image = RecuperarImagemTotalizadorGrid(total, largura, altura);
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.ToArray();
            base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }
        public string RedimensionarImagemTotalizadorGrid(
            string total,
            int largura,
            int altura,
            IDictionary<string, object> PropriedadesAdicionais = null)
        {
            byte[] bytes = null;
            string base64String = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            var image = RecuperarImagemTotalizadorGrid(total, largura, altura);
            var redimensionado = ResizeImage(image, 40, 40);
            redimensionado.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.ToArray();
            base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }
        public string AgruparImagemTotalizadorGrid(
            string total,
            int largura,
            int altura,
            IDictionary<string, object> PropriedadesAdicionais = null)
        {
            byte[] bytes = null;
            string base64String = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            List<Image> imagens = new List<Image>();
            var image = RecuperarImagemTotalizadorGrid(total, largura, altura);
            Bitmap agrupados = null;
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            agrupados = AgruparImagensTresColunas(imagens, largura, altura, 2, 3);

            agrupados.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.ToArray();
            base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }
        public string AgruparImagemTotalizadorBloco(
            string total,
            int largura,
            int altura,
            IDictionary<string, object> PropriedadesAdicionais = null)
        {
            byte[] bytes = null;
            string base64String = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            List<Image> imagens = new List<Image>();
            var image = RecuperarImagemTotalizadorGrid(total, largura, altura);
            Bitmap agrupados = null;
            imagens.Add(image);
            imagens.Add(image);
            imagens.Add(image);
            agrupados = AgruparImagensTresColunas(imagens, largura, altura, 0, 1);
            agrupados.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bytes = ms.ToArray();
            base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }
        public Image RecuperarImagemTotalizadorGrid(
            string total,
            int largura,
            int altura)
        {
            // Tamanhos esperados
            int largura_grande = 1085;
            int largura_medio = 507;
            int largura_pequeno = 328;
            int altura_grande = 555;
            int altura_media = 250;
            int limite_caracteres = 9;
            byte[] bytes = null;
            string base64String = null;
            Image image = new Bitmap(largura, altura);
            Graphics graph = Graphics.FromImage(image);
            graph.Clear(Color.White);

            int tamanhoFonte = 30;
            //int fator = 45;
            int fator = 15;
            int dimensaoX = 200;
            int dimensaoY = 100;

            var centroX = (largura / 2);
            var centroY = (altura / 2);
            var pontoX = centroX - (dimensaoX / 2);
            var pontoY = centroY - (dimensaoY / 2);
            Pen pen = new Pen(Brushes.Black);
            Brush corTexto = null;
            Brush corTotalizador = null;
            // Tratando limite de caracteres para expandir a largura 
            if (total.Length > limite_caracteres)
            {
                int diferenca = total.Length - limite_caracteres;
                int ajuste = (fator * diferenca);
                dimensaoX += ajuste;
                pontoX -= (ajuste / 2);
            }


            // Propriedades adicionais
            corTotalizador = new SolidBrush(Color.FromArgb(0, 255, 255));
            corTexto = new SolidBrush(Color.FromArgb(0, 0, 0));

            // Construct a new Rectangle .
            Rectangle displayRectangle = new Rectangle(new Point(pontoX, pontoY), new Size(dimensaoX, dimensaoY));
            StringFormat format1 = new StringFormat(StringFormatFlags.LineLimit);
            format1.LineAlignment = StringAlignment.Center;
            format1.Alignment = StringAlignment.Center;
            graph.FillRectangle(corTotalizador != null ? corTotalizador : Brushes.Yellow, displayRectangle);
            graph.DrawString(
                total, 
                new Font(new FontFamily("Arial"), tamanhoFonte, FontStyle.Bold),
                (corTexto != null ? corTexto : Brushes.Black), (RectangleF)displayRectangle, format1);
            return image;
        }
    }
}