using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Neis.FileCleanup.Configuration
{
    /// <summary>
    /// Stores the configuration for the File Cleanup Service
    /// </summary>
    [XmlRoot]
    [XmlInclude(typeof(DirectoryConfiguration))]
    [DataContract]
    public class FileCleanupConfiguration : NotifyableObject
    {
        private static XmlSerializer _serializer = new XmlSerializer(typeof(FileCleanupConfiguration));

        /// <summary>
        /// Property name for the <see cref="CleanupTime"/> property
        /// </summary>
        public const string CleanupTimePropertyName = "CleanupTime";
        /// <summary>
        /// Property name for the <see cref="ArchiveDays"/> property
        /// </summary>
        public const string ArchiveDaysPropertyName = "ArchiveDays";
        /// <summary>
        /// Property name for the <see cref="ArchiveLocation"/> property
        /// </summary>
        public const string ArchiveLocationPropertyName = "ArchiveLocation";
        /// <summary>
        /// Property name for the <see cref="RemoveEmptyFolders"/> property
        /// </summary>
        public const string RemoveEmptyFoldersPropertyName = "RemoveEmptyFolders";
        /// <summary>
        /// Property name for the <see cref="Directories"/> property
        /// </summary>
        public const string DirectoriesPropertyName = "Directories";

        private string _cleanupTime;
        private int _archiveDays;
        private string _archiveLocation;
        private bool _removeEmptyFolders;
        private ObservableCollection<DirectoryConfiguration> _directories;

        /// <summary>
        /// Time that the daily cleanup will occur
        /// </summary>
        [XmlElement]
        [DataMember]
        public string CleanupTime
        {
            get { return _cleanupTime; }
            set
            {
                if (_cleanupTime != value)
                {
                    _cleanupTime = value;
                    NotifyPropertyChanged(CleanupTimePropertyName);
                }
            }
        }

        /// <summary>
        /// Oldest age files can be before archiving
        /// </summary>
        [XmlElement]
        [DataMember]
        public int ArchiveDays
        {
            get { return _archiveDays; }
            set
            {
                if (_archiveDays != value)
                {
                    _archiveDays = value;
                    NotifyPropertyChanged(ArchiveDaysPropertyName);
                }
            }
        }

        /// <summary>
        /// Location archives are moved to
        /// </summary>
        [XmlElement]
        [DataMember]
        public string ArchiveLocation
        {
            get { return _archiveLocation; }
            set
            {
                if (_archiveLocation != value)
                {
                    _archiveLocation = value;
                    NotifyPropertyChanged(ArchiveLocationPropertyName);
                }
            }
        }

        /// <summary>
        /// Whether or not to remove empty folders during cleanup
        /// </summary>
        [XmlElement]
        [DataMember]
        public bool RemoveEmptyFolders
        {
            get { return _removeEmptyFolders; }
            set
            {
                if (_removeEmptyFolders != value)
                {
                    _removeEmptyFolders = value;
                    NotifyPropertyChanged(RemoveEmptyFoldersPropertyName);
                }
            }
        }

        /// <summary>
        /// List of directory configurations
        /// </summary>
        [XmlArray]
        [DataMember]
        public ObservableCollection<DirectoryConfiguration> Directories
        {
            get { return _directories; }
            private set
            {
                if (_directories != value)
                {
                    if (_directories != null)
                    {
                        _directories.CollectionChanged -= Directories_CollectionChanged;
                    }

                    _directories = value;

                    if (_directories != null)
                    {
                        _directories.CollectionChanged += Directories_CollectionChanged;
                        foreach (DirectoryConfiguration c in _directories)
                        {
                            c.PropertyChanged += DirectoryConfig_PropertyChanged;
                        }
                    }

                    NotifyPropertyChanged(DirectoriesPropertyName);
                }
            }
        }

        /// <summary>
        /// Constructor for the <see cref="FileCleanupConfiguration"/> class
        /// </summary>
        public FileCleanupConfiguration()
        {
            _directories = new ObservableCollection<DirectoryConfiguration>();
            _directories.CollectionChanged += Directories_CollectionChanged;
        }

        /// <summary>
        /// Event for when the <see cref="Directories"/> collection has changed
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private void Directories_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Add:
                    foreach (object o in e.NewItems)
                    {
                        DirectoryConfiguration config = o as DirectoryConfiguration;
                        if (config != null)
                        {
                            config.PropertyChanged += DirectoryConfig_PropertyChanged;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (object o in e.OldItems)
                    {
                        DirectoryConfiguration config = o as DirectoryConfiguration;
                        if (config != null)
                        {
                            config.PropertyChanged -= DirectoryConfig_PropertyChanged;
                        }
                    }
                    break;
            }

            IsDirty = true;
        }
        /// <summary>
        /// Event for when a property has changed on a directory configuration
        /// </summary>
        /// <param name="sender">Sender of this event</param>
        /// <param name="e">Necessary arguments for this event</param>
        private void DirectoryConfig_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsDirty = true;
        }

        /// <summary>
        /// Saves the configuration
        /// </summary>
        /// <param name="file">File to save the configuration to</param>
        public void Save(string file)
        {
            Save(this, file);
        }
        /// <summary>
        /// Saves a configuration
        /// </summary>
        /// <param name="config">Configuration to save</param>
        /// <param name="file">File to save the configuration to</param>
        public static void Save(FileCleanupConfiguration config, string file)
        {
            using (FileStream output = File.Create(file))
            {
                _serializer.Serialize(output, config);
            }
        }
        /// <summary>
        /// Loads a configuraion from a given files
        /// </summary>
        /// <param name="file">File to load configuration from</param>
        /// <returns>Configuration loaded from file</returns>
        public static FileCleanupConfiguration Load(string file)
        {
            try
            {
                using (FileStream input = File.Open(file, FileMode.Open))
                {
                    return _serializer.Deserialize(input) as FileCleanupConfiguration;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}