namespace Playstudios.Common.Configuration
{
    public class PlaystudiosConfiguration : IPlaystudiosConfiguration
    {
        public string DatabaseConnectionString => Environment.GetEnvironmentVariable("DefaultConnection") ?? "Server=USER-PC\\ALFEM;Database=playstudio-dev-db;Integrated Security=True;TrustServerCertificate=True;";
        
        public string ResetPasswordUrlString => Environment.GetEnvironmentVariable("ResetPasswordUrl") ?? "http://account/reset-password?";

        public string BrevoApiKeyString => Environment.GetEnvironmentVariable("BrevoApiKey") ?? "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";
    }
}
