# Azure DevOps Client

## Available Repositories
- Build
- Git
- Pipelines
- Projects
- Releases
- Test
- Work
- WorkItemTracking

## Example Code
```
string personalAccessToken = ""; // generate a token with the correct scopes from the website
string organization = ""; // https://dev.azure.com/orgname
string project = ""; // https://dev.azure.com/orgname/projectname

AzureDevOpsUrlBuilderFactory orgFactory = new(personalAccessToken, organization);
AzureDevOpsRepository orgRepository = new (orgFactory);
List<TeamProjectReference> projects = orgRepository.Projects.GetProjectsForOrganizationAsync();

AzureDevOpsUrlBuilderFactory projectFactory = new(personalAccessToken, organization, project);
AzureDevOpsRepository projectRepository = new (projectFactory);
List<GitRepository> repositories = projectRepository.Git.GetRepositoriesForProjectAsync();
```

Set the version of the DevOps API you want to target
`AzureDevOpsUrlBuilderFactory projectFactory = new(personalAccessToken, organization, project) {Version = "7.0"};`


## Notes
We use Newtonsoft.Json because System.Text.Json does not support any dictionary with a key that is not a string.