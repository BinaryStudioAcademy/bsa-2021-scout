using Application.Common.Files;
using Application.Common.Files.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Application.Common.Validators
{
    public static class FileExtensionValidationRule
    {
        public static IRuleBuilderOptions<T, FileDto> ExtensionMustBeInList<T>(this IRuleBuilder<T, FileDto> rule, IEnumerable<FileExtension> extensions)
        {
            return rule
                .Must(file => extensions.Select(e => e.Value).Contains(Path.GetExtension(file.FileName).ToLower()))
                .WithMessage("File extension is not valid");
        }
    }
}
