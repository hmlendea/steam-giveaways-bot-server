using System;

using NuciDAL.IO;
using NuciDAL.Repositories;

using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public sealed class UserRepository : XmlRepository<UserEntity>, IUserRepository
    {
        public UserRepository(ApplicationSettings settings)
            : base(settings.UserStorePath)
        {
        }

        public override void Update(UserEntity user)
        {
            LoadEntitiesIfNeeded();

            UserEntity userEntityToUpdate = Get(user.Id);

            if (userEntityToUpdate == null)
            {
                throw new EntityNotFoundException(user.Id, nameof(UserEntity));
            }

            userEntityToUpdate.Username = user.Username;
            userEntityToUpdate.SharedSecretKey = user.SharedSecretKey;
            userEntityToUpdate.AssignedSteamAccount = user.AssignedSteamAccount;
            
            XmlFile.SaveEntities(Entities.Values);
        }
    }
}
