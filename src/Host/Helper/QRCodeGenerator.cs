using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using Net.ConnectCode.BarcodeFontsStandard2D;
using System.IO;
using ZXing.QrCode;

namespace Host.Helper
{
    [HtmlTargetElement("qrcode")]
    public class QRCodeGenerator
    {
        public string Message { get; set; }

        public string QrCode()
        {
            QR qrCode = new QR("12345678", "M", 8);
            string output = qrCode.Encode();
            Message = output;
            return Message;
        }
    }


}

