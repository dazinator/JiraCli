﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMergeVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2015 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    public interface IMergeVersionService
    {
        bool ShouldBeMerged(string versionBeingReleased, string versionToCheck);

        bool ShouldFeatureVersionBeDeleted(string currentVersion, string versionToDelete, string featureName);

        bool ShouldHotfixVersionBeDeleted(string currentVersion, string hotfixVersion, string hotfixPrereleaseLabelPrefix = "beta.");
    }
}