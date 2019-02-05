using System.Collections.Generic;
using System.IO;
using System.Linq;

using SteamAccountDistributor.Core.Configuration;
using SteamAccountDistributor.DataAccess.Exceptions;
using SteamAccountDistributor.DataAccess.DataObjects;

namespace SteamAccountDistributor.DataAccess.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        const char CsvSeparator = ',';

        readonly SteamAccountDistributorConfiguration configuration;

        public UserRepository(SteamAccountDistributorConfiguration configuration)
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

            oldUser.Password = user.Password;
            oldUser.AssignedSteamAccount = user.AssignedSteamAccount;

            IEnumerable<string> csvLines = users.Select(x => $"{x.Username},{x.Password},{x.AssignedSteamAccount}");
            File.WriteAllLines(configuration.UserStorePath, csvLines);
        }

        public static UserEntity ReadUserEntity(string csvLine)
        {
            string[] fields = csvLine.Split(CsvSeparator);

            UserEntity user = new UserEntity();
            user.Username = fields[0];
            user.Password = fields[1];
            user.AssignedSteamAccount = fields[2];

            return user;
        }
    }
}
