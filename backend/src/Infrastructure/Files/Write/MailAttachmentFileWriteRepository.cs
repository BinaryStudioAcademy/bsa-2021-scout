using Application.Common.Exceptions.Applicants;
using Application.Interfaces.AWS;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using Infrastructure.Files.Abstraction;
using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Read
{
    public class MailAttachmentFileWriteRepository : IMailAttachmentFileWriteRepository
    {
        private readonly IAwsS3WriteRepository _awsS3WriteRepository;

        public MailAttachmentFileWriteRepository(IAwsS3WriteRepository awsS3WriteRepository)
        {
            _awsS3WriteRepository = awsS3WriteRepository;
        }

        public Task UploadAsync(string key, Stream mailAttachmentFileContent)
        {
             return _awsS3WriteRepository.UploadAsync(
                key, 
                mailAttachmentFileContent);
        }

        public Task DeleteAsync(string filePath, string fileName)
        {
            return _awsS3WriteRepository.DeleteAsync(filePath, fileName);
        }
    }
}
