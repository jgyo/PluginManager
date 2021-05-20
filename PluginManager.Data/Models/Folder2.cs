using System;

namespace PluginManager.Data.Models
{
    public partial class Folder : IViewModelSetter<IFolderViewModel>
    {
        public void SetViewModel(IFolderViewModel model)
        {
            model.GetModel(this);
        }

        public void GetViewModel(IFolderViewModel model)
        {
            // FolderId = model.FolderId;
            FolderName = model.FolderName;
            PackageId = model.PackageId;
            IsHidden = model.IsHidden;
            InstallDate = model.InstallDate;
        }
    }

    public interface IFolderViewModel
    {
        long FolderId { get; set; }
        string FolderName { get; set; }
        long? PackageId { get; set; }
        bool IsHidden { get; set; }
        DateTime InstallDate { get; set; }

        void GetModel(Folder folder);
    }
}