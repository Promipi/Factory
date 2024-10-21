using Common.Core.Contracts.Jobs;
using Common.Core.Jobs;
using Hangfire;
using Hangfire.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        
        public JobsController()
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobRequest request)
        {
            RecurringJob.AddOrUpdate<CheckDataRequestJob>(request.Name, job => job.Execute(request.UrlCheck), Cron.Minutely() );
            return Ok("Job created to execute every minute.");
        }
    }

}
