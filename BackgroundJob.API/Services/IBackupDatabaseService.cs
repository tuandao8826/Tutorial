namespace BackgroundJob.API.Services
{
    public interface IBackupDatabaseService
    {
        void BackupDatabase(string databaseName);
    }
}
