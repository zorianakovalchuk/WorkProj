using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WorkProj.Services
{
    public class EmailSender
    {
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole())
            .CreateLogger(typeof(EmailSender));

        public static void SendEmail(string mail, string subject, string message)
        {
            _logger.LogInformation("Вхід в метод надсилання листа");

            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress("Cozy Library", "zorianakovalchuk04@ukr.net"));
            email.To.Add(new MailboxAddress("Шановний клієнт", mail));
            email.Subject = subject;
            email.Body = new TextPart() { Text = message };

            _logger.LogInformation("Дані для надсилання встановленні");

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Connect("smtp.ukr.net", 465, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate("zorianakovalchuk04@ukr.net", "dAyZQpYEVwz0MBsZ");
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            _logger.LogInformation("Лист був успішно надісланий");
        }
    }
}
