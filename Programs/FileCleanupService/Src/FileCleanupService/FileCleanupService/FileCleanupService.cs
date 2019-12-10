using Neis.FileCleanup.Configuration;
using Neis.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;

namespace Neis.FileCleanup.Service
{
    /// <summary>
    /// File cleanup service class
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class FileCleanupService : ServiceBase, IFileCleanupConfiguration
    {
        #region Static Members
        private static ServiceHost _serviceHost;
        private static LogWriter _logger;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            _logger = new LogWriter("File Cleanup Service");
            _logger.ShowRawText("File Cleanup Service started at " + DateTime.Now.ToString());

            // initialize the log file path
            string logFile = Path.GetFullPath(Properties.Settings.Default.LogFile);
            if (!Path.IsPathRooted(Properties.Settings.Default.LogFile))
            {
                logFile = Path.Combine(Path.GetDirectoryName(typeof(FileCleanupService).Assembly.Location), Properties.Settings.Default.LogFile);
            }
            
            // initialize the text logger
            TextFileLoggerSettings textLoggerSettings = new TextFileLoggerSettings()
            {
                LogLevel = LogMessageType.Verbose,
                ShowMessageTypePrefixes = true,
                TimeStampOnNone = false,
                TimeStampOnError = true,
                TimeStampOnInformation = true,
                TimeStampOnWarning = true,

                FilePath = logFile
            };
            _logger.AddLogger(new TextFileLogger(textLoggerSettings));
            _logger.ShowInformation("Log file set to " + logFile);

            // determine if service is being run in console mode
            bool consoleMode = false;

            string[] args = Environment.GetCommandLineArgs();
            if (args != null)
            {
                foreach (string arg in args)
                {
                    switch (arg.ToUpper())
                    {
                        case "-C":
                            consoleMode = true;
                            break;
                        case "-?":
                            ShowHelp();
                            return;
                    }
                }
            }

            // initialize console logger
            if (consoleMode)
            {
                // add console logger if running in console mode
                ConsoleLoggerSettings settings = new ConsoleLoggerSettings()
                {
                    LogLevel = LogMessageType.Verbose,
                    ShowMessageTypePrefixes = true,

                    DefaultBackgroundColor = Console.BackgroundColor,
                    DefaultForegroundColor = Console.ForegroundColor,
                    TimeStampOnNone = false,

                    InformationBackgroundColor = ConsoleColor.Black,
                    InformationForegroundColor = ConsoleColor.Yellow,
                    TimeStampOnInformation = true,

                    WarningBackgroundColor = ConsoleColor.Black,
                    WarningForegroundColor = ConsoleColor.Yellow,
                    TimeStampOnWarning = true,

                    ErrorBackgroundColor = ConsoleColor.Black,
                    ErrorForegroundColor = ConsoleColor.Red,
                    TimeStampOnError = true
                };
                _logger.AddLogger(new ConsoleLogger(settings));
            }

            // run in console mode
            if (consoleMode)
            {
                FileCleanupService svc = new FileCleanupService();
                svc.Debug_OnStart(args);

                Console.ReadKey();

                svc.Debug_OnStop();
            }
            else
            {
                ServiceBase.Run(new FileCleanupService());
            }
        }

        /// <summary>
        /// Shows the help contents to the console
        /// </summary>
        private static void ShowHelp()
        {
            ConsoleLoggerSettings settings = new ConsoleLoggerSettings()
            {
                LogLevel = LogMessageType.Verbose,
                DefaultBackgroundColor = Console.BackgroundColor,
                DefaultForegroundColor = Console.ForegroundColor,
                TimeStampOnNone = false,
            };
            _logger.AddLogger(new ConsoleLogger(settings));

            _logger.ShowRawText(Properties.Resources.HelpContents);
        }

        /// <summary>
        /// Event for when the service host for configuring the File Cleanup Service has opened
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private static void ServiceHost_Opened(object sender, EventArgs e)
        {
            foreach (Uri address in _serviceHost.BaseAddresses)
            {
                _logger.ShowInformation(string.Format("Endpoint opened at '{0}'", address));
            }
        }
        /// <summary>
        /// Event for when the service host for configuring the File Cleanup Service has faulted
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private static void ServiceHost_Faulted(object sender, EventArgs e)
        {
            _logger.ShowError("Error occured within the configuration service.");
        }
        #endregion

        private string _configFile;
        private FileCleanupConfiguration _config;
        private Timer _runTimer;
        private DateTime _nextCleanup;
        private string _status;

        /// <summary>
        /// Gets the status of this object
        /// </summary>
        public string Status
        {
            get { return _status; }
            private set
            {
                if (value != _status)
                {
                    _status = value;
                    _logger.ShowInformation(string.Format("Status changed to: {0}", _status));
                }
            }
        }

        /// <summary>
        /// Constructor for the <see cref="FileCleanupService"/> class
        /// </summary>
        public FileCleanupService()
        {
            new Task(() =>
            {
                // open configuration endpoint
                try
                {
                    if (_serviceHost == null)
                    {
                        _serviceHost = new ServiceHost(this);
                        _serviceHost.Faulted += ServiceHost_Faulted;
                        _serviceHost.Opened += ServiceHost_Opened;
                        _serviceHost.Open();
                    }
                }
                catch (Exception ex)
                {
                    _logger.ShowError("Error intializing the configuration endpoint.");
                    _logger.ShowError(ex.ToString());
                }
            }).Start();

            Status = Properties.Resources.Status_Initializing;

            InitializeComponent();

            _runTimer = new Timer();
            _runTimer.Elapsed += RunTimer_Elapsed;

            Status = Properties.Resources.Status_NotRunning;
        }

        /// <summary>
        /// Initializes the service
        /// </summary>
        private void Initialize()
        {
            Status = Properties.Resources.Status_LoadingConfigurations;

            try
            {
                if (!LoadConfiguration(Properties.Settings.Default.ConfigFile))
                {
                    _logger.ShowInformation("Creating new configuration file...");

                    _config = new FileCleanupConfiguration();
                    _config.ArchiveDays = 30;
                    _config.ArchiveLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "archive");
                    _config.CleanupTime = "2200";
                    _config.Directories.Add(new DirectoryConfiguration()
                    {
                        CleanupAction = CleanupAction.Archive,
                        Path = Path.GetTempPath(),
                        Recursive = true
                    });

                    _configFile = string.IsNullOrWhiteSpace(Properties.Settings.Default.ConfigFile) ?
                                        Path.Combine(Path.GetTempPath(), "fileCleanupConfig.xml") :
                                        Properties.Settings.Default.ConfigFile;

                    SaveConfiguration();
                }
            }
            catch (Exception ex)
            {
                _logger.ShowError(ex.ToString());
            }
        }

        /// <summary>
        /// Parses the time string into hours and minutes
        /// </summary>
        /// <param name="timeString">Time string to parse</param>
        /// <param name="hours">Hours from time string</param>
        /// <param name="minutes">Minutes from time string</param>
        /// <returns>True if successful.  False if not.</returns>
        private bool GetHoursAndMinutes(string timeString, out int hours, out int minutes)
        {
            int timeVal = -1;
            if (int.TryParse(timeString, out timeVal))
            {
                int hour = timeVal / 100;
                int minute = timeVal - (hour * 100);

                if (hour < 24 && hour >= 0 && minute < 60 && minute >= 0)
                {
                    hours = hour;
                    minutes = minute;
                    return true;
                }
            }

            hours = 0;
            minutes = 0;
            return false;
        }
        /// <summary>
        /// Loads the configuration
        /// </summary>
        /// <param name="file">Path to configuration file</param>
        /// <returns>True if successful.  False if not.</returns>
        private bool LoadConfiguration(string file)
        {
            try
            {
                if (!File.Exists(file))
                {
                    _logger.ShowWarning(string.Format("Configuration file does not exist at '{0}'", file));
                }
                else
                {
                    _config = FileCleanupConfiguration.Load(file);
                    if (_config != null)
                    {
                        _configFile = file;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.ShowError("Error while loading configuration file.");
                _logger.ShowError(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// Saves the current configuration
        /// </summary>
        private void SaveConfiguration()
        {
            string oldStatus = Status;
            Status = Properties.Resources.Status_SavingConfigurations;

            try
            {
                _config.Save(_configFile);
            }
            catch (Exception ex)
            {
                _logger.ShowError("Error while saving configuration");
                _logger.ShowError(ex.ToString());
            }
            Status = oldStatus;
        }
        /// <summary>
        /// Calculates the next cleanup time
        /// </summary>
        private void CalculateNextCleanup()
        {
            if (_config != null)
            {
                int hour = -1;
                int minute = -1;

                if (GetHoursAndMinutes(_config.CleanupTime, out hour, out minute))
                {
                    _nextCleanup = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, hour, minute, 0);

                    if (_nextCleanup <= DateTime.Now)
                    {
                        _nextCleanup = _nextCleanup.AddDays(1);
                    }
                    _logger.ShowInformation(string.Format("Next log time scheduled for {0}", _nextCleanup));
                }
                else
                {
                    _logger.ShowWarning("Failed calculating next cleanup time for value " + _config.CleanupTime);
                }
            }
            else
            {
                _logger.ShowWarning("Failed calculating next cleanup time because configuration is null.");
            }
        }
        /// <summary>
        /// Executes the cleanup
        /// </summary>
        private void ExecuteCleanup()
        {
            // run cleanup in the background
            Task cleanupTask = new Task(() =>
            {
                // store the current state of the run timer so we can restore it later
                bool timerEnabled = _runTimer.Enabled;

                try
                {
                    // pause the timer
                    _runTimer.Enabled = false;

                    List<DirectoryConfiguration> directories;
                    string archiveLocation;
                    int archiveDays = -1;

                    lock (_config)
                    {
                        // store a copy of the configurations so we do not experience possible concurrency issues
                        directories = new List<DirectoryConfiguration>(_config.Directories);
                        archiveDays = _config.ArchiveDays;
                        archiveLocation = _config.ArchiveLocation;
                    }

                    // scan directories
                    List<string> archivePossibilities = new List<string>();
                    List<string> deletePossibilities = new List<string>();
                    for (int i = 0; i < directories.Count; i++)
                    {
                        Status = string.Format(Properties.Resources.Status_Scanning, i + 1, directories.Count);

                        DirectoryConfiguration dirConfig = directories[i];
                        string[] files = Directory.GetFiles(dirConfig.Path,
                                         "*.*",
                                         dirConfig.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                        switch (dirConfig.CleanupAction)
                        {
                            case CleanupAction.Archive:
                                archivePossibilities.AddRange(files);
                                break;

                            case CleanupAction.Delete:
                                deletePossibilities.AddRange(files);
                                break;
                        }
                    }

                    // check files
                    List<string> filesToArchive = new List<string>();
                    List<string> filesToDelete = new List<string>();
                    DateTime oldDate = DateTime.Today.AddDays(-_config.ArchiveDays);

                    _logger.ShowInformation(string.Format("Looking for files older than {0}", oldDate));

                    int totalFiles = archivePossibilities.Count + deletePossibilities.Count;
                    for (int i = 0; i < archivePossibilities.Count; i++)
                    {
                        Status = string.Format(Properties.Resources.Status_CheckingFiles, i + 1, totalFiles);
                        if (File.GetLastWriteTime(archivePossibilities[i]) < oldDate)
                        {
                            filesToArchive.Add(archivePossibilities[i]);
                        }
                    }
                    for (int i = 0; i < deletePossibilities.Count; i++)
                    {
                        Status = string.Format(Properties.Resources.Status_CheckingFiles, archivePossibilities.Count + i + 1, totalFiles);
                        if (File.GetLastAccessTime(deletePossibilities[i]) < oldDate)
                        {
                            filesToDelete.Add(deletePossibilities[i]);
                        }
                    }

                    // perform actions
                    ArchiveFiles(filesToArchive);
                    DeleteFiles(filesToDelete);

                    if (_config.RemoveEmptyFolders)
                    {
                        // find all empty folders
                        List<string> emptyFolders = new List<string>();
                        Status = Properties.Resources.Status_SearchForEmptyFolders;
                        directories.ForEach(dirConfig =>
                        {
                            string[] folders = Directory.GetDirectories(dirConfig.Path,
                                                    "*.*",
                                                    dirConfig.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                            foreach (string f in folders)
                            {
                                string[] files = Directory.GetFiles(f);
                                if (files == null || files.Length == 0)
                                    emptyFolders.Add(f);
                            }
                        });


                        // Sort and reverse the list of empty folders so that we do not have any errors when removing
                        emptyFolders.Sort();
                        emptyFolders.Reverse();

                        // remove all empty folders
                        for (int i = 0; i < emptyFolders.Count; i++)
                        {
                            Status = string.Format(Properties.Resources.Status_RemovingEmptyFolders, i, emptyFolders.Count, emptyFolders[i]);
                            Directory.Delete(emptyFolders[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.ShowError("Exception while cleaning");
                    _logger.ShowError(ex.ToString());
                }
                finally
                {
                    _runTimer.Enabled = true;
                    CalculateNextCleanup();
                    Status = Properties.Resources.Status_Ready;
                }
            });
            cleanupTask.Start();
        }
        /// <summary>
        /// Archives a list of files
        /// </summary>
        /// <param name="files">Files to archive</param>
        private void ArchiveFiles(List<string> files)
        {
            if (files.Count > 0)
            {
                string destinationFolder = Path.Combine(_config.ArchiveLocation, DateTime.Now.ToString("yyyy-MM-dd.HHmm"));

                // create archive location 
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    Status = string.Format(Properties.Resources.Status_Archiving, i + 1, files.Count, destinationFolder);
                    string file = files[i];
                    try
                    {
                        File.Move(file, Path.Combine(destinationFolder, Path.GetFileName(file)));
                    }
                    catch (Exception ex)
                    {
                        _logger.ShowError(string.Format("Exception while archiving file located at '{0}'", files[i]));
                        _logger.ShowError(ex.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// Deletes a list of files
        /// </summary>
        /// <param name="files">Files to delete</param>
        private void DeleteFiles(List<string> files)
        {
            for (int i = 0; i < files.Count; i++)
            {
                Status = string.Format(Properties.Resources.Status_Deleting, i + 1, files.Count);
                try
                {
                    File.Delete(files[i]);
                }
                catch (Exception ex)
                {
                    _logger.ShowError(string.Format("Exception while deleting file located at '{0}'", files[i]));
                    _logger.ShowError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Event for when the run timer elapses
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private void RunTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now > _nextCleanup)
            {
                ExecuteCleanup();
            }
        }

        #region Debug Service Methods
        /// <summary>
        /// Programmatically executes the <see cref="OnStart"/> method
        /// </summary>
        /// <param name="args">Arguments used for execution</param>
        internal void Debug_OnStart(string[] args)
        {
            OnStart(args);
        }
        /// <summary>
        /// Programmatically executes the <see cref="OnStop"/> method
        /// </summary>
        internal void Debug_OnStop()
        {
            OnStop();
        }
        #endregion

        #region Service Methods
        /// <summary>
        /// Occurs when the service is started
        /// </summary>
        /// <param name="args">Arguments for starting the service</param>
        protected override void OnStart(string[] args)
        {
            try
            {
                Status = Properties.Resources.Status_Starting;

                Initialize();

                CalculateNextCleanup();

                new Task(() =>
                {
                    System.Threading.Thread.Sleep((60 - DateTime.Now.Second) * 1000);
                    _runTimer.Enabled = true;
                    Status = Properties.Resources.Status_Ready;
                }).Start();
            }
            catch(Exception ex)
            {
                _logger.ShowError("Exception while starting");
                _logger.ShowError(ex.ToString());
            }
        }
        /// <summary>
        /// Occurs when the service is stopped
        /// </summary>
        protected override void OnStop()
        {
            Status = Properties.Resources.Status_Stopping;

            _runTimer.Enabled = false;

            Status = Properties.Resources.Status_NotRunning;
        }
        #endregion

        #region IFileCleanupConfiguration Members
        /// <summary>
        /// Gets the current status of the service
        /// </summary>
        /// <returns>Current status</returns>
        public string GetStatus()
        {
            return Status;
        }
        /// <summary>
        /// Gets the current configuration
        /// </summary>
        /// <returns>Current configuration</returns>
        public FileCleanupConfiguration GetConfiguration()
        {
            return _config;
        }
        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="config">Configuration to set</param>
        public void SetConfiguration(FileCleanupConfiguration config)
        {
            _logger.ShowInformation("Setting new configuration");

            lock (_config)
            {
                _config = config;
                SaveConfiguration();
            }
        }
        /// <summary>
        /// Adds a directory to the cleanup congfiguration
        /// </summary>
        /// <param name="path">File path for the directory</param>
        /// <param name="isRecursive">True for recursive cleanup.  False for top level only</param>
        /// <param name="action"><see cref="CleanupAction"/> type</param>
        public void AddDirectory(string path, bool isRecursive, CleanupAction action)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (_config != null)
                {
                    _logger.ShowInformation(string.Format("Adding directory: '{0}' Recursive: {1} Action: {2}", path, isRecursive, action));

                    lock (_config)
                    {
                        DirectoryConfiguration dir = null;
                        foreach (DirectoryConfiguration d in _config.Directories)
                        {
                            if (string.Compare(path.TrimEnd('\\'), d.Path.TrimEnd('\\'), true) == 0)
                            {
                                dir = d;
                                break;
                            }
                        }

                        if (dir == null)
                        {
                            dir = new DirectoryConfiguration();
                        }
                        else
                        {
                            _config.Directories.Remove(dir);
                        }

                        dir.Path = path;
                        dir.CleanupAction = action;
                        dir.Recursive = isRecursive;
                        _config.Directories.Add(dir);

                        SaveConfiguration();
                    }
                }
            }
        }
        /// <summary>
        /// Removes  a directory from the cleanup configuration
        /// </summary>
        /// <param name="path">File path of the directory to remove</param>
        public void RemoveDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (_config != null)
                {
                    _logger.ShowInformation(string.Format("Removing directory: '{0}'", path));

                    lock (_config)
                    {
                        DirectoryConfiguration dir = null;
                        foreach (DirectoryConfiguration d in _config.Directories)
                        {
                            if (string.Compare(path.TrimEnd('\\'), d.Path.TrimEnd('\\'), true) == 0)
                            {
                                dir = d;
                                break;
                            }
                        }

                        if (dir != null)
                        {
                            _config.Directories.Remove(dir);
                            SaveConfiguration();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Sets the age of the archive allowed
        /// </summary>
        /// <param name="age">Number of days</param>
        public void SetArchiveDays(int age)
        {
            if (_config != null && _config.ArchiveDays != age && age > 0)
            {
                _logger.ShowInformation(string.Format("Setting archive days: '{0}'", age));

                lock (_config)
                {
                    _config.ArchiveDays = age;
                    SaveConfiguration();
                }
            }
        }
        /// <summary>
        /// Sets the location for archived items to be placed
        /// </summary>
        /// <param name="path">Path for the archive</param>
        public void SetArchiveLocation(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (_config != null && _config.ArchiveLocation != path)
                {
                    _logger.ShowInformation(string.Format("Setting archive location: '{0}'", path));
                    lock (_config)
                    {
                        _config.ArchiveLocation = path;
                        SaveConfiguration();
                    }
                }
            }
        }
        /// <summary>
        /// Sets the time of day to execute the cleanup
        /// </summary>
        /// <param name="timeString">Time of day</param>
        public void SetCleanupTime(string timeString)
        {
            if (!string.IsNullOrWhiteSpace(timeString))
            {
                int hours = -1;
                int minutes = -1;

                _logger.ShowInformation(string.Format("Setting cleanup time: '{0}'", timeString));
                if (_config != null && _config.CleanupTime != timeString && GetHoursAndMinutes(timeString, out hours, out minutes))
                {
                    lock (_config)
                    {
                        _config.CleanupTime = timeString;
                        CalculateNextCleanup();
                        SaveConfiguration();
                    }
                }
            }
        }
        /// <summary>
        /// Sets whether or not to remove empty folders during cleanup
        /// </summary>
        /// <param name="remove">True to remove.  False to leave.</param>
        public void SetRemoveEmptyFolders(bool remove)
        {
            if (_config != null && _config.RemoveEmptyFolders != remove)
            {
                _logger.ShowInformation(string.Format("Setting remove empty folders: '{0}'", remove));
                lock (_config)
                {
                    _config.RemoveEmptyFolders = remove;
                    SaveConfiguration();
                }
            }
        }
        #endregion
    }
}