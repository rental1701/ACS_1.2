using ACS.ViewModels.Base;
using System.IO;

namespace ACS.ViewModels.WorkSchedules
{
    public class FileVM : ViewModel
    {
        private FileInfo _FileInfo;

        public string Name => _FileInfo.Name;

        public string Path => _FileInfo.FullName;

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                Set(ref _IsSelected, value);
                if (IsSelected)
                    ParentFolder.SelectedFile = this;

            }
        }

        public WorkSchedulesVM ParentFolder { get; set; }
        public FileVM(string Path, WorkSchedulesVM workSchedules)
        {
            _FileInfo = new FileInfo(Path);
            ParentFolder = workSchedules;
        }
    }
}
