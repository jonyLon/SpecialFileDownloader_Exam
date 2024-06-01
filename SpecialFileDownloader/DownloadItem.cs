using System.ComponentModel;

namespace SpecialFileDownloader
{
    public partial class MainWindow
    {
        public class DownloadItem : INotifyPropertyChanged
        {
            private string fileName;
            private string fileStatus;
            private int progress;

            public string FileName
            {
                get => fileName;
                set
                {
                    fileName = value;
                    OnPropertyChanged("FileName");
                }
            }

            public string FileStatus
            {
                get => fileStatus;
                set
                {
                    fileStatus = value;
                    OnPropertyChanged("FileStatus");
                }
            }
            public int Progress
            {
                get => progress;
                set
                {
                    progress = value;
                    OnPropertyChanged("Progress");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}