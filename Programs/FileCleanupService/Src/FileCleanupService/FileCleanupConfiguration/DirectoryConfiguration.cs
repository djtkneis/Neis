using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Neis.FileCleanup.Configuration
{
    /// <summary>
    /// Configuration for directory cleanup
    /// </summary>
    [DataContract]
    public class DirectoryConfiguration : NotifyableObject
    {
        /// <summary>
        /// Property name for the <see cref="CleanupAction"/> property
        /// </summary>
        public const string CleanupActionPropertyName = "CleanupAction";
        /// <summary>
        /// Property name for the <see cref="Recursive"/> property
        /// </summary>
        public const string RecursivePropertyName = "Recursive";
        /// <summary>
        /// Property name for the <see cref="Path"/> property
        /// </summary>
        public const string PathPropertyName = "Path";


        private string _path;
        private bool _recursive;
        private CleanupAction _cleanupAction;

        /// <summary>
        /// Path to directory
        /// </summary>
        [XmlAttribute("path")]
        [DataMember]
        public string Path
        {
            get { return _path; }
            set 
            {
                if (_path != value)
                {
                    _path = value;
                    NotifyPropertyChanged(PathPropertyName);
                }
            }
        }
        /// <summary>
        /// Whether or not to recursively clean up
        /// </summary>
        [XmlAttribute("recursive")]
        [DataMember]
        public bool Recursive
        {
            get { return _recursive; }
            set
            {
                if (_recursive != value)
                {
                    _recursive = value;
                    NotifyPropertyChanged(RecursivePropertyName);
                }
            }
        }
        /// <summary>
        /// Cleanup type
        /// </summary>
        [XmlAttribute("cleanupAction")]
        [DataMember]
        public CleanupAction CleanupAction
        {
            get { return _cleanupAction; }
            set
            {
                if (_cleanupAction != value)
                {
                    _cleanupAction = value;
                    NotifyPropertyChanged(CleanupActionPropertyName);
                }
            }
        }
    }
}