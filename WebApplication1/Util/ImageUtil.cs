using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace API.Util
{
    public class ImageUtil
    {
        //图片 转为    base64编码的文本
        public static void ImgToBase64String(string Imagefilename="/UserPhotos/test.jpg")
        {
            try
            {
                
                Bitmap bmp = new Bitmap(Imagefilename);
                FileStream fs = new FileStream(Imagefilename + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                sw.Write(strbaser64);

                sw.Close();
                fs.Close();
                // MessageBox.Show("转换成功!");
            }
            catch (Exception ex)
            {
                
            }
        }

      
        //base64编码的文本 转为    图片
        public static string Base64StringToImage(string filePath,string base64)
        {
            string guid = Guid.NewGuid().ToString();
            try
            {
                byte[] arr = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                string imgName = guid + ".png";
                bmp.Save(filePath + imgName, ImageFormat.Png);
                ms.Close();
                return imgName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}