using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf.qrcode;
using Host.DataModel;

namespace Host.Helper
{
    public class DownloadPdf
    {
        private static string strtemp;
        private static string path;
        Document _document;
        PdfWriter _pdfWriter;
        Phrase _phrase;
        


        public static byte[] Download(int stationLocationId,string stationName,string locationName, string code)
        {
            try
            {
               
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                
                var document = new iTextSharp.text.Document(new Rectangle(288f, 432f));

      

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                    Phrase phrase = new Phrase();
                    document.Add(phrase);


                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"D:\Eco\Eco\src\Host\wwwroot\images\Eco.jpg");
                    image.ScaleAbsolute(75f, 75f);
                    image.SetAbsolutePosition(30f, 345f);
                    image.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    document.Add(image);
                    

                string Phone = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone = new iTextSharp.text.Paragraph();

                    phone.SpacingBefore = 1;
                    phone.SpacingAfter = 1;
                    phone.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    phone.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    phone.Add(Phone);
                    document.Add(phone);

                    string Email = @"info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email = new iTextSharp.text.Paragraph();

                    email.SpacingBefore = 1;
                    email.SpacingAfter = 1;
                    email.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    email.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    email.Add(Email);
                    document.Add(email);

                    string Web = @"www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web = new iTextSharp.text.Paragraph();

                    web.SpacingBefore = 1;
                    web.SpacingAfter = 60;
                    web.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    web.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    web.Add(Web);
                    document.Add(web);

                BarcodeQRCode barcodeQRCode = new BarcodeQRCode(code, 1000, 1000, null);
                Image codeQrImage = barcodeQRCode.GetImage();
                codeQrImage.ScaleAbsolute(75, 75);
                codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                document.Add(codeQrImage);

                PdfContentByte cb = writer.DirectContent;
                                 var Rectangular = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                                  Rectangular.BorderWidthLeft = 1.1f;
                                  Rectangular.BorderWidthRight = 1.1f;
                                  Rectangular.BorderWidthTop = 1.1f;
                                  Rectangular.BorderWidthBottom = 1.1f;
       
                                  cb.Rectangle(Rectangular);
                                  cb.Stroke();


                                  cb.SetLineWidth(1);
                                  cb.Rectangle(15, 230, 140, 50); //left width,top height,right width,bottom height
                                  cb.BeginText();
                                  BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                  cb.SetFontAndSize(f_cn, 10);
                                  cb.SetTextMatrix(28, 250);
                                  cb.ShowText("StationID:  " + stationLocationId);
                                  cb.EndText();

                                  cb.Rectangle(15, 160, 140, 50);
                                  cb.SetLineWidth(3);
                                  cb.Stroke();
                                  cb.BeginText();
                                  BaseFont f_cnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                  cb.SetFontAndSize(f_cnn, 10);
                                  cb.SetTextMatrix(22, 180);
                                  cb.ShowText("Station Name:  " + stationName);
                                  cb.EndText();


                                  cb.Rectangle(15, 90, 140, 50);
                                  cb.SetLineWidth(3);
                                  cb.Stroke();
                                  cb.BeginText();
                                  BaseFont f_cnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                  cb.SetFontAndSize(f_cnnn, 10);
                                  cb.SetTextMatrix(19, 110);
                                  cb.ShowText("Location Name: " + locationName);
                                  cb.EndText();

                                    

                              // Page 2

                              document.NewPage();


                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(@"D:\Eco\Eco\src\Host\wwwroot\images\Eco.jpg");
                image1.ScaleAbsolute(75f, 75f);
                image1.SetAbsolutePosition(30f, 345f);
                image1.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                document.Add(image);

                string Phone1 = @"Phone:021-34829161/63";

                List<iTextSharp.text.Paragraph> paragraph1 = new List<iTextSharp.text.Paragraph>();


                iTextSharp.text.Paragraph phone1 = new iTextSharp.text.Paragraph();

                phone1.SpacingBefore = 1;
                phone1.SpacingAfter = 1;
                phone1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                phone1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                phone1.Add(Phone1);
                document.Add(phone1);

                string Email1 = @"info@ecoservices.com.pk";
                iTextSharp.text.Paragraph email1 = new iTextSharp.text.Paragraph();

                email1.SpacingBefore = 1;
                email1.SpacingAfter = 1;
                email1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                email1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                email1.Add(Email1);
                document.Add(email1);

                string Web1 = @"www.ecoservices.com.pk";
                iTextSharp.text.Paragraph web1 = new iTextSharp.text.Paragraph();

                web1.SpacingBefore = 1;
                web1.SpacingAfter = 60;
                web1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                web1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                web1.Add(Web1);
                document.Add(web1);

               

                PdfContentByte cb1 = writer.DirectContent;
                var Rectangular1 = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                Rectangular.BorderWidthLeft = 1.1f;
                Rectangular.BorderWidthRight = 1.1f;
                Rectangular.BorderWidthTop = 1.1f;
                Rectangular.BorderWidthBottom = 1.1f;

                cb.Rectangle(Rectangular);
                cb.Stroke();


                cb.SetLineWidth(1);
                cb.Rectangle(45, 230, 190, 50); //left width,top height,right width,bottom height
                cb.BeginText();
                BaseFont f_cn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(f_cn, 10);
                cb.SetTextMatrix(95, 250);
                cb.ShowText("StationID:  " + stationLocationId);
                cb.EndText();

                cb.Rectangle(45, 160, 190, 50);
                cb.SetLineWidth(3);
                cb.Stroke();
                cb.BeginText();
                BaseFont f_cnn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(f_cnn, 10);
                cb.SetTextMatrix(85, 180);
                cb.ShowText("Station Name:  " + stationName);
                cb.EndText();


                cb.Rectangle(45, 90, 190, 50);
                cb.SetLineWidth(3);
                cb.Stroke();
                cb.BeginText();
                BaseFont f_cnnn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.SetFontAndSize(f_cnnn, 10);
                cb.SetTextMatrix(85, 110);
                cb.ShowText("Location Name: " + locationName);
                cb.EndText();


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

        public static byte[] DownloadAllPdf (List<DownloadPdfDto> downloadAllPdf)
        {
            try
            {
                var document = new iTextSharp.text.Document(new Rectangle(288f, 432f));
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                byte[] bytes;
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                foreach (var s in downloadAllPdf)
                {
                   
                    document.Open();
                    Phrase phrase = new Phrase();
                    document.Add(phrase);


                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(@"D:\Eco\Eco\src\Host\wwwroot\images\Eco.jpg");
                    image.ScaleAbsolute(75f, 75f);
                    image.SetAbsolutePosition(30f, 345f);
                    image.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    document.Add(image);


                    string Phone = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone = new iTextSharp.text.Paragraph();

                    phone.SpacingBefore = 1;
                    phone.SpacingAfter = 1;
                    phone.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    phone.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    phone.Add(Phone);
                    document.Add(phone);

                    string Email = @"info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email = new iTextSharp.text.Paragraph();

                    email.SpacingBefore = 1;
                    email.SpacingAfter = 1;
                    email.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    email.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    email.Add(Email);
                    document.Add(email);

                    string Web = @"www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web = new iTextSharp.text.Paragraph();

                    web.SpacingBefore = 1;
                    web.SpacingAfter = 60;
                    web.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    web.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    web.Add(Web);
                    document.Add(web);

                    BarcodeQRCode barcodeQRCode = new BarcodeQRCode(s.Code, 1000, 1000, null);
                    Image codeQrImage = barcodeQRCode.GetImage();
                    codeQrImage.ScaleAbsolute(75, 75);
                    codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    document.Add(codeQrImage);

                    PdfContentByte cb = writer.DirectContent;
                    var Rectangular = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                    Rectangular.BorderWidthLeft = 1.1f;
                    Rectangular.BorderWidthRight = 1.1f;
                    Rectangular.BorderWidthTop = 1.1f;
                    Rectangular.BorderWidthBottom = 1.1f;

                    cb.Rectangle(Rectangular);
                    cb.Stroke();


                    cb.SetLineWidth(1);
                    cb.Rectangle(15, 230, 140, 50); //left width,top height,right width,bottom height
                    cb.BeginText();
                    BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cn, 10);
                    cb.SetTextMatrix(28, 250);
                    cb.ShowText("StationID:  " + s.StationLocationId);
                    cb.EndText();

                    cb.Rectangle(15, 160, 140, 50);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnn, 10);
                    cb.SetTextMatrix(22, 180);
                    cb.ShowText("Station Name:  " + s.StationName);
                    cb.EndText();


                    cb.Rectangle(15, 90, 140, 50);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnnn, 10);
                    cb.SetTextMatrix(19, 110);
                    cb.ShowText("Location Name: " + s.LocationName);
                    cb.EndText();



                    // Page 2

                    document.NewPage();


                    iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(@"D:\Eco\Eco\src\Host\wwwroot\images\Eco.jpg");
                    image1.ScaleAbsolute(75f, 75f);
                    image1.SetAbsolutePosition(30f, 345f);
                    image1.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    document.Add(image);

                    string Phone1 = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph1 = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone1 = new iTextSharp.text.Paragraph();

                    phone1.SpacingBefore = 1;
                    phone1.SpacingAfter = 1;
                    phone1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    phone1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    phone1.Add(Phone1);
                    document.Add(phone1);

                    string Email1 = @"info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email1 = new iTextSharp.text.Paragraph();

                    email1.SpacingBefore = 1;
                    email1.SpacingAfter = 1;
                    email1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    email1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    email1.Add(Email1);
                    document.Add(email1);

                    string Web1 = @"www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web1 = new iTextSharp.text.Paragraph();

                    web1.SpacingBefore = 1;
                    web1.SpacingAfter = 60;
                    web1.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    web1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                    web1.Add(Web1);
                    document.Add(web1);



                    PdfContentByte cb1 = writer.DirectContent;
                    var Rectangular1 = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                    Rectangular.BorderWidthLeft = 1.1f;
                    Rectangular.BorderWidthRight = 1.1f;
                    Rectangular.BorderWidthTop = 1.1f;
                    Rectangular.BorderWidthBottom = 1.1f;

                    cb.Rectangle(Rectangular);
                    cb.Stroke();


                    cb.SetLineWidth(1);
                    cb.Rectangle(45, 230, 190, 50); //left width,top height,right width,bottom height
                    cb.BeginText();
                    BaseFont f_cn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cn, 10);
                    cb.SetTextMatrix(95, 250);
                    cb.ShowText("StationID:  " + s.StationLocationId);
                    cb.EndText();

                    cb.Rectangle(45, 160, 190, 50);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnn, 10);
                    cb.SetTextMatrix(85, 180);
                    cb.ShowText("Station Name:  " + s.StationName);
                    cb.EndText();


                    cb.Rectangle(45, 90, 190, 50);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnnn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnnn, 10);
                    cb.SetTextMatrix(85, 110);
                    cb.ShowText("Location Name: " + s.LocationName);
                    cb.EndText();
                    document.NewPage();

                }
                document.Close();
                bytes = memoryStream.ToArray();
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
