// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateVersionAction.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace JiraCli
{
    using Catel;
    using Catel.Logging;
    using Services;

    [Action("CreateVersion",
        Description = "Creates a version for the specified project",
        ArgumentsUsage = "-project [projectkey] -version [version]")]
    public class CreateVersionAction : ActionBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IVersionService _versionService;

        public CreateVersionAction(IVersionService versionService)
        {
            Argument.IsNotNull(() => versionService);

            _versionService = versionService;
        }

        protected override void ValidateContext(Context context)
        {
            if (string.IsNullOrEmpty(context.Project))
            {
                Log.ErrorAndThrowException<JiraCliException>("Project is missing");
            }

            if (string.IsNullOrEmpty(context.Version))
            {
                Log.ErrorAndThrowException<JiraCliException>("Version is missing");
            }

            if (string.IsNullOrEmpty(context.Branch))
            {
                Log.ErrorAndThrowException<JiraCliException>("Branch is missing");
            }
        }

        protected override void ExecuteWithContext(Context context)
        {
            var jira = CreateJira(context);

            _versionService.CreateVersion(jira, context.Project, context.Version);

            // It's sometimes desirable to delete versions in Jira, for example when
            // a feature branch is integrated into develop branch, we can delete the feature branch versions
            // when the new develop build version is created. So that's what we do here.
            var mergedVersions = context.MergedFeatureBranchesForDelete?.ToArray();
            if (mergedVersions?.Length > 0)
            {
                var deletedVersions = _versionService.DeleteFeatureBranchVersions(jira, context.Project, context.Version, mergedVersions);
            }

            // todo: also handle merged hotfix branches
            var mergedHotfixVersions = context.MergedHotfixBranchesForDelete?.ToArray();
            if (mergedHotfixVersions?.Length > 0)
            {
                var deletedVersions = _versionService.DeleteHotfixBranchVersions(jira, context.Project, context.Version, mergedHotfixVersions);
            }

        }


    }

}
