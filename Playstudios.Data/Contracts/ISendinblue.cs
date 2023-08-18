namespace Playstudios.Data.Contracts
{
    public interface ISendinblue
    {
        bool SendResetPasswordCode(string email, string name, string resetCode);
    }
}
