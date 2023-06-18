using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Model.WorkSchedules
{
    internal class FileModel
    {
        private readonly FileInfo _FileInfo;

        public string Name => _FileInfo.Name;

        public string Path => _FileInfo.FullName;
        public FileModel(string path)
        {
            _FileInfo = new FileInfo(path);
        }
    }
}
