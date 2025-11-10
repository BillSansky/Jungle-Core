#if UNITY_EDITOR
using System;

/// <summary>
/// Helper utilities for comparing semantic version strings used by Jungle packages.
/// </summary>
internal static class JunglePackageVersionUtility
{
    internal static int CompareVersions(string left, string right)
    {
        if (string.IsNullOrEmpty(left))
        {
            return string.IsNullOrEmpty(right) ? 0 : -1;
        }

        if (string.IsNullOrEmpty(right))
        {
            return 1;
        }

        if (TryParse(left, out var leftVersion) && TryParse(right, out var rightVersion))
        {
            return leftVersion.CompareTo(rightVersion);
        }

        return string.Compare(left, right, StringComparison.OrdinalIgnoreCase);
    }

    internal static bool IsAtLeast(string version, string minimum)
    {
        if (string.IsNullOrEmpty(minimum))
        {
            return true;
        }

        if (string.IsNullOrEmpty(version))
        {
            return false;
        }

        return CompareVersions(version, minimum) >= 0;
    }

    private static bool TryParse(string value, out SemanticVersion version)
    {
        version = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();

        var metadataIndex = trimmed.IndexOf('+');
        if (metadataIndex >= 0)
        {
            trimmed = trimmed[..metadataIndex];
        }

        string preRelease = null;
        var prereleaseIndex = trimmed.IndexOf('-');
        if (prereleaseIndex >= 0)
        {
            preRelease = trimmed[(prereleaseIndex + 1)..];
            trimmed = trimmed[..prereleaseIndex];
        }

        var sections = trimmed.Split('.');
        if (sections.Length == 0)
        {
            return false;
        }

        if (!int.TryParse(sections[0], out var major))
        {
            return false;
        }

        var minor = sections.Length > 1 && int.TryParse(sections[1], out var minorValue) ? minorValue : 0;
        var patch = sections.Length > 2 && int.TryParse(sections[2], out var patchValue) ? patchValue : 0;
        var build = sections.Length > 3 && int.TryParse(sections[3], out var buildValue) ? buildValue : 0;

        if (sections.Length > 4)
        {
            return false;
        }

        version = new SemanticVersion(major, minor, patch, build, preRelease);
        return true;
    }

    private readonly struct SemanticVersion : IComparable<SemanticVersion>
    {
        private readonly int major;
        private readonly int minor;
        private readonly int patch;
        private readonly int build;
        private readonly string preRelease;

        public SemanticVersion(int major, int minor, int patch, int build, string preRelease)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
            this.build = build;
            this.preRelease = string.IsNullOrEmpty(preRelease) ? null : preRelease;
        }

        public int CompareTo(SemanticVersion other)
        {
            var compareMajor = major.CompareTo(other.major);
            if (compareMajor != 0)
            {
                return compareMajor;
            }

            var compareMinor = minor.CompareTo(other.minor);
            if (compareMinor != 0)
            {
                return compareMinor;
            }

            var comparePatch = patch.CompareTo(other.patch);
            if (comparePatch != 0)
            {
                return comparePatch;
            }

            var compareBuild = build.CompareTo(other.build);
            if (compareBuild != 0)
            {
                return compareBuild;
            }

            var hasPreRelease = !string.IsNullOrEmpty(preRelease);
            var otherHasPreRelease = !string.IsNullOrEmpty(other.preRelease);

            if (!hasPreRelease && !otherHasPreRelease)
            {
                return 0;
            }

            if (!hasPreRelease)
            {
                return 1;
            }

            if (!otherHasPreRelease)
            {
                return -1;
            }

            return string.Compare(preRelease, other.preRelease, StringComparison.OrdinalIgnoreCase);
        }
    }
}
#endif
