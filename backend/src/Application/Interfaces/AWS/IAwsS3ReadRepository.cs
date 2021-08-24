using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface IAwsS3ReadRepository
    {
        Task<byte[]> ReadAsync(string filePath);
        Task<byte[]> ReadAsync(string filePath, string fileName);
        Task<string> GetPublicUrlAsync(string filePath, string fileName);
        Task<string> GetSignedUrlAsync(string filePath, string fileName, TimeSpan timeSpan);
    }
}
