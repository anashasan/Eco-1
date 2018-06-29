using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Helper
{
    public class DownloadPdf
    {
        private static string strtemp;
        private static string path;
        Document _document;
         PdfWriter _pdfWriter;
         Phrase _phrase;


        public static byte[] Download(int stationId,string stationName,string locationName)
        {
            try
            {
               
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();


                    Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                
              //  document.Add(new Paragraph("Title2", titleFont));


                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    Phrase phrase = new Phrase();
                    document.Add(phrase);

                // Chunk chunk = new Chunk(id.ToString());

                //document.Add(chunk);
                //  Image.ImageUrl = (Server.MapPath("Image/" + strtemp));
              //  string[] lines = System.IO.File.ReadAllLines(@"C:\Users\home\source\repos\Eco-1\src\Host\wwwroot\images\Eco.jpeg'");
                // var imageName = "Eco.jpeg";
 
                   //using (FileStream fileStream = File.OpenWrite
                   //   (Path.Combine(directory, imageName)))
                /*iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(f)*/;
                //document.Add(image);


                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"D:\Eco\Eco\src\Host\wwwroot\images\Eco.jpg");
                image.ScaleAbsolute(149f, 110f);
                image.SetAbsolutePosition(50f, 725f);
                image.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                document.Add(image);


                string Phone = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone = new iTextSharp.text.Paragraph();

                    phone.SpacingBefore = 10;
                    phone.SpacingAfter = 10;
                    phone.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    phone.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f, BaseColor.BLACK);
                    phone.Add(Phone);
                    document.Add(phone);

                    string Email = @"info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email = new iTextSharp.text.Paragraph();

                    email.SpacingBefore = 10;
                    email.SpacingAfter = 10;
                    email.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    email.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f, BaseColor.BLACK);
                    email.Add(Email);
                    document.Add(email);

                    string Web = @"www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web = new iTextSharp.text.Paragraph();

                    web.SpacingBefore = 10;
                    web.SpacingAfter = 10;
                    web.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    web.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f, BaseColor.BLACK);
                    web.Add(Web);
                    document.Add(web);

                

                PdfContentByte cb = writer.DirectContent;
                    var Rectangular = new Rectangle(55, 715, 540, 175);
                    Rectangular.BorderWidthLeft = 2.1f;
                    Rectangular.BorderWidthRight = 3.1f;
                    Rectangular.BorderWidthTop = 4.1f;
                    Rectangular.BorderWidthBottom = 5.1f;
                    cb.Rectangle(Rectangular);
                    cb.Stroke();


                    cb.SetLineWidth(3);
                    cb.Rectangle(100, 600, 350, 100);
                    cb.BeginText();
                    BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cn, 20);
                    cb.SetTextMatrix(195, 655);
                    cb.ShowText("STATIONID:  " + stationId);
                    cb.EndText();
                    cb.Rectangle(100, 450, 350, 100);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnn, 20);
                    cb.SetTextMatrix(160, 500);
                    cb.ShowText("STATION NAME:  " + stationName);
                    cb.EndText();


                    cb.Rectangle(100, 300, 350, 100);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnnn, 20);
                    cb.SetTextMatrix(150, 350);
                    cb.ShowText("LOCATION NAME: " + locationName);
                    cb.EndText();



                // Page 2

                document.NewPage();



                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(@"D:\Eco\Eco\src\Host\wwwroot\images\Eco.jpg");
                image1.ScaleAbsolute(149f, 110f);
                image1.SetAbsolutePosition(50f, 725f);
                image1.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                document.Add(image1);
                
                string Phone1 = @"Phone:021-34829161/63";

                List<iTextSharp.text.Paragraph> paragraph1 = new List<iTextSharp.text.Paragraph>();


                iTextSharp.text.Paragraph phone1 = new iTextSharp.text.Paragraph();

                phone1.SpacingBefore = 10;
                phone1.SpacingAfter = 10;
                phone1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                phone1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f, BaseColor.BLACK);
                phone1.Add(Phone1);
                document.Add(phone1);

                string Email1 = @"info@ecoservices.com.pk";
                iTextSharp.text.Paragraph email1 = new iTextSharp.text.Paragraph();

                email1.SpacingBefore = 10;
                email1.SpacingAfter = 10;
                email1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                email1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f, BaseColor.BLACK);
                email1.Add(Email1);
                document.Add(email1);

                string Web1 = @"www.ecoservices.com.pk";
                iTextSharp.text.Paragraph web1 = new iTextSharp.text.Paragraph();

                web1.SpacingBefore = 10;
                web1.SpacingAfter = 10;
                web1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                web1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 16f, BaseColor.BLACK);
                web1.Add(Web1);
                document.Add(web1);

                PdfContentByte cb1 = writer.DirectContent;
                var Rectangular1 = new Rectangle(25, 715, 580, 175);
                Rectangular1.BorderWidthLeft = 2.1f;
                Rectangular1.BorderWidthRight = 3.1f;
                Rectangular1.BorderWidthTop = 4.1f;
                Rectangular1.BorderWidthBottom = 5.1f;
                cb1.Rectangle(Rectangular1);
                cb1.Stroke();


                cb1.SetLineWidth(3);
                cb1.Rectangle(50, 600, 220, 80); //left width,top height,right width,bottom height
                cb1.BeginText();
                BaseFont f_cnNN = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb1.SetFontAndSize(f_cn, 14);
                cb1.SetTextMatrix(90, 630);
                cb1.ShowText("STATIONID:  " + stationId);
                cb1.EndText();

                cb1.Rectangle(50, 450, 220, 80); //left width,top height,right width,bottom height
                cb1.SetLineWidth(3);
                cb1.Stroke();
                cb1.BeginText();
                BaseFont f_cnnN = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb1.SetFontAndSize(f_cnn, 14);
                cb1.SetTextMatrix(70, 480);
                cb1.ShowText("STATION NAME:  " + stationName);
                cb1.EndText();


                cb1.Rectangle(50, 300, 220, 80);
                cb1.SetLineWidth(3);
                cb1.Stroke();
                cb1.BeginText();
                BaseFont f_cnnnN = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb1.SetFontAndSize(f_cnnn, 14);
                cb1.SetTextMatrix(60, 335);
                cb1.ShowText("LOCATION NAME: " + locationName);
                cb1.EndText();



                document.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    return bytes;

                
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

    }
}
