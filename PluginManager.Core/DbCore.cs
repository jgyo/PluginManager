namespace PluginManager.Core
{
    using global::System.Collections.Generic;
    using global::System.Diagnostics;
    using PluginManager.Core.Logging;
    using PluginManager.Core.ViewModels;
    using PluginManager.Data.Models;

    /// <summary>
    /// Defines the <see cref="DbCore" />.
    /// </summary>
    public class DbCore
    {
        /// <summary>
        /// Initializes static members of the <see cref="DbCore"/> class.
        /// </summary>
        static DbCore()
        {
            var pvm = Locator.MainViewModel;

            using var dbc = new PmDb();
            foreach (var ent in dbc.ZipFiles)
            {
                var zfvm = new ZipFileViewModel();
                zfvm.GetModel(ent);
                pvm.ZipFileFolderCollection.Add(zfvm);
            }

            foreach (var ent in dbc.Folders)
            {
                var vm = new FolderViewModel();
                vm.GetModel(ent);
                pvm.FolderCollection.Add(vm);
            }
        }

        /// <summary>
        /// Adds a single folder to the database.
        /// </summary>
        /// <param name="vm">The vm<see cref="FolderViewModel"/>.</param>
        public static void Add(FolderViewModel vm)
        {
            Debug.Assert(vm != null);
            using var dbc = new PmDb();
            var ent = new Folder();
            ent.GetViewModel(vm);
            dbc.Folders.Add(ent);
            dbc.SaveChanges();
            vm.GetModel(ent);
        }

        /// <summary>
        /// Adds folders to the database.
        /// </summary>
        /// <param name="vms">The vms<see cref="IEnumerable{FolderViewModel}"/>.</param>
        public static void Add(IEnumerable<FolderViewModel> vms)
        {
            Debug.Assert(vms != null);
            using var dbc = new PmDb();

            var pis = new List<Folder>();
            var vmlist = new List<FolderViewModel>(vms);

            for (var i = 0; i < vmlist.Count; i++)
            {
                var vm = vmlist[i];
                var ent = new Folder();
                ent.GetViewModel(vm);
                pis.Add(ent);
            }

            dbc.Folders.AddRange(pis);
            dbc.SaveChanges();

            for (int i = 0; i < vmlist.Count; i++)
            {
                var vm = vmlist[i];
                var ent = pis[i];
                vm.GetModel(ent);
            }
        }

        /// <summary>
        /// Adds zip files to the database.
        /// </summary>
        /// <param name="vms">The vms<see cref="IEnumerable{ZipFileViewModel}"/>.</param>
        public static void Add(IEnumerable<ZipFileViewModel> vms)
        {
            Debug.Assert(vms != null);
            using var dbc = new PmDb();

            var pis = new List<ZipFile>();
            var vmlist = new List<ZipFileViewModel>(vms);

            for (var i = 0; i < vmlist.Count; i++)
            {
                var vm = vmlist[i];
                var ent = new ZipFile();
                ent.GetViewModel(vm);
                pis.Add(ent);
            }

            dbc.ZipFiles.AddRange(pis);
            dbc.SaveChanges();

            for (int i = 0; i < vmlist.Count; i++)
            {
                var vm = vmlist[i];
                var ent = pis[i];
                vm.GetModel(ent);
            }
        }

        /// <summary>
        /// Adds a single zip file to the database.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipFileViewModel"/>.</param>
        public static void Add(ZipFileViewModel vm)
        {
            Debug.Assert(vm != null);
            using var dbc = new PmDb();
            var ent = new ZipFile();
            ent.GetViewModel(vm);
            dbc.ZipFiles.Add(ent);
            dbc.SaveChanges();
            vm.GetModel(ent);
        }

        /// <summary>
        /// Deletes a folder from the database.
        /// </summary>
        /// <param name="vm">The vm<see cref="FolderViewModel"/>.</param>
        public static void Delete(FolderViewModel vm)
        {
            Debug.Assert(vm != null);
            Debug.Assert(vm.FolderId != 0);
            using var dbc = new PmDb();
            var ent = dbc.Find<Folder>(vm.FolderId);
            dbc.Folders.Remove(ent);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Deletes folders from the database.
        /// </summary>
        /// <param name="vms">The vms<see cref="IEnumerable{FolderViewModel}"/>.</param>
        public static void Delete(IEnumerable<FolderViewModel> vms)
        {
            Debug.Assert(vms != null);
            using var dbc = new PmDb();

            foreach (var vm in vms)
            {
                Debug.Assert(vm.FolderId != 0);
                var ent = dbc.Find<Folder>(vm.FolderId);
                dbc.Folders.Remove(ent);
            }

            dbc.SaveChanges();
        }

        /// <summary>
        /// Deletes zip files from the database.
        /// </summary>
        /// <param name="vms">The vms<see cref="IEnumerable{ZipFileViewModel}"/>.</param>
        public static void Delete(IEnumerable<ZipFileViewModel> vms)
        {
            Debug.Assert(vms != null);
            var dbc = new PmDb();

            foreach (var vm in vms)
            {
                Debug.Assert(vm.PackageId != 0);
                var ent = dbc.Find<ZipFile>(vm.PackageId);
                dbc.ZipFiles.Remove(ent);
            }

            dbc.SaveChanges();
        }

        /// <summary>
        /// Deletes a single zip file from the database.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipFileViewModel"/>.</param>
        public static void Delete(ZipFileViewModel vm)
        {
            Debug.Assert(vm != null);
            Debug.Assert(vm.PackageId != 0);
            using var dbc = new PmDb();
            var ent = dbc.Find<ZipFile>(vm.PackageId);
            dbc.ZipFiles.Remove(ent);
            dbc.SaveChanges();
        }

        /// <summary>
        /// Initializes a static instance of DbCore.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public static bool Initialize()
        {
            LogProvider.Instance.GetLogFor<DbCore>().Info("DbCore Initialized.");
            return true;
        }

        /// <summary>
        /// Updates a folder of the database.
        /// </summary>
        /// <param name="vm">The vm<see cref="FolderViewModel"/>.</param>
        public static void Update(FolderViewModel vm)
        {
            Debug.Assert(vm != null);
            Debug.Assert(vm.FolderId != 0);
            using var dbc = new PmDb();
            var ent = dbc.Find<Folder>(vm.FolderId);
            ent.GetViewModel(vm);
            dbc.SaveChanges();
        }

        /// <summary>
        /// updates folders of the database.
        /// </summary>
        /// <param name="vms">The vms<see cref="IEnumerable{FolderViewModel}"/>.</param>
        public static void Update(IEnumerable<FolderViewModel> vms)
        {
            Debug.Assert(vms != null);
            using var dbc = new PmDb();

            foreach (var vm in vms)
            {
                Debug.Assert(vm.FolderId != 0);
                var ent = dbc.Find<Folder>(vm.FolderId);
                ent.GetViewModel(vm);
            }

            dbc.SaveChanges();
        }

        /// <summary>
        /// Updates zip files of the database.
        /// </summary>
        /// <param name="vms">The vms<see cref="IEnumerable{ZipFileViewModel}"/>.</param>
        public static void Update(IEnumerable<ZipFileViewModel> vms)
        {
            Debug.Assert(vms != null);
            using var dbc = new PmDb();

            foreach (var vm in vms)
            {
                Debug.Assert(vm.PackageId != 0);
                var ent = dbc.Find<ZipFile>(vm.PackageId);
                ent.GetViewModel(vm);
            }

            dbc.SaveChanges();
        }

        /// <summary>
        /// Updates a single zip file of the database.
        /// </summary>
        /// <param name="vm">The vm<see cref="ZipFileViewModel"/>.</param>
        public static void Update(ZipFileViewModel vm)
        {
            Debug.Assert(vm != null);
            Debug.Assert(vm.PackageId != 0);
            using var dbc = new PmDb();
            var ent = dbc.Find<ZipFile>(vm.PackageId);
            ent.GetViewModel(vm);
            dbc.SaveChanges();
        }
    }
}