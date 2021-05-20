namespace PluginManager.Core.ViewModels
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.ComponentModel;
    using global::System.IO;
    using global::System.Linq;
    using global::System.Runtime.CompilerServices;
    using global::System.Windows.Input;
    using PluginManager.Core.Commands;
    using PluginManager.Core.EventHandlers;
    using PluginManager.Core.Logging;

    public class SetupViewModel : ViewModel, IDataErrorInfo
    {
        private bool canAcceptChanges = false;
        private string communityFolder;
        private string error = string.Empty;
        private Dictionary<string, string> errors;
        private string hiddenFilesFolder;
        private bool loggingEnabled;
        private LogLevel loggingLevel;
        private string zipFilesFolder;

        public event EventHandler<ViewModelEventArgs> AcceptChangesRequested;
        public event EventHandler<BrowserEventArgs> BrowseForFolderRequested;
        public ICommand AcceptChangesCommand => new Command(AcceptChanges);
        public ICommand BrowseForCommunityFolderCommand => new Command(BrowseForCommunityFolder);
        public ICommand BrowseForHiddenFolderCommand => new Command(BrowseForHiddenFolder);
        public ICommand BrowseForZipFilesFolderCommand => new Command(BrowseForCZipFilesFolder);

        public bool CanAcceptChanges
        {
            get { return canAcceptChanges; }
            set { SetProperty(ref canAcceptChanges, value); }
        }

        public string CommunityFolder
        {
            get { return communityFolder; }
            set
            {
                var path = value ?? "";
                if (path == string.Empty)
                {
                    SetErrors("The community folder path is required");
                }
                else if (path.Length > 256)
                {
                    SetErrors("The community folder length > 256");
                }
                else
                {
                    try
                    {
                        path = Path.GetFullPath(path);
                    }
                    catch (Exception e)
                    {
                        SetErrors(e.Message);
                        SetProperty(ref communityFolder, value);
                        return;
                    }

                    if (path.Length > 256)
                    {
                        SetErrors("The community folder length > 256");
                    }
                    else if (Directory.Exists(path))
                        if (Path.GetPathRoot(path) == Path.GetPathRoot(HiddenFilesFolder))
                            SetErrors();
                        else
                        {
                            SetErrors("The hidden and community folders must be on the same drive.", "HiddenFilesFolder");
                            RaisePropertyChanged("HiddenFilesFolder");
                        }
                    else
                        SetErrors("The given path does not exist");
                }

                SetProperty(ref communityFolder, path);
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <Returns>
        /// An error message indicating what is wrong with this object. The default is an
        /// empty string ("").
        /// </Returns>
        public string Error
        {
            get { return error; }
        }

        public string HiddenFilesFolder
        {
            get { return hiddenFilesFolder; }
            set
            {
                var path = value;
                if (path == string.Empty)
                {
                    SetErrors("The hidden files folder path is required");
                }
                else if (path.Length > 256)
                {
                    SetErrors("The hidden files folder length > 256");
                }
                else
                {
                    try
                    {
                        path = Path.GetFullPath(path);
                    }
                    catch (Exception e)
                    {
                        SetErrors(e.Message);
                        SetProperty(ref hiddenFilesFolder, value);
                        return;
                    }

                    if (path.Length > 256)
                    {
                        SetErrors("The hidden files folder length > 256");
                    }
                    else if (Directory.Exists(path))
                        if (Path.GetPathRoot(path) == Path.GetPathRoot(CommunityFolder))
                            SetErrors();
                        else
                            SetErrors("The hidden and community folders must be on the same drive.");
                    else
                        SetErrors("The given does not exist");
                }

                SetProperty(ref hiddenFilesFolder, path);
            }
        }

        public bool LoggingEnabled
        {
            get { return loggingEnabled; }
            set { SetProperty(ref loggingEnabled, value); }
        }

        public LogLevel LoggingLevel
        {
            get { return loggingLevel; }
            set { SetProperty(ref loggingLevel, value); }
        }

        public string ZipFilesFolder
        {
            get { return zipFilesFolder; }
            set
            {
                var path = value;
                if (path == string.Empty)
                {
                    SetErrors();
                    SetProperty(ref zipFilesFolder, path);
                    return;
                }

                if (path.Length > 256)
                {
                    SetErrors("The zip files folder.Length > 256");
                }
                else
                {
                    try
                    {
                        path = Path.GetFullPath(path);
                    }
                    catch (Exception e)
                    {
                        SetErrors(e.Message);
                        SetProperty(ref zipFilesFolder, value);
                        return;
                    }

                    if (path.Length > 256)
                    {
                        SetErrors("The zip files folder length > 256");
                    }
                    else if (Directory.Exists(path))
                        SetErrors();
                    else
                        SetErrors("The given path does not exist");
                }

                SetProperty(ref zipFilesFolder, path);
            }
        }

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="columnName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        public string this[string columnName]
        {
            get
            {
                if (errors == null || errors.ContainsKey(columnName) == false)
                    return string.Empty;

                return errors[columnName];
            }
        }

        private void AcceptChanges()
        {
            AcceptChangesRequested?.Invoke(this, new ViewModelEventArgs(this));
        }

        private void BrowseForCommunityFolder()
        {
            CommunityFolder = OnBrowseForFolderRequested(CommunityFolder, "Select the Community Folder");
        }

        private void BrowseForCZipFilesFolder()
        {
            ZipFilesFolder = OnBrowseForFolderRequested(ZipFilesFolder, "Select the Zip File Folder");
        }

        private void BrowseForHiddenFolder()
        {
            HiddenFilesFolder = OnBrowseForFolderRequested(HiddenFilesFolder, "Select the Hidden Files Folder");
        }

        private string OnBrowseForFolderRequested(string original, string description)
        {
            var e = new BrowserEventArgs() { Folder = original, Description = description };
            BrowseForFolderRequested?.Invoke(this, e);
            return e.Folder;
        }

        private void SetErrors(string value = "", [CallerMemberName] string columnName = null)
        {
            if (errors == null)
                errors = new Dictionary<string, string>();

            error = value;

            if (errors.ContainsKey(columnName))
            {
                errors[columnName] = value;
                CanAcceptChanges = errors.Values.All(m => m == string.Empty);
                return;
            }

            errors.Add(columnName, value);

            CanAcceptChanges = errors.Values.All(m => m == string.Empty);
        }
    }
}
