// Licensed to Laurent Ellerbach under one or more agreements.
// Laurent Ellerbach licenses this file to you under the MIT license.

using System;

namespace LegoBluetooth
{
    /// <summary>
    /// Provides methods to encode and decode version numbers.
    /// </summary>
    public static class VersionNumberEncoder
    {
        /// <summary>
        /// Decodes an integer into a <see cref="Version"/> object.
        /// </summary>
        /// <param name="data">The integer representing the encoded version number.</param>
        /// <returns>A <see cref="Version"/> object representing the decoded version number.</returns>
        /// <remarks>
        /// The encoding format is as follows:
        /// - Major: 3 bits (bits 28-30)
        /// - Minor: 4 bits (bits 24-27)
        /// - Bugfix: 8 bits (bits 16-23)
        /// - Build: 12 bits (bits 0-11)
        /// </remarks>
        public static Version Decode(int data)
        {
            var major = (data & 0b0111_0000___0000_0000___0000_0000___0000_0000) >> 28;
            var minor = (data & 0b0000_1111___0000_0000___0000_0000___0000_0000) >> 24;
            var bugfix = ((data & 0b0000_0000___1111_0000___0000_0000___0000_0000) >> 20) * 10 + ((data & 0b0000_0000___0000_1111___0000_0000___0000_0000) >> 16);
            var build = ((data & 0b0000_0000___0000_0000___1111_0000___0000_0000) >> 12) * 1000 +
                        ((data & 0b0000_0000___0000_0000___0000_1111___0000_0000) >> 8) * 100 +
                        ((data & 0b0000_0000___0000_0000___0000_0000___1111_0000) >> 4) * 10 +
                        ((data & 0b0000_0000___0000_0000___0000_0000___0000_1111) >> 0) * 1;

            return new Version(major, minor, bugfix, build);
        }

        /// <summary>
        /// Encodes a <see cref="Version"/> object into an integer.
        /// </summary>
        /// <param name="version">The <see cref="Version"/> object to encode.</param>
        /// <returns>An integer representing the encoded version number.</returns>
        /// <exception cref="ArgumentException">Thrown when the version is not valid.</exception>
        /// <remarks>
        /// The encoding format is as follows:
        /// - Major: 3 bits (bits 28-30)
        /// - Minor: 4 bits (bits 24-27)
        /// - Bugfix: 8 bits (bits 16-23)
        /// - Build: 12 bits (bits 0-11)
        /// </remarks>
        public static int Encode(Version version)
        {
            if (version.Major > 7 || version.Minor > 15 || version.Build > 99 || version.Revision > 9999)
            {
                throw new ArgumentException("version is not valid", nameof(version));
            }

            return (version.Major << 28) +
                    (version.Minor << 24) +
                    ((version.Build / 10) << 20) + ((version.Build % 10) << 16) +
                    ((version.Revision / 1000) << 12) +
                    (((version.Revision % 1000) / 100) << 8) +
                    (((version.Revision % 100) / 10) << 4) +
                    (((version.Revision % 10) / 1) << 0);
        }
    }
}
