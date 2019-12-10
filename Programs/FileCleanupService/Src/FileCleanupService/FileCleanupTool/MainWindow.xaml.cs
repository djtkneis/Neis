using Neis.FileCleanup.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceProcess;
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
using System.Xml.Serialization;

namespace Neis.FileCleanupTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Routed command Browse
        /// </summary>
        public static RoutedCommand BrowseCommand = new RoutedCommand("Browse", typeof(MainWindow));
        /// <summary>
        /// Routed command Save
        /// </summary>
        public static RoutedCommand SaveCommand = new RoutedCommand("Save", typeof(MainWindow));
        /// <summary>
        /// Routed command Exit
        /// </summary>
        public static RoutedCommand ExitCommand = new RoutedCommand("Exit", typeof(MainWindow));

        /// <summary>
        /// List of changed properties
        /// </summary>
        private List<string> _changedProperties = new List<string>();

        /// <summary>
        /// <see cref="FileCleanupConfiguration"/> cast of the <see cref="DataContext"/>
        /// </summary>
        public FileCleanupConfiguration Configuration
        {
            get { return DataContext as FileCleanupConfiguration; }
        }

        /// <summary>
        /// Constructor for the <see cref="MainWindow"/> class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(BrowseCommand, ExecutedBrowse));
            CommandBindings.Add(new CommandBinding(ExitCommand, ExecutedExit));
            CommandBindings.Add(new CommandBinding(SaveCommand, ExecutedSave, CanExecuteSave));

            this.IsEnabled = false;

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        /// <summary>
        /// Gets the configuration from the File Cleanup Service
        /// </summary>
        private void GetConfiguration()
        {
            try
            {
                HttpWebResponse webResponse = null;
                if (SendServiceWebRequest("GetConfiguration", "GET", out webResponse) && webResponse != null && webResponse.StatusCode == HttpStatusCode.OK)
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(FileCleanupConfiguration));
                    FileCleanupConfiguration config = serializer.ReadObject(webResponse.GetResponseStream()) as FileCleanupConfiguration;
                    
                    foreach (DirectoryConfiguration dir in config.Directories)
                    {
                        dir.IsDirty = false;
                    }
                    config.IsDirty = false;

                    config.PropertyChanged += Configuration_PropertyChanged;
                    DataContext = config;
                }
                else
                {
                    MessageBox.Show("Web response is not OK", "Invalid Response", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while getting configurataion: " + Environment.NewLine + ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        /// <summary>
        /// Saves the changes to the configuration
        /// </summary>
        private void SaveChanges()
        {
            if (_changedProperties != null)
            {
                List<string> errors = new List<string>();
                _changedProperties.ForEach(prop =>
                {
                    HttpWebResponse webResponse = null;
                    switch (prop)
                    {
                        case FileCleanupConfiguration.ArchiveDaysPropertyName:
                            {
                                string endpoint = string.Format("SetArchiveDays?age={0}", Configuration.ArchiveDays);
                                if (!SendServiceWebRequest(endpoint, "GET", out webResponse) || webResponse == null || webResponse.StatusCode != HttpStatusCode.OK)
                                {
                                    errors.Add("Error saving 'archive days'");
                                }
                            }
                            break;

                        case FileCleanupConfiguration.ArchiveLocationPropertyName:
                            {
                                string endpoint = string.Format("SetArchiveLocation?path={0}", Configuration.ArchiveLocation);
                                if (!SendServiceWebRequest(endpoint, "GET", out webResponse) || webResponse == null || webResponse.StatusCode != HttpStatusCode.OK)
                                {
                                    errors.Add("Error saving 'archive location'");
                                }
                            }
                            break;

                        case FileCleanupConfiguration.CleanupTimePropertyName:
                            {
                                string endpoint = string.Format("SetCleanupTime?timeString={0}", Configuration.CleanupTime);
                                if (!SendServiceWebRequest(endpoint, "GET", out webResponse) || webResponse == null || webResponse.StatusCode != HttpStatusCode.OK)
                                {
                                    errors.Add("Error saving 'cleanup time'");
                                }
                            }
                            break;

                        case FileCleanupConfiguration.RemoveEmptyFoldersPropertyName:
                            {
                                string endpoint = string.Format("SetRemoveEmptyFolders?remove={0}", Configuration.RemoveEmptyFolders);
                                if (!SendServiceWebRequest(endpoint, "GET", out webResponse) || webResponse == null || webResponse.StatusCode != HttpStatusCode.OK)
                                {
                                    errors.Add("Error saving 'remove empty folders'");
                                }
                            }
                            break;

                        case FileCleanupConfiguration.DirectoriesPropertyName:
                            break;
                    }
                });

                if (errors.Count > 0)
                {
                    StringBuilder errorString = new StringBuilder();
                    errorString.AppendLine("Error while saving: ");
                    for (int i = 0; i < errors.Count; i++)
                    {
                        errorString.AppendLine(string.Format("    {0}. {1}", i + 1, errors[i]));
                    }

                    MessageBox.Show(this, errorString.ToString(), "Save error");
                }
                else
                {
                    MessageBox.Show(this, "Save Complete!", "Save complete");
                    _changedProperties.Clear();
                }

            }
        }
        /// <summary>
        /// Sends a web request to the File Cleanup Service
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="method">Web method</param>
        /// <param name="response">Response from web request</param>
        /// <returns>True if successful.  False if not.</returns>
        private bool SendServiceWebRequest(string endpoint, string method, out HttpWebResponse response)
        {
            string uri = string.Format("{0}/{1}",
                            Properties.Settings.Default.ServiceEndpoint,
                            endpoint);
            var webRequest = HttpWebRequest.Create(uri);
            webRequest.Method = "GET";

            try
            {
                response = webRequest.GetResponse() as HttpWebResponse;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception while setting archive days: " + Environment.NewLine + ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                response = null;
                return false;
            }
        }

        /// <summary>
        /// Determines whether or not Save can execute
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        protected void CanExecuteSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Configuration != null && _changedProperties != null && _changedProperties.Count > 0;
        }

        /// <summary>
        /// Executes Browse
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        public void ExecutedBrowse(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            dlg.SelectedPath = Configuration.ArchiveLocation;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Configuration.ArchiveLocation = dlg.SelectedPath;
            }
        }
        /// <summary>
        /// Executes Exit
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        public void ExecutedExit(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Executes Save
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        public void ExecutedSave(object sender, ExecutedRoutedEventArgs e)
        {
            SaveChanges();
        }

        /// <summary>
        /// Event for when a property changes on the configuration object
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private void Configuration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!_changedProperties.Contains(e.PropertyName))
            {
                _changedProperties.Add(e.PropertyName);
            }
        }
        /// <summary>
        /// Event for when the <see cref="MainWindow"/> is closing
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_changedProperties != null && _changedProperties.Count > 0)
            {
                if (MessageBox.Show(this, "Do you want to save your changes before exiting?", "Save changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Event for when the <see cref="MainWindow"/> is loaded
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceController[] services = ServiceController.GetServices(Environment.MachineName);
            var service = services.FirstOrDefault(s => s.ServiceName == "Neis.FileCleanup.Service");
            if (service != null && service.Status == ServiceControllerStatus.Stopped)
            {
                if (MessageBox.Show(this,
                        "File Cleanup Service is currently not running.  Would you like to start the service?",
                        "Start service?",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    service.Start();
                    System.Threading.Thread.Sleep(1000);
                }
            }

            GetConfiguration();
            this.IsEnabled = true;
        }
    }
}