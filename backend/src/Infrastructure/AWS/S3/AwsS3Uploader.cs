using System.IO;
using System.Threading.Tasks;
using Application.Interfaces.AWS;

namespace Infrastructure.AWS
{
    public class AwsS3Uploader : IS3Uploader
    {
        private readonly IAwsS3ReadRepository _awsS3ReadRepository;
        private readonly IAwsS3WriteRepository _awsS3WriteRepository;

        public AwsS3Uploader(IAwsS3ReadRepository awsS3ReadRepository, IAwsS3WriteRepository awsS3WriteRepository)
        {
            _awsS3ReadRepository = awsS3ReadRepository;
            _awsS3WriteRepository = awsS3WriteRepository;
        }

        public async Task<byte[]> ReadAsync(string path)
        {
            return await _awsS3ReadRepository.ReadAsync(path);
        }

        public async Task UploadAsync(string path, byte[] fileContent)
        {
            await _awsS3WriteRepository.UploadAsync(path, fileContent);
        }

        public async Task UploadAsync(string path, Stream fileContent)
        {
            await _awsS3WriteRepository.UploadAsync(path, fileContent);
        }
    }
}
