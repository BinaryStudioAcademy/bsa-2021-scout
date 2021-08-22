using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Files
{
    public class FileExtension
    {
        private FileExtension(string value) { Value = value; }

        public string Value { get; private set; }

        public static FileExtension Pdf { get { return new FileExtension(".pdf"); } }
    }
}
