using System.Collections.Generic;
using System.IO;
using System.Linq;

using SteamGiveawaysBot.Server.Core.Configuration;
using SteamGiveawaysBot.Server.DataAccess.Exceptions;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        const char CsvSeparator = ',';

        readonly ApplicationConfiguration configuration;

        public UserRepository(ApplicationConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<UserEntity> GetAll()
        {
            IEnumerable<string> lines = File.ReadAllLines(configuration.UserStorePath);
            IList<UserEntity> users = new List<UserEntity>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
    
                UserEntity user = ReadUserEntity(line);
                users.Add(user);
            }

            return users;
        }    

        public UserEntity Get(string username)
        {
            IEnumerable<UserEntity> users = GetAll();
            UserEntity user = users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                throw new EntityNotFoundException(username, nameof(UserEntity));
            }

            return user;
        }

        public void Update(UserEntity user)
        {
            IEnumerable<UserEntity> users = GetAll();
            UserEntity oldUser = users.FirstOrDefault(x => x.Username == user.Username);

            if (oldUser == null)
            {
                throw new EntityNotFoundException(user.Username, nameof(UserEntity));
            }

            oldUser.SharedSecretKey = user.SharedSecretKey;
            oldUser.AssignedSteamAccount = user.AssignedSteamAccount;

            IEnumerable<string> csvLines = users.Select(x => $"{x.Username},{x.SharedSecretKey},{x.AssignedSteamAccount}");
            File.WriteAllLines(configuration.UserStorePath, csvLines);
        }

        public static UserEntity ReadUserEntity(string csvLine)
        {
            string[] fields = csvLine.Split(CsvSeparator);

            UserEntity user = new UserEntity();
            user.Username = fields[0];
            user.SharedSecretKey = fields[1];
            user.AssignedSteamAccount = fields[2];

            return user;
        }
    }
}
