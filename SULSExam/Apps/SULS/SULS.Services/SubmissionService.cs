using SULS.Data;
using SULS.Models;
using System;
using System.Linq;

namespace SULS.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly SULSContext context;
        private readonly IProblemService problemService;

        public SubmissionService(SULSContext context, IProblemService problemService)
        {
            this.context = context;
            this.problemService = problemService;
        }

        public void Create(string code, string problemId, string userId)
        {
            var problem = this.problemService.GetProblemById(problemId);

            var random = new Random();
            var achievedPoints = random.Next(0, problem.Points);

            var submission = new Submission
            {
                Code = code,
                AchievedResult = achievedPoints,
                CreatedOn = DateTime.UtcNow,
                ProblemId = problemId,
                UserId = userId
            };

            this.context.Submissions.Add(submission);
            this.context.SaveChanges();
        }

        public bool DeleteById(string submissionId)
        {
            var submision = this.context.Submissions.SingleOrDefault(x => x.Id == submissionId);
            this.context.Submissions.Remove(submision);
            var deletedCount = this.context.SaveChanges();

            return deletedCount == 1; 
        }
    }
}