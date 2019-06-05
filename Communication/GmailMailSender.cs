using System;
using System.Net;
using System.Net.Mail;

namespace SteamGiveawaysBot.Server.Communication
{
    public sealed class GmailMailSender : MailSender
    {
        protected override SmtpClient BuildSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            return client;
        }
    }
}