using System.Windows.Controls;
using System.Windows.Media;

namespace SpecialFileDownloader
{
    public partial class MainWindow
    {
        class FocusablePlaceholder
        {
            public static void RemoveText(object sender, EventArgs e)
            {
                TextBox textBox = sender as TextBox;
                if (textBox.Text == "Enter file URL" || textBox.Text == "Enter save path")
                {
                    textBox.Text = "";
                    textBox.Foreground = Brushes.Black;
                }
            }

            public static void AddText(object sender, EventArgs e)
            {
                TextBox textBox = sender as TextBox;
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    if (textBox.Name == "UrlTextBox")
                    {
                        textBox.Text = "Enter file URL";
                    }
                    else if (textBox.Name == "SavePathTextBox")
                    {
                        textBox.Text = "Enter save path";
                    }
                    textBox.Foreground = Brushes.Gray;
                }
            }
        }

    }
}