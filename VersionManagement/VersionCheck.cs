namespace VersionManagement
{
    using PluginManager.Core.ViewModels;
    using System;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="VersionCheck" />.
    /// </summary>
    public class VersionCheck : ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCheck"/> class.
        /// </summary>
        /// <param name="currentVersion">The currentVersion<see cref="string"/>.</param>
        /// <param name="packageUrl">The packageUrl<see cref="string"/>.</param>
        public VersionCheck(string currentVersion, string packageUrl)
        {
            if (string.IsNullOrEmpty(currentVersion))
            {
                throw new ArgumentException($"'{nameof(currentVersion)}' cannot be null or empty.", nameof(currentVersion));
            }

            if (string.IsNullOrEmpty(packageUrl))
            {
                throw new ArgumentException($"'{nameof(packageUrl)}' cannot be null or empty.", nameof(packageUrl));
            }

            CurrentVersion = currentVersion;
            PackageUrl = packageUrl;

            CurrentVersionInfo = new VersionInfo(CurrentVersion);
            Versions = new VersionCollection(PackageUrl);
            DoesUpdateExist = !CurrentVersionInfo.IsLatestVersion(Versions);
            LatestVersionInfo = Versions.Max();
            LastestVersionIsPrerelease = !string.IsNullOrEmpty(LatestVersionInfo.Suffix);
            LatestVersionDownloadUrl = LatestVersionInfo.Download;
        }

        /// <summary>
        /// Gets the CurrentVersion.
        /// </summary>
        public string CurrentVersion { get; }

        /// <summary>
        /// Gets the CurrentVersionInfo.
        /// </summary>
        public VersionInfo CurrentVersionInfo { get; }

        /// <summary>
        /// Gets a value indicating whether DoesUpdateExist.
        /// </summary>
        public bool DoesUpdateExist { get; }

        /// <summary>
        /// Gets a value indicating whether LastestVersionIsPrerelease.
        /// </summary>
        public bool LastestVersionIsPrerelease { get; }

        /// <summary>
        /// Gets the LatestVersionDownloadUrl.
        /// </summary>
        public string LatestVersionDownloadUrl { get; }

        /// <summary>
        /// Gets the LatestVersionInfo.
        /// </summary>
        public VersionInfo LatestVersionInfo { get; }

        /// <summary>
        /// Gets the PackageUrl.
        /// </summary>
        public string PackageUrl { get; }

        /// <summary>
        /// Gets the Versions.
        /// </summary>
        public VersionCollection Versions { get; }

        /// <summary>
        /// Open the default browser to the Download Site.
        /// </summary>
        public void OpenDownloadSite()
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {LatestVersionDownloadUrl}") { CreateNoWindow = true });
        }
    }
}
