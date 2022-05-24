using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileSharingApp.BL.Helper
{
    public static class FilesHelper
    {


        public static async Task<string> UploadFile(IFormFile file, string folderName = "Files")
        {
            try
            {
                string newFileName = string.Concat(Guid.NewGuid(), Path.GetExtension(file.FileName));
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, newFileName);

                using FileStream fs = File.Create(path); //created file
                await file.CopyToAsync(fs);  // fill content file
                return newFileName;
            }
            catch (Exception ex)
            { 
                return ex.Message;
            }

        }


        public static void RemoveFile(string fileName, string folderName = "Files")
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, fileName);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }






}
}
