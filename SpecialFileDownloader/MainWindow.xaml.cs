using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace SpecialFileDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebClient webClient;
        private ObservableCollection<DownloadItem> downloads;

        public MainWindow()
        {
            InitializeComponent();

            downloads = new ObservableCollection<DownloadItem>();
            DownloadListBox.ItemsSource = downloads;

            webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            
            UrlTextBox.GotFocus += FocusablePlaceholder.RemoveText;
            UrlTextBox.LostFocus += FocusablePlaceholder.AddText;
            SavePathTextBox.GotFocus += FocusablePlaceholder.RemoveText;
            SavePathTextBox.LostFocus += FocusablePlaceholder.AddText;
        }

        private void StartDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var url = UrlTextBox.Text;
            var savePath = SavePathTextBox.Text;
            var directoryPath = Path.GetDirectoryName(savePath);

            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The specified directory does not exist.");
                return;
            }

            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(savePath))
            {
                if (File.Exists(savePath))
                {
                    MessageBox.Show("File already exists. Please choose a different save path.");
                    return;
                }
                var downloadItem = new DownloadItem { FileName = savePath, Progress = 0, FileStatus = "Downloading..." };
                downloads.Add(downloadItem);
                webClient.DownloadFileAsync(new Uri(url), savePath, downloadItem);

            }
            else
            {
                MessageBox.Show("Please enter a valid URL and save path.");
            }
        }

        private void StopDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
                MessageBox.Show("Download stoped");
            }
        }

        private void DeleteDownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadListBox.SelectedItem != null)
            {
                var selectedItem = DownloadListBox.SelectedItem as DownloadItem;
                var fileName = selectedItem.FileName;

                if (selectedItem != null)
                {
                    try
                    {
                        // Додамо лог для відлагодження шляху до файлу
                        MessageBox.Show($"Attempting to delete file at: {fileName}");

                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                            downloads.Remove(selectedItem); // Видалення елемента з колекції
                            MessageBox.Show($"Deleted: {fileName}");
                        }
                        else
                        {
                            downloads.Remove(selectedItem); // Видалення елемента з колекції, якщо файл не існує
                            MessageBox.Show("The file does not exist, but the entry was removed from the list.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting file: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a download to delete.");
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState is DownloadItem downloadItem)
            {
                if (e.Cancelled)
                {
                    downloadItem.FileStatus = "Cancelled";
                }
                else if (e.Error != null)
                {
                    downloadItem.FileStatus = "Error: " + e.Error.Message;
                }
                else
                {
                    downloadItem.FileStatus = "Completed";
                }
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.UserState is DownloadItem downloadItem)
            {
                downloadItem.Progress = e.ProgressPercentage;
            }
        }
        private void DownloadListBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (DownloadListBox.SelectedItem == null)
            {
                e.Handled = true; // Disable context menu if no item is selected
            }
        }
    }
}