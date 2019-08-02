using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace TestOnlineBase.Helper.FileHelper
{
    public  class UploadImageFile
    {
        public static string UploadImage(IFormFile file)
        {
            try
            {
                var fileName = file.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload_img", fileName);
                if (File.Exists(path))
                {
                    return fileName;
                }
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);

                }
                return fileName;
            }catch(Exception)
            {
                return null;
            }
          
        }

        public static string SaveImg(Image img)
        {
            if (img != null )
            {
                var imgName = Guid.NewGuid().ToString()+".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imgName);        
                img.Save(path);
                return imgName;
            }
            return null;
        }

        public static void UploadFile(IFormFile file)
        {
            
        }
    }
}
