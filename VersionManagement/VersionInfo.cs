namespace VersionManagement
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="VersionInfo" />.
    /// </summary>
    public class VersionInfo : IComparable<VersionInfo>, IComparable, IEqualityComparer<VersionInfo>
    {
        /// <summary>
        /// Defines the build version.
        /// </summary>
        private int build;

        /// <summary>
        /// Defines the major version.
        /// </summary>
        private int major;

        /// <summary>
        /// Defines the minor version.
        /// </summary>
        private int minor;

        /// <summary>
        /// Defines the suffix version.
        /// </summary>
        private string suffix;

        /// <summary>
        /// Defines the version.
        /// </summary>
        private string version;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionInfo"/> class.
        /// </summary>
        /// <param name="version">The version<see cref="string"/>.</param>
        /// <param name="versionUrl">The versionUrl<see cref="string"/>.</param>
        public VersionInfo(string version, string versionUrl = null)
        {
            Version = version;
            if (versionUrl != null)
                Download = versionUrl + $"/releases/tag/v.{version}";
        }

        /// <summary>
        /// Gets the Build version.
        /// </summary>
        public int Build => build;

        /// <summary>
        /// Gets the Download url.
        /// </summary>
        public string Download { get; }

        /// <summary>
        /// Gets the Major version.
        /// </summary>
        public int Major => major;

        /// <summary>
        /// Gets the Minor version.
        /// </summary>
        public int Minor => minor;

        /// <summary>
        /// Gets the Suffix version.
        /// </summary>
        public string Suffix => suffix;

        /// <summary>
        /// Gets the Version.
        /// </summary>
        public string Version
        {
            get
            {
                return version;
            }

            private set
            {
                version = value;
                var parts = value.Split('.', '-');
                major = int.Parse(parts[0]);
                minor = int.Parse(parts[1]);
                build = int.Parse(parts[2]);
                if (parts.Length == 4)
                    suffix = parts[3];
            }
        }

        /// <summary>
        /// Compares this with the given object to determine relative order.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>An <see cref="int"/> value of 1 if this is higher than the given object,
        /// 0, if equal, or -1 if less.</returns>
        public int CompareTo(object obj)
        {
            if (obj is VersionInfo)
                return CompareTo(obj as VersionInfo);

            throw new ArgumentException("Parameter must be a VersionInfo object.", nameof(obj));
        }

        /// <summary>
        /// Compares this with the given object to determine relative order.
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/>.</param>
        /// <returns>An <see cref="int"/> value of 1 if this is higher than the given object,
        /// 0, if equal, or -1 if less.</returns>
        public int CompareTo(VersionInfo other)
        {
            var result = Major.CompareTo(other.Major);
            if (result != 0)
                return result;
            result = Minor.CompareTo(other.Minor);
            if (result != 0)
                return result;
            result = Build.CompareTo(other.Build);
            if (result != 0)
                return result;

            // If both are null or empty, they are equal.
            if (string.IsNullOrEmpty(Suffix) && string.IsNullOrEmpty(other.Suffix))
                return 0;

            // If this suffix is null or empty, it is greater than the other.
            if (string.IsNullOrEmpty(Suffix))
                return 1;

            // If the other suffix is null or empty, the other is greater than this.
            if (string.IsNullOrEmpty(other.Suffix))
                return -1;

            return Suffix.ToLower().CompareTo(other.Suffix.ToLower());
        }

        /// <summary>
        /// Tests the VersionInfos for equality
        /// </summary>
        /// <param name="x">The x<see cref="VersionInfo"/>.</param>
        /// <param name="y">The y<see cref="VersionInfo"/>.</param>
        /// <returns>A <see cref="bool"/> value of true if equal. Otherwise, false.</returns>
        public bool Equals(VersionInfo x, VersionInfo y)
        {
            if (x == null)
            {
                return y == null;
            }

            if (y == null)
            {
                return false;
            }

            return x.CompareTo(y) == 0;
        }

        /// <summary>
        /// Gets the objects HashCode.
        /// </summary>
        /// <param name="obj">The obj<see cref="VersionInfo"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        public int GetHashCode([DisallowNull] VersionInfo obj)
        {
            return Version.ToLower().GetHashCode();
        }

        /// <summary>
        /// Converts the value to a string representation.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public override string ToString()
        {
            return Version;
        }

        /// <summary>
        /// Determines whether this is later than or equal to the maximum version of vesions.
        /// </summary>
        /// <param name="versions">The versions<see cref="IEnumerable{VersionInfo}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal bool IsLatestVersion(IEnumerable<VersionInfo> versions)
        {
            var latest = versions.Max();
            return CompareTo(latest) >= 0;
        }
    }
}
