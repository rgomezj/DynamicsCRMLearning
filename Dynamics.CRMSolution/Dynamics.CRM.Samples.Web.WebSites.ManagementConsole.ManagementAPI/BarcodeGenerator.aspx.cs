//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Sdk.Client;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.ServiceModel.Description;
//using Microsoft.Xrm.Sdk.Query;
//using Microsoft.Crm.Sdk.Messages;
//using Telerik.Web.UI;

//namespace Pavliks.WAM.ManagementConsole.ManagementAPI
//{
//    public partial class BarcodeGenerator : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            RadBarcode barcode = new RadBarcode();
//            barcode.Type = BarcodeType.Code128;
//            barcode.Height = new Unit(120, UnitType.Pixel);
//            var barcodeText = Request.QueryString["BarcodeText"];

//            if(string.IsNullOrEmpty(barcodeText))
//            {
//                throw new ArgumentNullException("BarcodeText", "Text to generate the barcode was not specified (BarcodeText parameter)");
//            }
//            else
//            {
//                barcode.Text = barcodeText;
//                System.Drawing.Image image = barcode.GetImage();

//                if (Request.QueryString["Save"] != null)
//                {
//                    string path = HttpContext.Current.Server.MapPath("~") + @"\BarcodeImages\" + barcodeText + ".png";
//                    image.Save(path, System.Drawing.Imaging.ImageFormat.Png); //you can choose any ImageFormat. here
//                    Response.Write("Image saved to:" + path);
//                }
//                else
//                {
//                    Response.ContentType = "image/png"; //You can  put Response.ContentType = "image/jpeg" here 
//                    image.Save(HttpContext.Current.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png); //you can choose any ImageFormat. here
//                    Response.End();
//                }
//            }
//        }
//    }
//}