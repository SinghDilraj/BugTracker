using System.Web;
namespace Blog.Models.Extensions
{
    public static class FileHelper
    {
        public static readonly string UploadFolder = "/upload/";
        public static readonly string MappedUploadFolder = HttpContext.Current.Server.MapPath(UploadFolder);
    }
}