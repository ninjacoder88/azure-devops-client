using System.Collections.Generic;

namespace Ninjasoft.AzureDevOpsClient.Models
{
    public class GitPullRequest
    {
        public IdentityRef CreatedBy { get; set; }

        public string CreationDate { get; set; }

        public bool IsDraft { get; set; }

        public List<IdentityRefWithVote> Reviewers { get; set; }

        //public string Url {get;set;}

        public string Status { get; set; }

        public string Title { get; set; }
    }
}