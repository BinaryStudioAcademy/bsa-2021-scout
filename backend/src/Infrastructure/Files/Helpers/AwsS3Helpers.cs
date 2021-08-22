using Domain.Entities;

namespace Infrastructure.Files.Helpers
{
    public static class AwsS3Helpers
    {
        public static string GetFileKey(string filePath, string fileName)
        {
            return $"{filePath}/{fileName}";
        }

        public static string GetFileKey(FileInfo fileInfo)
        {
            return GetFileKey(fileInfo.Path, fileInfo.Name);
        }
    }
}
