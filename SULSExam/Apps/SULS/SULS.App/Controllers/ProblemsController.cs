using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels.Problems;
using SULS.Services;
using System.Linq;

namespace SULS.App.Controllers
{
    public class ProblemsController : Controller
    {
        private const string ProblemsCreatePage = "/Problems/Create";
        private const string HomePage = "/";

        private readonly IProblemService problemService;

        public ProblemsController(IProblemService problemService)
        {
            this.problemService = problemService;
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View(); 
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateProblemInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(ProblemsCreatePage);
            }

            this.problemService.CreateProblem(model.Name, model.Points); 
            
            return this.Redirect(HomePage); 
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var problem = this.problemService.GetProblemById(id);

            var problemViewModel = new DetailsProblemViewModel
            {
                Name = problem.Name,
                MaxPoints = problem.Points,
                Submissions = problem.Submissions.Select(x => new DetailsProblemSubmissionsViewModel
                {
                    Username = x.User.Username,
                    AchievedResult = x.AchievedResult,
                    CreatedOn = x.CreatedOn,
                    SubmissionId = x.Id
                })
                .ToList()
            }; 

            return this.View(problemViewModel);
        }
    }
}