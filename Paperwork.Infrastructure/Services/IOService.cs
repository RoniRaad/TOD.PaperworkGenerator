using Paperwork.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paperwork.Infrastructure.Services
{
    public class IOService : IIOService
    {
        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public void ZipFolderIntoFile(string folderPath, string zipFilePath)
        {
            ZipFile.CreateFromDirectory(folderPath, zipFilePath);
        }
    }
}
