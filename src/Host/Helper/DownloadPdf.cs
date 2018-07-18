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
      
        public static  byte[] Download( int stationLocationId,string stationName,string locationName, string code,int Sno, IHostingEnvironment _hostingEnvironment)
        {
            try
            {
               
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                
                var document = new iTextSharp.text.Document(new Rectangle(288f, 432f));

      

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();
                    Phrase phrase = new Phrase();
                    document.Add(phrase);

                var path = Path.Combine(_hostingEnvironment.WebRootPath, @"images\Eco.jpg");

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(path);
                    image.ScaleAbsolute(145f, 70f);
                    image.SetAbsolutePosition(70f, 345f);
                    image.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                //image.SpacingAfter = 40;
                    document.Add(image);
                    

                string Phone = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone = new iTextSharp.text.Paragraph();

                    phone.SpacingBefore = 50;
                    phone.SpacingAfter = 1;
                    phone.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    phone.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                    phone.Add(Phone);
                    document.Add(phone);

                    string Email = @"Email:info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email = new iTextSharp.text.Paragraph();

                    email.SpacingBefore = 1;
                    email.SpacingAfter = 1;
                    email.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    email.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                    email.Add(Email);
                    document.Add(email);

                    string Web = @"Web: www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web = new iTextSharp.text.Paragraph();

                    web.SpacingBefore = 1;
                    web.SpacingAfter = 30;
                    web.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    web.Font = FontFactory.GetFont(FontFactory.TIMES, 16F, BaseColor.DARK_GRAY);
                    web.Add(Web);
                    document.Add(web);

               

              /*  PdfContentByte cb = writer.DirectContent;
                                 var Rectangular = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                                  Rectangular.BorderWidthLeft = 1.1f;
                                  Rectangular.BorderWidthRight = 1.1f;
                                  Rectangular.BorderWidthTop = 1.1f;
                                  Rectangular.BorderWidthBottom = 1.1f;*/

                string Station = stationName;
                iTextSharp.text.Paragraph station = new iTextSharp.text.Paragraph();

                station.SpacingBefore = 10;
                station.SpacingAfter = 25;
                station.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                station.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                station.Add(Station);
                document.Add(station);

                int StationNo = Sno;
                iTextSharp.text.Paragraph stationno = new iTextSharp.text.Paragraph();

                stationno.SpacingBefore = 10;
                stationno.SpacingAfter = 10;
                stationno.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                stationno.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 60f, BaseColor.BLACK);
                stationno.Add(""+Sno);
                document.Add(stationno);

                

                string LocationName = locationName;
                iTextSharp.text.Paragraph locationname = new iTextSharp.text.Paragraph();

                locationname.SpacingBefore = 10;
                locationname.SpacingAfter = 1;
                locationname.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                locationname.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                locationname.Add(LocationName);
                document.Add(locationname);

                 BarcodeQRCode barcodeQRCode = new BarcodeQRCode(code, 1000, 1000, null);
                Image codeQrImage = barcodeQRCode.GetImage();
                codeQrImage.ScaleAbsolute(95, 5);
                codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                document.Add(codeQrImage);
                /*    cb.Rectangle(Rectangular);
                    cb.Stroke();


                    cb.SetLineWidth(1);
                    cb.Rectangle(15, 230, 140, 50); //left width,top height,right width,bottom height
                    cb.BeginText();
                    BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cn, 18);
                    cb.SetTextMatrix(28, 250);
                    cb.ShowText("StationID:  " + Sno);
                    cb.EndText();

                    cb.Rectangle(15, 160, 140, 50);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnn, 18);
                    cb.SetTextMatrix(22, 180);
                    cb.ShowText("Station Name:  " + stationName);
                    cb.EndText();


                    cb.Rectangle(15, 90, 140, 50);
                    cb.SetLineWidth(3);
                    cb.Stroke();
                    cb.BeginText();
                    BaseFont f_cnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(f_cnnn, 18);
                    cb.SetTextMatrix(19, 110);
                    cb.ShowText("Location Name: " + locationName);
                    cb.EndText();
                    */


                // Page 2

                document.NewPage();


                iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(path);
                image1.ScaleAbsolute(75f, 75f);
                image1.SetAbsolutePosition(30f, 345f);
                image1.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                document.Add(image);

                string Phone1 = @"Phone:021-34829161/63";

                List<iTextSharp.text.Paragraph> paragraph1 = new List<iTextSharp.text.Paragraph>();


                iTextSharp.text.Paragraph phone1 = new iTextSharp.text.Paragraph();

                phone1.SpacingBefore = 50;
                phone1.SpacingAfter = 1;
                phone1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                phone1.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                phone1.Add(Phone1);
                document.Add(phone1);

                string Email1 = @"Email:info@ecoservices.com.pk";
                iTextSharp.text.Paragraph email1 = new iTextSharp.text.Paragraph();

                email1.SpacingBefore = 1;
                email1.SpacingAfter = 1;
                email1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                email1.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                email1.Add(Email1);
                document.Add(email1);

                string Web1 = @"Web: www.ecoservices.com.pk";
                iTextSharp.text.Paragraph web1 = new iTextSharp.text.Paragraph();

                web1.SpacingBefore = 1;
                web1.SpacingAfter = 30;
                web1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                web1.Font = FontFactory.GetFont(FontFactory.TIMES, 16F, BaseColor.DARK_GRAY);
                web1.Add(Web1);
                document.Add(web1);



                /*  PdfContentByte cb = writer.DirectContent;
                                   var Rectangular = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                                    Rectangular.BorderWidthLeft = 1.1f;
                                    Rectangular.BorderWidthRight = 1.1f;
                                    Rectangular.BorderWidthTop = 1.1f;
                                    Rectangular.BorderWidthBottom = 1.1f;*/

                string Station1 = stationName;
                iTextSharp.text.Paragraph station1 = new iTextSharp.text.Paragraph();

                station1.SpacingBefore = 10;
                station1.SpacingAfter = 25;
                station1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                station1.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                station1.Add(Station1);
                document.Add(station1);

                int StationNo1 = Sno;
                iTextSharp.text.Paragraph stationno1 = new iTextSharp.text.Paragraph();

                stationno1.SpacingBefore = 10;
                stationno1.SpacingAfter = 10;
                stationno1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                stationno1.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 60f, BaseColor.BLACK);
                stationno1.Add("" + Sno);
                document.Add(stationno1);



                string LocationName1 = locationName;
                iTextSharp.text.Paragraph locationname1 = new iTextSharp.text.Paragraph();

                locationname1.SpacingBefore = 10;
                locationname1.SpacingAfter = 1;
                locationname1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                locationname1.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                locationname1.Add(LocationName1);
                document.Add(locationname1);

                var path1 = Path.Combine(_hostingEnvironment.WebRootPath, @"images\Caution.png");

                iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(path1);
                image2.ScaleAbsolute(165f, 120f);
                image2.SetAbsolutePosition(60f, 30f);
                image2.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                //image.SpacingAfter = 40;
                document.Add(image2);

                //  PdfContentByte cb1 = writer.DirectContent;
                /*  var Rectangular1 = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                  Rectangular.BorderWidthLeft = 1.1f;
                  Rectangular.BorderWidthRight = 1.1f;
                  Rectangular.BorderWidthTop = 1.1f;
                  Rectangular.BorderWidthBottom = 1.1f;*/

                /*   cb.Rectangle(Rectangular);
                   cb.Stroke();


                   cb.SetLineWidth(1);
                   cb.Rectangle(45, 230, 190, 50); //left width,top height,right width,bottom height
                   cb.BeginText();
                   BaseFont f_cn1 = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                   cb.SetFontAndSize(f_cn, 10);
                   cb.SetTextMatrix(95, 250);
                   cb.ShowText("StationID:  " + Sno);
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

       */
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

        public  static byte[] DownloadAllPdf (List<DownloadPdfDto> downloadAllPdf,string locationName , IHostingEnvironment _hostingEnvironment)
        {
            try
            {
                var document = new iTextSharp.text.Document(new Rectangle(288f, 432f));
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                byte[] bytes;
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                var path = Path.Combine(_hostingEnvironment.WebRootPath, @"images\Eco.jpg");
                foreach (var s in downloadAllPdf)
                {
                   
                    document.Open();
                    Phrase phrase = new Phrase();
                    document.Add(phrase);


                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(path);
                    image.ScaleAbsolute(145f, 70f);
                    image.SetAbsolutePosition(70f, 345f);
                    image.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    //image.SpacingAfter = 40;
                    document.Add(image);


                    string Phone = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone = new iTextSharp.text.Paragraph();

                    phone.SpacingBefore = 50;
                    phone.SpacingAfter = 1;
                    phone.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    phone.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                    phone.Add(Phone);
                    document.Add(phone);

                    string Email = @"Email:info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email = new iTextSharp.text.Paragraph();

                    email.SpacingBefore = 1;
                    email.SpacingAfter = 1;
                    email.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    email.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                    email.Add(Email);
                    document.Add(email);

                    string Web = @"Web: www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web = new iTextSharp.text.Paragraph();

                    web.SpacingBefore = 1;
                    web.SpacingAfter = 30;
                    web.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    web.Font = FontFactory.GetFont(FontFactory.TIMES, 16F, BaseColor.DARK_GRAY);
                    web.Add(Web);
                    document.Add(web);



                    /*  PdfContentByte cb = writer.DirectContent;
                                       var Rectangular = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                                        Rectangular.BorderWidthLeft = 1.1f;
                                        Rectangular.BorderWidthRight = 1.1f;
                                        Rectangular.BorderWidthTop = 1.1f;
                                        Rectangular.BorderWidthBottom = 1.1f;*/

                    string Station = s.StationName;
                    iTextSharp.text.Paragraph station = new iTextSharp.text.Paragraph();

                    station.SpacingBefore = 10;
                    station.SpacingAfter = 25;
                    station.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    station.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                    station.Add(Station);
                    document.Add(station);

                    int StationNo = s.SNo;
                    iTextSharp.text.Paragraph stationno = new iTextSharp.text.Paragraph();

                    stationno.SpacingBefore = 10;
                    stationno.SpacingAfter = 10;
                    stationno.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    stationno.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 60f, BaseColor.BLACK);
                    stationno.Add("" + s.SNo);
                    document.Add(stationno);



                    string LocationName = locationName;
                    iTextSharp.text.Paragraph locationname = new iTextSharp.text.Paragraph();

                    locationname.SpacingBefore = 10;
                    locationname.SpacingAfter = 1;
                    locationname.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    locationname.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                    locationname.Add(LocationName);
                    document.Add(locationname);

                    BarcodeQRCode barcodeQRCode = new BarcodeQRCode(s.Code, 1000, 1000, null);
                    Image codeQrImage = barcodeQRCode.GetImage();
                    codeQrImage.ScaleAbsolute(95, 95);
                    codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    codeQrImage.Alignment = iTextSharp.text.Element.ALIGN_BOTTOM;
                    document.Add(codeQrImage);
                    /*    cb.Rectangle(Rectangular);
                        cb.Stroke();


                        cb.SetLineWidth(1);
                        cb.Rectangle(15, 230, 140, 50); //left width,top height,right width,bottom height
                        cb.BeginText();
                        BaseFont f_cn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetFontAndSize(f_cn, 18);
                        cb.SetTextMatrix(28, 250);
                        cb.ShowText("StationID:  " + Sno);
                        cb.EndText();

                        cb.Rectangle(15, 160, 140, 50);
                        cb.SetLineWidth(3);
                        cb.Stroke();
                        cb.BeginText();
                        BaseFont f_cnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetFontAndSize(f_cnn, 18);
                        cb.SetTextMatrix(22, 180);
                        cb.ShowText("Station Name:  " + stationName);
                        cb.EndText();


                        cb.Rectangle(15, 90, 140, 50);
                        cb.SetLineWidth(3);
                        cb.Stroke();
                        cb.BeginText();
                        BaseFont f_cnnn = BaseFont.CreateFont("c:\\windows\\fonts\\calibri.ttf", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        cb.SetFontAndSize(f_cnnn, 18);
                        cb.SetTextMatrix(19, 110);
                        cb.ShowText("Location Name: " + locationName);
                        cb.EndText();
                        */


                    // Page 2

                    document.NewPage();


                    iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(path);
                    image1.ScaleAbsolute(75f, 75f);
                    image1.SetAbsolutePosition(30f, 345f);
                    image1.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                    document.Add(image);

                    string Phone1 = @"Phone:021-34829161/63";

                    List<iTextSharp.text.Paragraph> paragraph1 = new List<iTextSharp.text.Paragraph>();


                    iTextSharp.text.Paragraph phone1 = new iTextSharp.text.Paragraph();

                    phone1.SpacingBefore = 50;
                    phone1.SpacingAfter = 1;
                    phone1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    phone1.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                    phone1.Add(Phone1);
                    document.Add(phone1);

                    string Email1 = @"Email:info@ecoservices.com.pk";
                    iTextSharp.text.Paragraph email1 = new iTextSharp.text.Paragraph();

                    email1.SpacingBefore = 1;
                    email1.SpacingAfter = 1;
                    email1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    email1.Font = FontFactory.GetFont(FontFactory.TIMES, 16f, BaseColor.DARK_GRAY);
                    email1.Add(Email1);
                    document.Add(email1);

                    string Web1 = @"Web: www.ecoservices.com.pk";
                    iTextSharp.text.Paragraph web1 = new iTextSharp.text.Paragraph();

                    web1.SpacingBefore = 1;
                    web1.SpacingAfter = 30;
                    web1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    web1.Font = FontFactory.GetFont(FontFactory.TIMES, 16F, BaseColor.DARK_GRAY);
                    web1.Add(Web1);
                    document.Add(web1);



                    /*  PdfContentByte cb = writer.DirectContent;
                                       var Rectangular = new Rectangle(7, 420, 280, 25); //left width,top height,right width,bottom height
                                        Rectangular.BorderWidthLeft = 1.1f;
                                        Rectangular.BorderWidthRight = 1.1f;
                                        Rectangular.BorderWidthTop = 1.1f;
                                        Rectangular.BorderWidthBottom = 1.1f;*/

                    string Station1 = s.StationName;
                    iTextSharp.text.Paragraph station1 = new iTextSharp.text.Paragraph();

                    station1.SpacingBefore = 10;
                    station1.SpacingAfter = 25;
                    station1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    station1.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                    station1.Add(Station1);
                    document.Add(station1);

                    int StationNo1 = s.SNo;
                    iTextSharp.text.Paragraph stationno1 = new iTextSharp.text.Paragraph();

                    stationno1.SpacingBefore = 10;
                    stationno1.SpacingAfter = 10;
                    stationno1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    stationno1.Font = FontFactory.GetFont(FontFactory.TIMES_BOLD, 60f, BaseColor.BLACK);
                    stationno1.Add("" + s.SNo);
                    document.Add(stationno1);



                    string LocationName1 = locationName;
                    iTextSharp.text.Paragraph locationname1 = new iTextSharp.text.Paragraph();

                    locationname1.SpacingBefore = 10;
                    locationname1.SpacingAfter = 1;
                    locationname1.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    locationname1.Font = FontFactory.GetFont(FontFactory.TIMES, 30f, BaseColor.BLACK);
                    locationname1.Add(LocationName1);
                    document.Add(locationname1);

                    var path1 = Path.Combine(_hostingEnvironment.WebRootPath, @"images\Caution.png");

                    iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(path1);
                    image2.ScaleAbsolute(165f, 120f);
                    image2.SetAbsolutePosition(60f, 30f);
                    image2.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    //image.SpacingAfter = 40;
                    document.Add(image2);
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
