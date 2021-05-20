using System;
using System.IO;

namespace PluginManager.Data.Models
{
    public partial class ZipFile : IViewModelSetter<IZipFileViewModel>
    {
        public void SetViewModel(IZipFileViewModel model)
        {
            model.GetModel(this);
        }

        public void GetViewModel(IZipFileViewModel model)
        {
            // PackageId = model.PackageId;
            Filename = model.Filename;
            AddonName = model.AddonName;
            Version = model.Version;
            FileSize = model.FileSize;
            FileDate = model.FileDate;
            AddedDate = model.AddedDate;
            FilePath = model.FilePath;
        }

        public void GetFileInfo(FileInfo fileInfo)
        {
            Filename = fileInfo.Name;
            FileSize = fileInfo.Length;
            FileDate = fileInfo.CreationTime;
            AddedDate = DateTime.Now;
            FilePath = fileInfo.DirectoryName;
            Version = "na";
            AddonName = "na";
        }
    }

    public interface IViewModelSetter<T>
    {
        void GetViewModel(T model);

        void SetViewModel(T model);
    }

    public interface IZipFileViewModel
    {
        long PackageId { get; set; }
        string Filename { get; set; }
        string AddonName { get; set; }
        string Version { get; set; }
        long FileSize { get; set; }
        DateTime FileDate { get; set; }
        DateTime AddedDate { get; set; }
        string FilePath { get; set; }

        void GetModel(ZipFile zipFile);
    }
}