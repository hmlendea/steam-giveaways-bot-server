using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SteamGiveawaysBot.Server.Communication
{
    public abstract class MailSender : IMailSender
    {
        readonly SmtpClient smtpClient;

        public MailSender()
        {
            smtpClient = BuildSmtpClient();
        }

        public async Task SendMailAsync(
            string senderAddress,
            string senderPassword,
            string subject,
            string body,
            params string[] recipientAddresses)
            => await SendMailAsync(senderAddress, senderAddress, senderPassword, subject, body, recipientAddresses);

        public async Task SendMailAsync(
            string senderAddress,
            string senderName,
            string senderPassword,
            string subject,
            string body,
            params string[] recipientAddresses)
        {
            MailAddress senderMailAddress = new MailAddress(senderAddress, senderName);
            smtpClient.Credentials = new NetworkCredential(senderAddress, senderPassword);

            using (MailMessage mail = new MailMessage())
            {
                mail.From = senderMailAddress;
                mail.Subject = subject;
                mail.Body = body;
                
                foreach (string recipientAddress in recipientAddresses)
                {
                    mail.To.Add(recipientAddress);
                }

                await smtpClient.SendMailAsync(mail);
            }
        }

        protected abstract SmtpClient BuildSmtpClient();
    }
}
