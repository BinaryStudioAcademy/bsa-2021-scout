using Application.Common.Files.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validators
{
    public static class FileExtensionValidationRule
    {
        public static IRuleBuilderOptions<T, FileDto> ExtensionMustBeInList<T>(this IRuleBuilder<T, FileDto> rule, string[] extensions)
        {
            return rule
                .Must(file => {
                    Console.WriteLine(extensions.Contains(Path.GetExtension(file.FileName).ToLower()));
                    return extensions.Contains(Path.GetExtension(file.FileName).ToLower());
                    })
                .WithMessage("File extension is not valid");
        }
    }
}
