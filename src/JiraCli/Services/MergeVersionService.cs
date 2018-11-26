// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using Catel;
    using Catel.Logging;
    using System;

    public class MergeVersionService : IMergeVersionService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IVersionInfoService _versionInfoService;

        public MergeVersionService(IVersionInfoService versionInfoService)
        {
            Argument.IsNotNull(() => versionInfoService);

            _versionInfoService = versionInfoService;
        }

        public bool ShouldFeatureVersionBeDeleted(string currentVersion, string featureVersion, string featureName)
        {
            Argument.IsNotNull(() => currentVersion);
            Argument.IsNotNull(() => featureVersion);

            // Features can only be deleted when:
            // 1. the current version being built is an unstable build (i.e develop branch),   
            // 2. the version we are considering to delete should have a pre-release label matching the feature branch name that has been merged.


            //1.
            if (_versionInfoService.IsReleaseVersion(currentVersion))
            {
                return false;
            }       

            // 2.
            if (!_versionInfoService.IsPreRelease(featureVersion, (label) =>
            {
                return label.ToLowerInvariant().StartsWith(featureName.ToLowerInvariant());

            }))
            {
                return false;
            }

            //// 3.

            //var versionComparison = _versionInfoService.CompareVersions(versionToDelete, currentVersion);
            //switch (versionComparison)
            //{
            //    case VersionComparisonResult.LessThan:
            //        return true;

            //    case VersionComparisonResult.Unknown:
            //        Log.Warning("Unable to compare version '{0}' with version '{1}'. Version will not be included in the Merge.", versionToDelete, versionBeingReleased);
            //        return false;

            //    default:
            //        return false;
            //}

            return true;
        }

        public bool ShouldBeMerged(string versionBeingReleased, string versionToCheck)
        {
            Argument.IsNotNull(() => versionBeingReleased);
            Argument.IsNotNull(() => versionToCheck);

            // Only non pre-release versions can be merged in to.
            if (!_versionInfoService.IsReleaseVersion(versionBeingReleased))
            {
                return false;
            }

            // Only pre-release versions with an "unstable" label prefix are valid to merge.
            // semVer.Prerelease.ToLowerInvariant().StartsWith(labelPrefix.ToLowerInvariant())

            if (!_versionInfoService.IsPreRelease(versionToCheck, (label) =>
            {
                return label.ToLowerInvariant().StartsWith("unstable") ||
                       label.ToLowerInvariant().StartsWith("alpha");
            }))
            {
                return false;
            }

            // version must be less than the version being released.
            var versionComparison = _versionInfoService.CompareVersions(versionToCheck, versionBeingReleased);
            switch (versionComparison)
            {
                case VersionComparisonResult.LessThan:
                    return true;

                case VersionComparisonResult.Unknown:
                    Log.Warning("Unable to compare version '{0}' with version '{1}'. Version will not be included in the Merge.", versionToCheck, versionBeingReleased);
                    return false;

                default:
                    return false;
            }

        }
    }
}