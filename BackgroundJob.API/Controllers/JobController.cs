using BackgroundJob.API.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJob.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobTestService _jobTestService;

        // IBackgroundJobClient: Cung cấp các phương thức để tạo và lên lịch cho các Job
        private readonly IBackgroundJobClient _backgroundJobClient;

        // IRecurringJobManager: Dùng để quản lý các công việc theo định kỳ
        private readonly IRecurringJobManager _recurringJobManager;

        public JobController(IJobTestService jobTestService, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            this._jobTestService = jobTestService;
            this._backgroundJobClient = backgroundJobClient;
            this._recurringJobManager = recurringJobManager;
        }

        [HttpGet("FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            return Ok();
        }

        [HttpGet("DelayedJob")]
        public ActionResult CreateDelayedJob()
        {
            _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(30));
            return Ok();
        }

        [HttpGet("RecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            _recurringJobManager.AddOrUpdate(
                "JobId",                  // ID của job
                () => _jobTestService.ReccuringJob(),  // Expression chỉ định công việc sẽ thực hiện
                Cron.Minutely // Biểu thức cron định nghĩa tần suất thực hiện công việc
            );           
            return Ok();
        }

        [HttpGet("ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var jobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
            _backgroundJobClient.ContinueJobWith(
                jobId,                  
                () => _jobTestService.ContinuationJob()
            );

            return Ok();
        }
    }
}
