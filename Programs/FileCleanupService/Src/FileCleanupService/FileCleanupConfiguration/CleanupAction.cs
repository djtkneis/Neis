
namespace Neis.FileCleanup.Configuration
{
    /// <summary>
    /// Types of cleanup
    /// </summary>
    public enum CleanupAction
    {
        /// <summary>
        /// Archive the files into the specified archive location
        /// </summary>
        Archive,
        /// <summary>
        /// Delete the files from disk
        /// </summary>
        Delete
    }
}