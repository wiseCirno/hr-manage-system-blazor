using System.Security.Cryptography;
using System.Text;

namespace HRMSystemLiu.DAL;

public static class CommonHelper
{
    public static string Md5EncryptI32(string raw)
    {
        ArgumentNullException.ThrowIfNull(raw);
        var sb = new StringBuilder();
        foreach (var b in MD5.HashData(Encoding.UTF8.GetBytes(raw))) sb.Append(b.ToString("x2"));

        return sb.ToString();
    }
    
    public static string GetImageContentType(byte[] imageData)
    {
        if (imageData is [0xFF, 0xD8, 0xFF, ..])
        {
            return "image/jpeg";
        }

        return imageData.Length switch
        {
            >= 8 when imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47 &&
                      imageData[4] == 0x0D && imageData[5] == 0x0A && imageData[6] == 0x1A &&
                      imageData[7] == 0x0A => "image/png",
            >= 6 when ((imageData[0] == 'G' && imageData[1] == 'I' && imageData[2] == 'F' && imageData[3] == '8' &&
                        imageData[4] == '7' && imageData[5] == 'a') || (imageData[0] == 'G' && imageData[1] == 'I' &&
                                                                        imageData[2] == 'F' && imageData[3] == '8' &&
                                                                        imageData[4] == '9' &&
                                                                        imageData[5] == 'a')) => "image/gif",
            >= 2 when imageData[0] == 0x42 && imageData[1] == 0x4D => "image/bmp",
            >= 4 when ((imageData[0] == 0x49 && imageData[1] == 0x49 && imageData[2] == 0x2A && imageData[3] == 0x00) ||
                       (imageData[0] == 0x4D && imageData[1] == 0x4D && imageData[2] == 0x00 && imageData[3] == 0x2A))
                => "image/tiff",
            _ => "image/unknown"
        };
    }
}