﻿using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Files.Abstraction
{
    public interface IFileReadRepository
    {
        public Task<string> GetSignedUrlAsync(string filePath, string fileName, TimeSpan timeSpan);
    }
}
