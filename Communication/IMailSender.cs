using System.Threading.Tasks;

namespace SteamGiveawaysBot.Server.Communication
{
    public interface IMailSender
    {
        Task SendMailAsync(
            string senderAddress,
            string senderPassword,
            string subject,
            string body,
            params string[] recipientAddresses);
        
        Task SendMailAsync(
            string senderAddress,
            string senderName,
            string senderPassword,
            string subject,
            string body,
            params string[] recipientAddresses);
    }
}