namespace PluginManager.Core.System
{
    using global::System;
    using global::System.IO;
    using PluginManager.Core.Logging;

    /// <summary>
    /// Defines the <see cref="FileOps" class. />.
    /// </summary>
    public class FileOps
    {
        /// <summary>
        /// Initializes static members of the <see cref="FileOps"/> class.
        /// </summary>
        static FileOps()
        {
            LogProvider.Instance.GetLogFor<FileOps>().Debug("FileOps initialized.");
        }

        /// <summary>
        /// Deletes the specified folder from the hidden or community directory.
        /// </summary>
        /// <param name="folderName">The folderName<see cref="string"/>.</param>
        /// <param name="folderPath">The folderPath<see cref="string"/>.</param>
        public static void Delete(string folderName, string folderPath)
        {
            var path = Path.Combine(folderPath, folderName);
            var log = LogProvider.Instance.GetLogFor<FileOps>();

            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Delete(path, true);
                    log.Info($"{folderName} successfully deleted.");
                }
                catch (Exception e)
                {
                    log.InfoException($"Unable to delete {folderName}.", e);
                }
            }
            else
            {
                log.Info($"{folderName} does not exist in {folderPath}.");
            }
        }

        /// <summary>
        /// Moves the selected folder to the hidden directory.
        /// </summary>
        /// <param name="folderName">Name of the folder to hide.</param>
        /// <param name="communityFolder">The communityFolder<see cref="string"/>.</param>
        /// <param name="hiddenFolder">The hiddenFolder<see cref="string"/>.</param>
        /// <returns>True if the folder was hidden. Otherwise false.</returns>
        public static bool Hide(string folderName, string communityFolder, string hiddenFolder)
        {
            var log = LogProvider.Instance.GetLogFor<FileOps>();

            try
            {
                var fullname = Path.Combine(communityFolder, folderName);
                hiddenFolder = Path.Combine(hiddenFolder, folderName);
                Directory.Move(fullname, hiddenFolder);
            }
            catch (Exception e)
            {
                log.InfoException("Error Hiding Folder.", e);
                return false;
            }

            log.Info($"{folderName} successfully hidden.");
            return true;
        }

        /// <summary>
        /// Moves the specified folder to the community folder.
        /// </summary>
        /// <param name="folderName">The folderName<see cref="string"/>.</param>
        /// <param name="communityFolder">The communityFolder<see cref="string"/>.</param>
        /// <param name="hiddenFolder">The hiddenFolder<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool Restore(string folderName, string communityFolder, string hiddenFolder)
        {
            var log = LogProvider.Instance.GetLogFor<FileOps>();

            try
            {
                var fullname = Path.Combine(hiddenFolder, folderName);
                communityFolder = Path.Combine(communityFolder, folderName);
                Directory.Move(fullname, communityFolder);
            }
            catch (Exception e)
            {
                log.InfoException("Error Hiding Folder.", e);
                return false;
            }

            log.Info($"{folderName} successfully restored.");
            return true;
        }
    }
}