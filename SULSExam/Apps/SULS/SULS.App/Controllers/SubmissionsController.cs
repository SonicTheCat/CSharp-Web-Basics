using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels.Submissions;
using SULS.Services;

namespace SULS.App.Controllers
{
    public class SubmissionsController : Controller
    {
        private const string HomePage = "/";

        private readonly IProblemService problemService;
        private readonly ISubmissionService submissionService;

        public SubmissionsController(IProblemService problemService, ISubmissionService submissionService)
        {
            this.problemService = problemService;
            this.submissionService = submissionService;
        }

        [Authorize]
        public IActionResult Create(string id)
        {
            var problem = this.problemService.GetProblemById(id);

            var viewModel = new CreateSubmissionViewModel
            {
                Name = problem.Name,
                ProblemId = problem.Id
            }; 

            return this.View(viewModel); 
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateSubmissionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Create(model.ProblemId); 
            }

            this.submissionService.Create(model.Code, model.ProblemId, this.User.Id);

            return this.Redirect(HomePage); 
        }

        [Authorize]
        public IActionResult Delete(string id)
        {
           this.submissionService.DeleteById(id);

            return this.Redirect(HomePage); 
        }
    }
}