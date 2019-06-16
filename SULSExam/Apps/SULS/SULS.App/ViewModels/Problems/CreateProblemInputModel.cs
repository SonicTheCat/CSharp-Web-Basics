using SIS.MvcFramework.Attributes.Validation;

namespace SULS.App.ViewModels.Problems
{
    public class CreateProblemInputModel
    {
        private const string NameErrorMessage = "Name must be between 5 and 20 characters long.";
        private const string ProblemErrorMessage = "Problem Points must be between 50 and 300.";

        [RequiredSis]
        [StringLengthSis(5, 20, NameErrorMessage)]
        public string Name { get; set; }

        [RequiredSis]
        [RangeSis(50, 300, ProblemErrorMessage)]
        public int Points { get; set; }
    }
}