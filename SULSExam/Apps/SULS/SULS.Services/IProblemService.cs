using SULS.Models;
using System.Collections.Generic;

namespace SULS.Services
{
    public interface IProblemService
    {
        void CreateProblem(string name, int points);

        IEnumerable<Problem> AllProblems();

        Problem GetProblemById(string id); 
    }
}