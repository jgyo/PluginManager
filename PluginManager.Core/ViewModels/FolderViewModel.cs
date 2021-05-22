namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Windows.Input;
    using PluginManager.Core.Commands;
    using PluginManager.Data.Models;

    /// <summary>
    /// Defines the <see cref="FolderViewModel" />.
    /// </summary>
    public class FolderViewModel : ViewModel, IFolderViewModel
    {
        /// <summary>
        /// Defines the folderName.
        /// </summary>
        private string folderName;

        /// <summary>
        /// Defines the id.
        /// </summary>
        private long id;

        /// <summary>
        /// Defines the installedDate.
        /// </summary>
        private DateTime installedDate;

        /// <summary>
        /// Defines the isHidden.
        /// </summary>
        private bool isHidden;

        /// <summary>
        /// Defines the package.
        /// </summary>
        private ZipFileViewModel package;

        /// <summary>
        /// Defines the packageId.
        /// </summary>
        private long? packageId;

        /// <summary>
        /// Defines the DeleteFolderRequested.
        /// </summary>
        public event EventHandler DeleteFolderRequested;

        /// <summary>
        /// Defines the DoneEditingRequested.
        /// </summary>
        public event EventHandler DoneEditingRequested;

        /// <summary>
        /// Defines the HideFolderRequested.
        /// </summary>
        public event EventHandler HideFolderRequested;

        /// <summary>
        /// Defines the RestoreFolderRequested.
        /// </summary>
        public event EventHandler RestoreFolderRequested;

        /// <summary>
        /// Gets the ZipFileFolderCollection.
        /// </summary>
        public static IEnumerable<ZipFileViewModel> ZipFileFolderCollection => Locator.MainViewModel.ZipFileFolderCollection.OrderBy(m => m.Filename.ToLower());

        /// <summary>
        /// Gets a value indicating whether CanHide.
        /// </summary>
        public bool CanHide
        {
            get { return !IsHidden; }
        }

        /// <summary>
        /// Gets a value indicating whether CanRemovePackage.
        /// </summary>
        public bool CanRemovePackage
        {
            get { return Package != null; }
        }

        /// <summary>
        /// Gets a value indicating whether CanRestore.
        /// </summary>
        public bool CanRestore
        {
            get { return IsHidden; }
        }

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public ICommand DeleteCommand => new Command(Delete);

        /// <summary>
        /// Gets or sets a value indicating whether DeleteScheduled.
        /// </summary>
        public bool DeleteScheduled { get; set; }

        /// <summary>
        /// Gets the DoneEditingCommand.
        /// </summary>
        public ICommand DoneEditingCommand => new Command(DoneEditing);

        /// <summary>
        /// Gets or sets the FolderId.
        /// </summary>
        public long FolderId
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        /// <summary>
        /// Gets or sets the FolderName.
        /// </summary>
        public string FolderName
        {
            get { return folderName; }

            set { SetProperty(ref folderName, value); }
        }

        /// <summary>
        /// Gets the HideCommand.
        /// </summary>
        public ICommand HideCommand => new Command(Hide);

        /// <summary>
        /// Gets or sets the InstallDate.
        /// </summary>
        public DateTime InstallDate
        {
            get { return installedDate; }
            set { SetProperty(ref installedDate, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsHidden.
        /// </summary>
        public bool IsHidden
        {
            get { return isHidden; }
            set
            {
                SetProperty(ref isHidden, value);
                RaisePropertyChanged("CanHide");
                RaisePropertyChanged("CanRestore");
            }
        }

        /// <summary>
        /// Gets or sets the Package.
        /// </summary>
        public ZipFileViewModel Package
        {
            get { return package; }
            set
            {
                SetProperty(ref package, value);
                PackageId = package?.PackageId;
                RaisePropertyChanged("CanRemovePackage");
            }
        }

        /// <summary>
        /// Gets or sets the PackageId.
        /// </summary>
        public long? PackageId
        {
            get { return packageId; }
            set
            {
                var updatePackage = packageId != value;
                SetProperty(ref packageId, value);
                if (updatePackage)
                {
                    if (packageId == null)
                    {
                        Package = null;
                        return;
                    }

                    var thePackage = Locator.MainViewModel.ZipFileFolderCollection.Where(m => m.PackageId == packageId).SingleOrDefault();
                    Package = thePackage;
                }
            }
        }

        /// <summary>
        /// Gets the RemovePackageCommand.
        /// </summary>
        public ICommand RemovePackageCommand => new Command(RemovePackage);

        /// <summary>
        /// Gets the RestoreCommand.
        /// </summary>
        public ICommand RestoreCommand => new Command(Restore);

        /// <summary>
        /// The GetModel.
        /// </summary>
        /// <param name="folder">The folder<see cref="Folder"/>.</param>
        public void GetModel(Folder folder)
        {
            FolderId    = folder.FolderId;
            InstallDate = folder.InstallDate;
            FolderName  = folder.FolderName;
            IsHidden    = folder.IsHidden;
            PackageId   = folder.PackageId;

            Package = Locator.MainViewModel.ZipFileFolderCollection
                .Where(m => m.PackageId == PackageId)
                .SingleOrDefault();
        }

        /// <summary>
        /// The Delete.
        /// </summary>
        private void Delete()
        {
            DeleteFolderRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The DoneEditing.
        /// </summary>
        private void DoneEditing()
        {
            DoneEditingRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The Hide.
        /// </summary>
        private void Hide()
        {
            HideFolderRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The RemovePackage.
        /// </summary>
        private void RemovePackage()
        {
            Package = null;
        }

        /// <summary>
        /// The Restore.
        /// </summary>
        private void Restore()
        {
            RestoreFolderRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
