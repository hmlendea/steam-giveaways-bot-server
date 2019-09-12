namespace SteamGiveawaysBot.Server.Communication
{
    public interface IMailSender
    {
        void SendMail(
            string senderAddress,
            string senderPassword,
            string subject,
            string body,
            params string[] recipientAddresses);
        
        void SendMail(
            string senderAddress,
            string senderName,
            string senderPassword,
            string subject,
            string body,
            params string[] recipientAddresses);
    }
}