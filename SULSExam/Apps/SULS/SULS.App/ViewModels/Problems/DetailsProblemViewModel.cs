using System.Collections.Generic;

namespace SULS.App.ViewModels.Problems
{
    public class DetailsProblemViewModel
    {
        public string Name { get; set; }

        public int MaxPoints { get; set; }

        public IEnumerable<DetailsProblemSubmissionsViewModel> Submissions { get; set; } =
            new HashSet<DetailsProblemSubmissionsViewModel>(); 
    }
}