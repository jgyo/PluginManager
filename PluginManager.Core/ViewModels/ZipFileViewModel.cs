namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;
    using PluginManager.Data.Models;

    /// <summary>
    /// Defines the <see cref="ZipFileViewModel" />.
    /// </summary>
    public class ZipFileViewModel : ViewModel, IZipFileViewModel
    {
        /// <summary>
        /// Defines the addedDate.
        /// </summary>
        private DateTime addedDate;

        /// <summary>
        /// Defines the addonName.
        /// </summary>
        private string addonName;

        /// <summary>
        /// Defines the fileDate.
        /// </summary>
        private DateTime fileDate;

        /// <summary>
        /// Defines the filename.
        /// </summary>
        private string filename;

        /// <summary>
        /// Defines the filePath.
        /// </summary>
        private string filePath;

        /// <summary>
        /// Defines the fileSize.
        /// </summary>
        private long fileSize;

        /// <summary>
        /// Defines the packageId.
        /// </summary>
        private long packageId;

        /// <summary>
        /// Defines the version.
        /// </summary>
        private string version;

        /// <summary>
        /// Gets or sets the AddedDate.
        /// </summary>
        public DateTime AddedDate
        {
            get => addedDate;
            set
            {
                SetProperty(ref addedDate, value);
            }
        }

        // 100 Characters
        /// <summary>
        /// Gets or sets the AddonName.
        /// </summary>
        public string AddonName
        {
            get => addonName;
            set
            {
                SetProperty(ref addonName, value);
            }
        }

        /// <summary>
        /// Gets or sets the FileDate.
        /// </summary>
        public DateTime FileDate
        {
            get => fileDate;
            set
            {
                SetProperty(ref fileDate, value);
            }
        }

        // 100 Characters
        /// <summary>
        /// Gets or sets the Filename.
        /// </summary>
        public string Filename
        {
            get => filename;
            set
            {
                SetProperty(ref filename, value);
            }
        }

        // 256 characters
        /// <summary>
        /// Gets or sets the FilePath.
        /// </summary>
        public string FilePath
        {
            get => filePath;
            set
            {
                SetProperty(ref filePath, value);
            }
        }

        /// <summary>
        /// Gets or sets the FileSize.
        /// </summary>
        public long FileSize
        {
            get => fileSize;
            set
            {
                SetProperty(ref fileSize, value);
            }
        }

        /// <summary>
        /// Gets the Folders.
        /// </summary>
        public IEnumerable<IFolderViewModel> Folders
        {
            get
            {
                foreach (var folder in Locator.MainViewModel.FolderCollection)
                {
                    if (folder.PackageId == PackageId)
                    {
                        yield return folder;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the PackageId.
        /// </summary>
        public long PackageId
        {
            get { return packageId; }
            set
            {
                SetProperty(ref packageId, value);
            }
        }

        // 32 Characters
        /// <summary>
        /// Gets or sets the Version.
        /// </summary>
        public string Version
        {
            get => version;
            set
            {
                SetProperty(ref version, value);
            }
        }

        /// <summary>
        /// The GetModel.
        /// </summary>
        /// <param name="zipFile">The zipFile<see cref="ZipFile"/>.</param>
        public void GetModel(ZipFile zipFile)
        {
            PackageId = zipFile.PackageId;
            AddedDate = zipFile.AddedDate;
            FilePath = zipFile.FilePath;
            Filename = zipFile.Filename;
            FileDate = zipFile.FileDate;
            FileSize = zipFile.FileSize;
            AddonName = zipFile.AddonName;
            Version = zipFile.Version;
        }
    }
}
