using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SULS.Data;
using SULS.Models;

namespace SULS.Services
{
    public class ProblemService : IProblemService
    {
        private readonly SULSContext context;

        public ProblemService(SULSContext context)
        {
            this.context = context;
        }

        public IEnumerable<Problem> AllProblems()
        {
            var problems = this.context.Problems
                .Include(x => x.Submissions)
                .ToList();

            return problems;
        }

        public void CreateProblem(string name, int points)
        {
            var problem = new Problem
            {
                Name = name,
                Points = points
            };

            this.context.Problems.Add(problem);
            this.context.SaveChanges(); 
        }

        public Problem GetProblemById(string id)
        {
           var problem =  this.context.Problems
                .Include(x => x.Submissions)
                .ThenInclude(x => x.User)
                .SingleOrDefault(x => x.Id == id);

            return problem; 
        }
    }
}