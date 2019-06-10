using NuciLog.Core;

namespace SteamGiveawaysBot.Server.Logging
{
    public sealed class MyLogInfoKey : LogInfoKey
    {
        MyLogInfoKey(string name)
            : base(name)
        {
            
        }
        
        public static LogInfoKey User => new MyLogInfoKey(nameof(User));
        
        public static LogInfoKey GiveawaysProvider => new MyLogInfoKey(nameof(GiveawaysProvider));
        
        public static LogInfoKey GiveawayId => new MyLogInfoKey(nameof(GiveawayId));
    }
}
