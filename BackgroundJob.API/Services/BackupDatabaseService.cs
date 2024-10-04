using System.Data;
using System.Data.SqlClient;

namespace BackgroundJob.API.Services
{
    public class BackupDatabaseService : IBackupDatabaseService
    {
        private readonly IConfiguration _configuration;

        public BackupDatabaseService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void BackupDatabase(string databaseName)
        {
            // use the default sql server base path from appsettings.json if localDatabasePath is null
            var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            var localDatabasePath = Path.Combine(_configuration["ConnectionStrings:SqlServerBasePath"], "Backup", $"{databaseName}-{currentTime}.bak");

            // otherwise check if it ends with .bak
            if (!localDatabasePath.EndsWith(".bak"))
            {
                throw new ArgumentException("localDatabasePath must end with .bak.");
            }

            var formatMediaName = $"DatabaseToolkitBackup_{databaseName}";
            var formatName = $"Full Backup of {databaseName}";

            using (var connection = new SqlConnection(_configuration["ConnectionStrings:HangfireConnection"]))
            {
                var sql = @"BACKUP DATABASE @databaseName
                    TO DISK = @localDatabasePath
                    WITH FORMAT,
                      MEDIANAME = @formatMediaName,
                        NAME = @formatName";

                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 7200;
                    command.Parameters.AddWithValue("@databaseName", databaseName);
                    command.Parameters.AddWithValue("@localDatabasePath", localDatabasePath);
                    command.Parameters.AddWithValue("@formatMediaName", formatMediaName);
                    command.Parameters.AddWithValue("@formatName", formatName);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
