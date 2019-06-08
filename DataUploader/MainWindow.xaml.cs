using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace DataUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseManager databaseManager = new DatabaseManager();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPathText.Text = dialog.SelectedPath;
            }
        }

        private void Backfill_Button_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(FolderPathText.Text);
            DirectoryInfo[] subDiArr = di.GetDirectories();

            foreach (DirectoryInfo subDi in subDiArr)
            {
                databaseManager.CreateTable(subDi.Name);

                foreach (FileInfo file in subDi.GetFiles("*.txt"))
                {
                    StreamReader sr = file.OpenText();
                    StringManager stringManager = new StringManager(sr);
                    List<String[]> DataList = stringManager.CreateDataList();
                    databaseManager.BulkInsert(subDi.Name, DataList);
                }
            }
        }

        private void StartWatching_Button_Click(object sender, RoutedEventArgs e)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = FolderPathText.Text;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;

            watcher.EnableRaisingEvents = true;
        }

        void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
    }
}
