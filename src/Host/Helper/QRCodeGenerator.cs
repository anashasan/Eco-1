using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using Net.ConnectCode.BarcodeFontsStandard2D;
using System.IO;
using ZXing.QrCode;
using System.Drawing;
using QRCoder;

namespace Host.Helper
{
    [HtmlTargetElement("qrcode")]
    public class QRCodeGenerator
    {
        public Bitmap QR()
        {
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;




        }
    }


}

