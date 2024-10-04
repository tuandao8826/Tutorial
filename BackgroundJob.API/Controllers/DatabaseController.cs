using BackgroundJob.API.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundJob.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IBackupDatabaseService _backupDatabaseService;
        private readonly IRecurringJobManager _recurringJobManager;

        public DatabaseController(IBackupDatabaseService backupDatabaseService, IRecurringJobManager recurringJobManager)
        {
            this._backupDatabaseService = backupDatabaseService;
            this._recurringJobManager = recurringJobManager;
        }

        /*[HttpGet("BackupDatabase")]
        public ActionResult BackupDatabase()
        {
            _recurringJobManager.AddOrUpdate("BackupDB", () => _backupDatabaseService.BackupDatabase("ProductTutorial"), Cron.Minutely);
            return Ok();
        }*/
    }
}
