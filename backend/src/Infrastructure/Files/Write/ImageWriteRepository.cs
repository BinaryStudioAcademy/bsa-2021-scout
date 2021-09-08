using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using Infrastructure.Files.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Files.Write
{
    class ImageWriteRepository: IImageWriteRepository
    {
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IUserReadRepository _userReadRepository;

        public ImageWriteRepository(IFileWriteRepository fileWriteRepository, IUserReadRepository userReadRepository)
        {
            _fileWriteRepository = fileWriteRepository;
            _userReadRepository = userReadRepository;
        }

        public Task<FileInfo> UploadAsync(string userId, System.IO.Stream cvFileContent)
        {
            return _fileWriteRepository.UploadPublicFileAsync(
                GetFilePath(),
                GetFileName(userId),
                cvFileContent);
        }

        public async Task UpdateAsync(string applicantId, System.IO.Stream imageContent)
        {
            var imageInfo = await _userReadRepository.GetAvatarInfoAsync(applicantId);
            await _fileWriteRepository.UpdateFileAsync(imageInfo, imageContent);
        }

        public async Task DeleteAsync(FileInfo cvFileInfo)
        {
            await _fileWriteRepository.DeleteFileAsync(cvFileInfo);
        }

        private static string GetFilePath()
        {
            return "images";
        }

        private static string GetFileName(string userId)
        {
            return $"{userId}.jpg";
        }
    }
}
