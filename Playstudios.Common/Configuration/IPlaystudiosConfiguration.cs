namespace Playstudios.Common.Configuration
{
    public interface IPlaystudiosConfiguration
    {
        string DatabaseConnectionString { get; }

        string ResetPasswordUrlString { get; }

        string BrevoApiKeyString { get; }
    }
}
