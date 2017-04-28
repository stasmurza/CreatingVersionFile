using System;
using System.Collections.Generic;
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
using System.IO;
using System.Windows.Forms;
using InstallUpdates;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Collections.ObjectModel;

namespace CreatingVersionFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<FilesForDownloading> collectionOfFiles = new ObservableCollection<FilesForDownloading>();


        public MainWindow()
        {
            InitializeComponent();

            lstFiles.ItemsSource = collectionOfFiles;

            collectionOfFiles.CollectionChanged += (o, e) =>
            {
                collectionOfFiles = new ObservableCollection<FilesForDownloading>(collectionOfFiles.ToList<FilesForDownloading>());
            };
        }


        private void validationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                System.Windows.MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }


        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            collectionOfFiles = new ObservableCollection<FilesForDownloading>();
            lstFiles.ItemsSource = collectionOfFiles;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "DefaultOutputName.json";
            saveFileDialog.Filter = "Json File | *.json";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<FilesForDownloading>));

                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, collectionOfFiles);

                memoryStream.Position = 0;
                StreamReader sr = new StreamReader(memoryStream);                

                File.WriteAllText(saveFileDialog.FileName, sr.ReadToEnd());
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            collectionOfFiles.Remove((FilesForDownloading)lstFiles.SelectedItem);
        }

        private void btnAddNewRow_Click(object sender, RoutedEventArgs e)
        {
            collectionOfFiles.Add(new FilesForDownloading());
            lstFiles.ItemsSource = collectionOfFiles;
            lstFiles.Items.Refresh();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            DialogResult result = openFileDialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            List<FilesForDownloading> listOfFiles = DownloadingFiles.GetListOfFiles(openFileDialog.FileName);

            collectionOfFiles = new ObservableCollection<FilesForDownloading>(listOfFiles);

            lstFiles.ItemsSource = collectionOfFiles;

            openFileDialog.Dispose();

        }

        private void lstFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstFiles.Items.Refresh();
        }

        private void txtWebDirectory_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            
        }
    }
}
