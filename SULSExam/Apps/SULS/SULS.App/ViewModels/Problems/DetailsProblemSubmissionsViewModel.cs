using System;

namespace SULS.App.ViewModels.Problems
{
    public class DetailsProblemSubmissionsViewModel
    {
        public string Username { get; set; }

        public int AchievedResult { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SubmissionId { get; set; }
    }
}
