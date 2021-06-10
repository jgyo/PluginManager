namespace VersionManagement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="VersionCollection" />.
    /// </summary>
    public class VersionCollection : ICollection<VersionInfo>
    {
        /// <summary>
        /// Defines the GitHub program home URL
        /// </summary>
        private readonly string programHome;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCollection"/> class.
        /// </summary>
        /// <param name="programHome">The versionUrl<see cref="string"/>.</param>
        public VersionCollection(string programHome)
        {
            this.programHome = programHome;
        }

        /// <summary>
        /// Gets the Count of versions found.
        /// </summary>
        public int Count => VersionInfos.Count;

        /// <summary>
        /// Gets a value indicating whether IsReadOnly.
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Gets a list of versions.
        /// Defines the versionInfos..
        /// </summary>
        private List<VersionInfo> VersionInfos => new Lazy<List<VersionInfo>>(
                () => new List<VersionInfo>(
                    GetKnownVersions(programHome)))
            .Value;

        /// <summary>
        /// The Add methode is not implemented.
        /// </summary>
        /// <param name="item">The item<see cref="VersionInfo"/>.</param>
        public void Add(VersionInfo item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Clear method is not implemented.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Contains method determines if the given item is contined
        /// within the collection.
        /// </summary>
        /// <param name="item">The item<see cref="VersionInfo"/>.</param>
        /// <returns>The <see cref="bool"/> result is true if the item is
        /// contained in the collection. Otherwise, false.
        /// </returns>
        public bool Contains(VersionInfo item)
        {
            return VersionInfos.Contains(item);
        }

        /// <summary>
        /// Copies the list items to an array starting at the array index.
        /// </summary>
        /// <param name="array">The array<see cref="VersionInfo[]"/>.</param>
        /// <param name="arrayIndex">The arrayIndex<see cref="int"/>.</param>
        public void CopyTo(VersionInfo[] array, int arrayIndex)
        {
            VersionInfos.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets an enumerator of the version collection.
        /// </summary>
        /// <returns>The <see cref="IEnumerator{VersionInfo}"/>.</returns>
        public IEnumerator<VersionInfo> GetEnumerator()
        {
            return VersionInfos.GetEnumerator();
        }

        /// <summary>
        /// The Remove method is not implemented.
        /// </summary>
        /// <param name="item">The item<see cref="VersionInfo"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool Remove(VersionInfo item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an IEnumerable of the known versions.
        /// </summary>
        /// <param name="versionUrl">The versionUrl<see cref="string"/>.</param>
        /// <returns>The <see cref="IEnumerable{VersionInfo}"/>.</returns>
        private static IEnumerable<VersionInfo> GetKnownVersions(string versionUrl)
        {
            string page;
            var captures = new List<string>();
            using (WebClient wc = new())
            {
                page = wc.DownloadString(versionUrl + "/tags");
            }

            string pattern = @"v\.\d+\.\d+\.\d+(-\w+)*";
            RegexOptions regexOptions = RegexOptions.None;
            Regex regex = new(pattern, regexOptions);

            foreach (Match match in regex.Matches(page))
            {
                if (match.Success && !captures.Contains(match.Value))
                {
                    captures.Add(match.Value);
                    yield return new VersionInfo(match.Value.TrimStart('v', '.'), versionUrl);
                }
            }
        }

        /// <summary>
        /// The GetEnumerator.
        /// </summary>
        /// <returns>The <see cref="IEnumerator"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return VersionInfos.GetEnumerator();
        }
    }
}
