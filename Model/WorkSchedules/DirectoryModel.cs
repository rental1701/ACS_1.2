using ACS.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACS.Model.WorkSchedules
{
    internal class DirectoryModel : ViewModel
    {
        private readonly DirectoryInfo _DirectotyInfo;

        public string Name => _DirectotyInfo.Name;
        public string Path => _DirectotyInfo.FullName;
      
        public IEnumerable<DirectoryInfo> SubDirectories
        {
            get
            {
                try
                {
                    var temp = _DirectotyInfo.EnumerateDirectories().Select(dir_info => new DirectoryInfo(dir_info.FullName));
                    return temp;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public IEnumerable<FileModel> Files
        {
            get
            {
                try
                {
                    var temp = _DirectotyInfo.EnumerateFiles()
                        .Select(f => new FileModel(f.FullName));
                    return temp;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        public IEnumerable<object> DirectoryItems
        {
            get
            {
                try
                {
                    var temp = SubDirectories.Cast<object>().Concat(Files);
                    return temp;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }
        public DirectoryModel(string path)
        {
            _DirectotyInfo = new DirectoryInfo(path);
        }
    }
}
