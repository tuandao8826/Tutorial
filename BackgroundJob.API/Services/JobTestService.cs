namespace BackgroundJob.API.Services
{
    public class JobTestService : IJobTestService
    {
        private readonly ILogger<JobTestService> _logger;

        public JobTestService(ILogger<JobTestService> logger)
        {
            this._logger = logger;
        }
        public void ContinuationJob()
        {
            _logger.LogInformation("Continuation Job");
        }

        public void DelayedJob()
        {
            _logger.LogInformation("Delayed Job");
        }

        public void FireAndForgetJob()
        {
            _logger.LogInformation("FireAndForget Job");
        }

        public void ReccuringJob()
        {
            _logger.LogInformation("Reccuring Job");
        }
    }
}
