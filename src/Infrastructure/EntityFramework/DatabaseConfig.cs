namespace QuizeMC.Infrastructure.EntityFramework
{
    public class DatabaseConfig
    {
        public string Host { get; set; } = "localhost";
        public string Database { get; set; } = "QuizeMCnew";
        public string Username { get; set; } = "postgres";
        public string Password { get; set; } = "999999";
        public int Port { get; set; } = 5432;
        public bool Pooling { get; set; } = true;
        public int CommandTimeout { get; set; } = 30;
        public int MaxPoolSize { get; set; } = 100;
        public int ConnectionLifetime { get; set; } = 300;
        public bool TrustServerCertificate { get; set; } = true;
        public string SslMode { get; set; } = "Prefer";

        public string GetConnectionString()
        {
            return $"Host={Host};Database={Database};Username={Username};Password={Password};Port={Port};" +
                   $"Pooling={Pooling};CommandTimeout={CommandTimeout};Maximum Pool Size={MaxPoolSize};" +
                   $"Connection Lifetime={ConnectionLifetime};SSL Mode={SslMode};Trust Server Certificate={TrustServerCertificate}";
        }

        public static DatabaseConfig GetDefault()
        {
            return new DatabaseConfig
            {
                Host = "localhost",
                Database = "QuizeMCnew",
                Username = "postgres",
                Password = "999999",
                Port = 5432,
                Pooling = true,
                CommandTimeout = 30,
                MaxPoolSize = 100,
                ConnectionLifetime = 300,
                TrustServerCertificate = true,
                SslMode = "Prefer"
            };
        }
    }
}