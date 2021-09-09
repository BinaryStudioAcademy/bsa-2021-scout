using Domain.Interfaces.Read;
using Infrastructure.Files.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Files.Read
{
    public class ImageReadRepository : IImageReadRepository
    {
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IUserReadRepository _applicantReadRepository;

        public ImageReadRepository(IFileReadRepository fileReadRepository, IUserReadRepository applicantReadRepository)
        {
            _fileReadRepository = fileReadRepository;
            _applicantReadRepository = applicantReadRepository;
        }

        public async Task<string> GetPublicUrlAsync(string applicantId)
        {
            var applicantCvFileInfo = await _applicantReadRepository.GetAvatarInfoAsync(applicantId);

            return await _fileReadRepository.GetPublicUrlAsync(
                applicantCvFileInfo.Path,
                applicantCvFileInfo.Name);
        }
    }
}