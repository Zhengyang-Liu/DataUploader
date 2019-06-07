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
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)   
            {
                FolderPathText.Text = dialog.SelectedPath;  
            }  
        }

        private void Backfill_Button_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(FolderPathText.Text);

            foreach (FileInfo file in d.GetFiles("*.txt"))
            {
                StreamReader sr = file.OpenText();
                StringManager stringManager = new StringManager(sr);
                List<DataStruct> DataList = stringManager.CreateDataList();

                foreach (DataStruct item in DataList)
                {
                    databaseManager.Upsert("SensorData", item.dateTime, item.acceleration);
                }
            }
        }
    }
}
