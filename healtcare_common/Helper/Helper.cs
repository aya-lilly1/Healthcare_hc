using HealthCare_Common.Extensions;
using System;
using System.IO;


namespace HealthCare_Helper
{
    public class Helper
    {
        public static string SaveImage(string base64img, string baseFolder)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(baseFolder))
                {
                    throw new ServiceValidationException("Invalid folder name for upload images");
                }

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var base64Array = base64img.Split(";base64,");
                if (base64Array.Length < 1)
                {
                    return "";
                }

                base64img = base64Array[1];
                var fileName = $"{Guid.NewGuid()}{"Logo.png"}".Replace("-", string.Empty);
                
                if (!string.IsNullOrWhiteSpace(folderPath))
                {
                    var url = $@"{baseFolder}\{fileName}";
                    fileName = @$"{folderPath}\{fileName}";
                    File.WriteAllBytes(fileName, Convert.FromBase64String(base64img));
                    return url;
                }

                return "";
            }
            catch (Exception ex)
            {
                throw new ServiceValidationException(ex.Message);
            }
        }

    }
}
