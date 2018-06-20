using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Helper
{
    public class DownloadPdf
    {
         Document _document;
         PdfWriter _pdfWriter;
         Phrase _phrase;


        public static byte[] Download(int stationId,string stationName,string locationName )
        {
            try
            {
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();



                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                Phrase phrase = new Phrase();
                document.Add(phrase);

                // Chunk chunk = new Chunk(id.ToString());

                //document.Add(chunk);


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
