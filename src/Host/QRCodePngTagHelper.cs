using System;
using System.IO;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing.QrCode;

namespace Host
{
    [HtmlTargetElement("qrcode-png")]
    public class QRCodePngTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // string url, string alt = "QR code", int height = 500, int width = 500, int margin = 0)
            var content = context.AllAttributes["content"].Value.ToString();
            var alt = context.AllAttributes["alt"].Value.ToString();
            var width = Convert.ToInt32(context.AllAttributes["width"] == null ? "500" : context.AllAttributes["width"].Value.ToString());
            var height = Convert.ToInt32(context.AllAttributes["height"] == null ? "500" : context.AllAttributes["height"].Value.ToString());
            var margin = Convert.ToInt32(context.AllAttributes["margin"] == null ? "500" : context.AllAttributes["margin"].Value.ToString());

            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions { Height = height, Width = width, Margin = margin }
            };


            var pixelData = qrWriter.Write(content);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
            using (var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            using (var ms = new MemoryStream())
            {
                // lock the data area for fast access
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
                   System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                       pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                // save to stream as PNG
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                output.TagName = "img";
                output.Attributes.Clear();
                output.Attributes.Add("width", width);
                output.Attributes.Add("height", height);
                output.Attributes.Add("alt", alt);
                output.Attributes.Add("src",
                   String.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray())));
            }
        }
    }
}
