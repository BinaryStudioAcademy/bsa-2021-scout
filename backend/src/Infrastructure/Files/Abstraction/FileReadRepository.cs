using Application.Interfaces.AWS;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Files.Abstraction
{
    public class FileReadRepository : IFileReadRepository
    {
        private readonly IAwsS3ReadRepository _awsS3ReadRepository;

        public FileReadRepository(IAwsS3ReadRepository awsS3ReadRepository)
        {
            _awsS3ReadRepository = awsS3ReadRepository;
        }

        public Task<string> GetPublicUrlAsync(string filePath, string fileName)
        {
            return _awsS3ReadRepository.GetPublicUrlAsync(filePath, fileName);
        }

        public Task<string> GetSignedUrlAsync(string filePath, string fileName, TimeSpan timeSpan)
        {
            return _awsS3ReadRepository.GetSignedUrlAsync(filePath, fileName, timeSpan);
        }
    }
}
