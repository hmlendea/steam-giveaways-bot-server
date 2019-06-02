using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using SteamGiveawaysBot.Server.Core.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.DataAccess.Repositories
{
    public sealed class RewardRepository : IRewardRepository
    {
        readonly ApplicationSettings settings;

        public RewardRepository(ApplicationSettings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<RewardEntity> GetAll()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<RewardEntity>));

            using (TextReader reader = new StreamReader(settings.RewardsStorePath))
            {
                return (IEnumerable<RewardEntity>)serializer.Deserialize(reader);
            }
        }

        public void Add(RewardEntity entity)
        {
            IList<RewardEntity> entities = GetAll().ToList();

            if (entities.Any(x =>
                x.GiveawaysProvider == entity.GiveawaysProvider &&
                x.GiveawayId == entity.GiveawayId &&
                x.SteamUsername == entity.SteamUsername))
            {
                return;
            }

            entities.Add(entity);
            SaveAll(entities);
        }

        void SaveAll(IEnumerable<RewardEntity> entities)
        {
            FileStream fs = new FileStream(settings.RewardsStorePath, FileMode.Create, FileAccess.Write);

            using (StreamWriter sw = new StreamWriter(fs))
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<RewardEntity>));
                xs.Serialize(sw, entities);
            }
        }
    }
}
