namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.ObjectModel;
    using global::System.Collections.Specialized;
    using global::System.Diagnostics;
    using global::System.IO;
    using global::System.Windows.Input;
    using PluginManager.Core.Commands;
    using PluginManager.Core.EventHandlers;

    /// <summary>
    /// Defines the <see cref="MainViewModel" />.
    /// </summary>
    public class MainViewModel : ViewModel
    {
        /// <summary>
        /// Defines the selectedZipFile.
        /// </summary>
        private ZipFileViewModel selectedZipFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            SelectedFoldersCollection.CollectionChanged += SelectedFoldersCollection_CollectionChanged;
            SelectedZipFilesCollection.CollectionChanged += SelectedZipFilesCollection_CollectionChanged;

            OpenSetupCommand = new Command(OpenSetup);

            DeleteSelectedFoldersCommand = new Command(DeleteSelectedFolders);
            EditSelectedFolderCommand = new Command(EditSelectedFolder);
            HideSelectedFoldersCommand = new Command(HideSelectedFolders);
            RestoreSelectedFoldersCommand = new Command(RestoreSelectedFolders);
            SynchronizeDataBaseCommand = new Command(SynchronizeDataBase);

            OpenZipArchiveCommand = new Command(OpenZipArchive);
            DeleteSelectedZipFileCommand = new Command(DeleteSelectedZipFiles);
            EditSelectedZipFileCommand = new Command(EditSelectedZipFile);
            AddNewZipFileCommand = new Command(AddNewZipFile);
        }

        /// <summary>
        /// Defines the AddNewZipFileRequested event.
        /// </summary>
        public event EventHandler AddNewZipFileRequested;

        /// <summary>
        /// Defines the DeleteSelectedItemsRequested event.
        /// </summary>
        public event EventHandler DeleteSelectedFoldersRequested;

        public event EventHandler DeleteSelectedZipFilesRequested;

        /// <summary>
        /// Defines the EditSelectedFolderRequested event.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> EditSelectedFolderRequested;

        /// <summary>
        /// Defines the EditSelectedZipFileRequested.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> EditSelectedZipFileRequested;

        /// <summary>
        /// Defines the HideSelectedItemsRequested event.
        /// </summary>
        public event EventHandler HideSelectedFoldersRequested;

        /// <summary>
        /// Defines the OpenSetupRequested event.
        /// </summary>
        public event EventHandler<ViewModelEventArgs> OpenSetupRequested;

        /// <summary>
        /// Defines the RestoreSelectedItemsRequested event.
        /// </summary>
        public event EventHandler RestoreSelectedItemsRequested;

        public event EventHandler OpenZipArchiveRequested;

        /// <summary>
        /// Defines the SynchronizeDataBaseRequested event.
        /// </summary>
        public event EventHandler SynchronizeDataBaseRequested;

        public ICommand AddNewZipFileCommand { get; }

        public SetupViewModel SetupViewModel => Locator.SetupViewModel;

        /// <summary>
        /// Gets a value indicating whether folders are selected.
        /// </summary>
        public bool AreFoldersSelected
        {
            get { return SelectedFoldersCollection?.Count > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether zip files are selected.
        /// </summary>
        public bool AreZipFilesSelected
        {
            get { return SelectedZipFilesCollection?.Count > 0; }
        }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Needed for binding in xaml.")]
        public bool CanSynchronizeDataBase
        {
            get
            {
                var setup = Locator.SetupViewModel;
                var community = setup.CommunityFolder;
                var hidden = setup.HiddenFilesFolder;

                return !string.IsNullOrEmpty(community) &&
                    !string.IsNullOrEmpty(hidden) &&
                    Directory.Exists(community) &&
                    Directory.Exists(hidden);
            }
        }

        /// <summary>
        /// Gets the DeleteSelectedFoldersCommand.
        /// </summary>
        public ICommand DeleteSelectedFoldersCommand { get; }

        public ICommand DeleteSelectedZipFileCommand { get; }

        /// <summary>
        /// Gets the EditSelectedFolderCommand.
        /// </summary>
        public ICommand EditSelectedFolderCommand { get; }

        /// <summary>
        /// Gets the EditSelectedZipFileCommand.
        /// </summary>
        public ICommand EditSelectedZipFileCommand { get; }

        /// <summary>
        /// Gets the FolderCollection.
        /// </summary>
        public ObservableCollection<FolderViewModel> FolderCollection { get; } = new Lazy<ObservableCollection<FolderViewModel>>(() => new ObservableCollection<FolderViewModel>()).Value;

        /// <summary>
        /// Gets the HideSelectedFoldersCommand.
        /// </summary>
        public ICommand HideSelectedFoldersCommand { get; }

        public ICommand OpenZipArchiveCommand { get; }

        /// <summary>
        /// Gets a value indicating whether on folder is selected.
        /// </summary>
        public bool IsOneFolderSelected
        {
            get { return SelectedFoldersCollection?.Count == 1; }
        }

        /// <summary>
        /// Gets a value indicating whether one zip file is selected.
        /// </summary>
        public bool IsOneZipFileSelected
        {
            get { return SelectedZipFilesCollection?.Count == 1; }
        }

        /// <summary>
        /// Gets the OpenSetupCommand.
        /// </summary>
        public ICommand OpenSetupCommand { get; }

        /// <summary>
        /// Gets the RestoreSelectedFoldersCommand.
        /// </summary>
        public ICommand RestoreSelectedFoldersCommand { get; }

        /// <summary>
        /// Gets the SelectedFoldersCollection.
        /// </summary>
        public ObservableCollection<FolderViewModel> SelectedFoldersCollection { get; } = new Lazy<ObservableCollection<FolderViewModel>>(() => new ObservableCollection<FolderViewModel>()).Value;

        /// <summary>
        /// Gets the SelectedZipFile.
        /// </summary>
        public ZipFileViewModel SelectedZipFile { get => selectedZipFile; private set => SetProperty(ref selectedZipFile, value); }

        /// <summary>
        /// Gets the SelectedZipFilesCollection.
        /// </summary>
        public ObservableCollection<ZipFileViewModel> SelectedZipFilesCollection { get; } = new Lazy<ObservableCollection<ZipFileViewModel>>(() => new ObservableCollection<ZipFileViewModel>()).Value;

        /// <summary>
        /// Gets the SynchronizeDataBaseCommand.
        /// </summary>
        public ICommand SynchronizeDataBaseCommand { get; }

        /// <summary>
        /// Gets the ZipFileFolderCollection.
        /// </summary>
        public ObservableCollection<ZipFileViewModel> ZipFileFolderCollection { get; } = new Lazy<ObservableCollection<ZipFileViewModel>>(() => new ObservableCollection<ZipFileViewModel>()).Value;

        /// <summary>
        /// Closes the folder view model.
        /// </summary>
        /// <param name="pfvm">The pfvm<see cref="FolderViewModel"/>.</param>
        public void FolderViewModelClosed(FolderViewModel pfvm) // , MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            // This event occurs when a FolderViewModel closes. The
            // logic checks to see if it has been scheduled for deletion.
            // If so, the view model is deleted from collection and the
            // database.

            if (pfvm?.DeleteScheduled == true)
            {
                FolderCollection.Remove(pfvm);
                DbCore.Delete(pfvm);
            }
        }

        public void ZipFileViewModelClosed(ZipFileViewModel zfvm)
        {
            // This event occurs when a FolderViewModel closes. The
            // logic checks to see if it has been scheduled for deletion.
            // If so, the view model is deleted from collection and the
            // database.

            if (zfvm?.DeleteScheduled == true)
            {
                ZipFileFolderCollection.Remove(zfvm);
                DbCore.Delete(zfvm);
            }
        }

        /// <summary>
        /// Adds a new zip file to the zip files collection.
        /// </summary>
        private void AddNewZipFile()
        {
            AddNewZipFileRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Deletes the selected folders from the Hidden or Community
        /// folders and the Folders database.
        /// </summary>
        private void DeleteSelectedFolders()
        {
            DeleteSelectedFoldersRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Deletes one or more zip files and/or records.
        /// </summary>
        private void DeleteSelectedZipFiles()
        {
            DeleteSelectedZipFilesRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Edits the selected folder
        /// </summary>
        private void EditSelectedFolder()
        {
            var vm = SelectedFoldersCollection[0];
            Debug.Assert(vm != null);

            EditSelectedFolderRequested?.Invoke(this, new ViewModelEventArgs(vm));
        }

        /// <summary>
        /// Edits the selected zip file.
        /// </summary>
        private void EditSelectedZipFile()
        {
            var vm = SelectedZipFile;
            Debug.Assert(vm != null);

            EditSelectedZipFileRequested?.Invoke(this, new ViewModelEventArgs(vm));
        }

        /// <summary>
        /// Moves selected folders to the Hidden folder.
        /// </summary>
        private void HideSelectedFolders()
        {
            HideSelectedFoldersRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OpenZipArchive()
        {
            OpenZipArchiveRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Opens the setup window.
        /// </summary>
        private void OpenSetup()
        {
            var suvm = Locator.SetupViewModel;
            OpenSetupRequested?.Invoke(this, new ViewModelEventArgs(suvm));
            RaisePropertyChanged("CanSynchronizeDataBase");
        }

        /// <summary>
        /// Moves selected folders to the Community folder.
        /// </summary>
        private void RestoreSelectedFolders()
        {
            RestoreSelectedItemsRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The SelectedFoldersCollection_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="NotifyCollectionChangedEventArgs"/>.</param>
        private void SelectedFoldersCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("AreFoldersSelected");
            RaisePropertyChanged("IsOneFolderSelected");
        }

        /// <summary>
        /// The SelectedZipFilesCollection_CollectionChanged.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="NotifyCollectionChangedEventArgs"/>.</param>
        private void SelectedZipFilesCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("AreZipFilesSelected");
            RaisePropertyChanged("IsOneZipFileSelected");

            if (IsOneZipFileSelected)
            {
                SelectedZipFile = SelectedZipFilesCollection[0];
            }
            else
            {
                SelectedZipFile = null;
            }
        }

        /// <summary>
        /// Synchronizes the database with existing folders
        /// </summary>
        private void SynchronizeDataBase()
        {
            SynchronizeDataBaseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}