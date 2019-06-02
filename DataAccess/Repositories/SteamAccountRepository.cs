using System.Collections.Generic;
using System.IO;
using System.Linq;

using SteamGiveawaysBot.Server.Core.Configuration;
using SteamGiveawaysBot.Server.DataAccess.Exceptions;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public sealed class SteamAccountRepository : ISteamAccountRepository
    {
        const char CsvSeparator = ',';

        readonly ApplicationConfiguration configuration;

        public SteamAccountRepository(ApplicationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<SteamAccountEntity> GetAll()
        {
            IEnumerable<string> lines = File.ReadAllLines(configuration.SteamAccountStorePath);
            IList<SteamAccountEntity> steamAccounts = new List<SteamAccountEntity>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
    
                SteamAccountEntity steamAccount = ReadSteamAccountEntity(line);
                steamAccounts.Add(steamAccount);
            }

            return steamAccounts;
        }    

        public SteamAccountEntity Get(string username)
        {
            IEnumerable<SteamAccountEntity> steamAccounts = GetAll();
            SteamAccountEntity steamAccount = steamAccounts.FirstOrDefault(x => x.Username == username);

            if (steamAccount == null)
            {
                throw new EntityNotFoundException(username, nameof(SteamAccountEntity));
            }

            return steamAccount;
        }

        public static SteamAccountEntity ReadSteamAccountEntity(string csvLine)
        {
            string[] fields = csvLine.Split(CsvSeparator);

            SteamAccountEntity steamAccount = new SteamAccountEntity();
            steamAccount.Username = fields[0];
            steamAccount.Password = fields[1];
            steamAccount.IsSteamGiftsSuspended = bool.Parse(fields[2]);

            return steamAccount;
        }
    }
}
