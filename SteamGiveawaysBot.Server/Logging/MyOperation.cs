using NuciLog.Core;

namespace SteamGiveawaysBot.Server.Logging
{
    public sealed class MyOperation : Operation
    {
        public static Operation RecordReward => new MyOperation(nameof(RecordReward));

        public static Operation AppDataRetrieval => new MyOperation(nameof(AppDataRetrieval));

        private MyOperation(string name) : base(name) { }
    }
}
