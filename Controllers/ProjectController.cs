using Microsoft.AspNetCore.Mvc;
using Test1retake.Exceptions;
using Test1retake.Services;
namespace Test1retake.Controllers;

public class ProjectController : Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public ProjectsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var project = await _dbService.GetProjectByIdAsync(id);
                return Ok(project);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }

    

}






