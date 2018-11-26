// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IVersionService.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli.Services
{
    using Atlassian.Jira;
    using JiraCli.Models;
    using System.Collections.Generic;

    public interface IVersionService
    {
        void CreateVersion(IJiraRestClient jiraRestClient, string projectKey, string version);
        void ReleaseVersion(IJiraRestClient jiraRestClient, string projectKey, string version);
        void MergeVersions(IJiraRestClient jiraRestClient, string projectKey, string version);
        string[] AssignVersionToIssues(IJiraRestClient jiraRestClient, string projectKey, string version, string[] issues);
        List<JiraProjectVersion> DeleteFeatureBranchVersions(IJiraRestClient jiraRestClient, string projectKey, string currentVersion, string[] mergedFeatureBranchNames);
    }
}