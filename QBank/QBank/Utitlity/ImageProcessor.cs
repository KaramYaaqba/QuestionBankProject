//using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

    public class ImageProcessor
    {
        private const string Root = "/app/uploads/image/";
        private const string ImagePath = Root + "{unique_user_id}/"; //QBManager.QBank.Name + "/";

        //If set true, image will be extracted and deletes - This is to ensure that images are not corrupt
        private bool _onlyValiate = false;
        //It will be set to True if image is valid
        private bool _IsValid = false;

        private string _path;

        public ImageProcessor(string imageFolder)
        {
            //string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            //UriBuilder uri = new UriBuilder(codeBase);
            _path = imageFolder;
        }

        public bool ValidateImage(string source)
        {
            _onlyValiate = true;
            string s = ExtractImage(source);
            return _IsValid;
        }

        public string ExtractImage(string source)
        {
            //if (source == null)
            //{
            //    _IsValid = true;
            //    return "";
            //}

            //string fullOutputPath, attribVal, extn, fileName;
            //System.Drawing.Imaging.ImageFormat format;

            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(source);

            //var imgTags = doc.DocumentNode.SelectNodes("//img");

            //if (imgTags != null)
            //{
            //    foreach (HtmlNode node in imgTags)
            //    {
            //        attribVal = node.Attributes["src"].Value;
            //        string[] content = attribVal.Split(',');

            //        if (IsBase64(content[1]))
            //        {
            //            if (content[0].IndexOf("jpeg") > 0)
            //            {
            //                format = System.Drawing.Imaging.ImageFormat.Jpeg;
            //                extn = "jpeg";
            //            }
            //            else if (content[0].IndexOf("ico") > 0)
            //            {
            //                format = System.Drawing.Imaging.ImageFormat.Icon;
            //                extn = "ico";
            //            }
            //            else if (content[0].IndexOf("gif") > 0)
            //            {
            //                format = System.Drawing.Imaging.ImageFormat.Gif;
            //                extn = "gif";
            //            }
            //            else
            //            {
            //                format = System.Drawing.Imaging.ImageFormat.Png;
            //                extn = "png";
            //            }

            //            fileName = Guid.NewGuid().ToString() + "." + extn;
            //            fullOutputPath = _path + @"\" + fileName;
            //            //Save Image
            //            _IsValid = SaveByteArrayAsImage(fullOutputPath, content[1], format);

            //            //Replace the value of base64 string with file name
            //            node.Attributes["src"].Value = ImagePath + fileName;
            //        }
            //        else
            //        {
            //            node.Attributes["src"].Value = Root + "missing.png";
            //        }
            //    }
            //    return doc.DocumentNode.InnerHtml;
            //}
            //else
            //{
            //    _IsValid = true;
            //    return source;
            //}
            return "redo";
        }

        private bool SaveByteArrayAsImage(string fullOutputPath, string base64String, System.Drawing.Imaging.ImageFormat format)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }

                Bitmap bmp = new Bitmap(image);
                bmp.MakeTransparent(Color.Transparent);
                bmp.Save(fullOutputPath, format);
                bmp.Dispose();
                if (_onlyValiate)
                {
                    try
                    {
                        //Delete image
                        File.Delete(fullOutputPath);
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
        }

        private  bool IsBase64(string base64String)
        {
            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }
    }
