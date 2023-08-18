namespace Playstudios.Data.Providers.Email
{
    using Playstudios.Common.Configuration;
    using Playstudios.Data.Contracts;
    using sib_api_v3_sdk.Api;
    using sib_api_v3_sdk.Client;
    using sib_api_v3_sdk.Model;
    using System;
    using System.Collections.Generic;

    public class Sendinblue : ISendinblue
    {
        private IPlaystudiosConfiguration config;

        public Sendinblue(IPlaystudiosConfiguration _config)
        {
            config= _config;
        }

        public bool SendResetPasswordCode(string email, string name, string resetCode)
        {
            string subject = "Reset Password";
            string htmlContent = 
                $"<html lang=\"en\">" + 
                $"<head>" + 
                $"    <meta charset=\"UTF-8\">" + 
                $"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" + 
                $"    <title>Reset Password</title>" + 
                $"</head>" + 
                $"<body>" + 
                $"    <table style=\"width: 100%; max-width: 600px; margin: 0 auto; font-family: Arial, sans-serif; border-collapse: collapse;\">" + 
                $"        <tr>" + 
                $"            <td style=\"padding: 20px; text-align: center; background-color: #007bff; color: #ffffff;\">" + 
                $"                <h1>Reset Your Password</h1>" + 
                $"            </td>" + 
                $"        </tr>" + 
                $"        <tr>" + 
                $"            <td style=\"padding: 20px;\">" + 
                $"                <p>Hello,</p>" + 
                $"                <p>We received a request to reset your password. Click the button below to reset your password:</p>" + 
                $"                <p style=\"text-align: center;\">" + 
                $"                    <a href=\"{config.ResetPasswordUrlString}{resetCode}\" style=\"display: inline-block; padding: 10px 20px; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 5px;\">Reset Password</a>" + 
                $"                </p>" + 
                $"                <p>If you did not request a password reset, please ignore this email.</p>" + 
                $"            </td>" + 
                $"        </tr>" + 
                $"        <tr>" + 
                $"            <td style=\"padding: 20px; text-align: center; background-color: #f4f4f4;\">" + 
                $"                <p>If you have any questions or need assistance, please contact our support team at support@example.com.</p>" + 
                $"            </td>" + 
                $"        </tr>" + 
                $"    </table>" + 
                $"</body>" + 
                $"</html>";

            return SendEmail(email,name,resetCode,htmlContent);
        }

        private bool SendEmail(string email, string name, string subject, string htmlContent)
        {
            Configuration.Default.ApiKey.Add("api-key", config.BrevoApiKeyString);

            var apiInstance = new TransactionalEmailsApi();
            string SenderName = "Alfem";
            string SenderEmail = "alfem6330@gmail.com";
            SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);
            string ToEmail = email;
            string ToName = name;
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, ToName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(smtpEmailTo);
            List<SendSmtpEmailBcc> Bcc = new List<SendSmtpEmailBcc>();;
            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
            string HtmlContent = htmlContent;
            string Subject = subject;
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(sender:Email, to:To,htmlContent: HtmlContent, subject:Subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
