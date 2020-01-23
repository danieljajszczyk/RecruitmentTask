using System.IO;
using Microsoft.AspNetCore.Http;

namespace RecruitmentTask.Api.Models
{
    public class FileProcessor
    {
        public static byte[] GetFileBytes(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
