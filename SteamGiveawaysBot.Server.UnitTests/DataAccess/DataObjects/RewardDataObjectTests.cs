using NUnit.Framework;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server.UnitTests.DataAccess.DataObjects
{
    [TestFixture]
    public class RewardDataObjectTests
    {
        // ── SteamAppUrl ───────────────────────────────────────────────────────

        [Test]
        public void GivenNumericSteamAppId_WhenGettingSteamAppUrl_ThenReturnsCorrectSteamStoreUrl()
        {
            RewardDataObject entity = new() { SteamAppId = "873" };

            Assert.That(
                entity.SteamAppUrl,
                Is.EqualTo("https://store.steampowered.com/app/873"));
        }

        [Test]
        public void GivenAnotherSteamAppId_WhenGettingSteamAppUrl_ThenReturnsCorrectSteamStoreUrl()
        {
            RewardDataObject entity = new() { SteamAppId = "613" };

            Assert.That(
                entity.SteamAppUrl,
                Is.EqualTo("https://store.steampowered.com/app/613"));
        }

        [Test]
        public void GivenSteamAppId_WhenGettingSteamAppUrl_ThenUrlStartsWithSteamStoreBase()
        {
            RewardDataObject entity = new() { SteamAppId = "12345" };

            Assert.That(
                entity.SteamAppUrl,
                Does.StartWith("https://store.steampowered.com/app/"));
        }

        [Test]
        public void GivenSteamAppId_WhenGettingSteamAppUrl_ThenUrlContainsAppId()
        {
            RewardDataObject entity = new() { SteamAppId = "730" };

            Assert.That(entity.SteamAppUrl, Does.Contain("730"));
        }

        [Test]
        public void GivenTwoDifferentSteamAppIds_WhenGettingSteamAppUrls_ThenUrlsAreDifferent()
        {
            RewardDataObject firstEntity = new() { SteamAppId = "873" };
            RewardDataObject secondEntity = new() { SteamAppId = "613" };

            Assert.That(firstEntity.SteamAppUrl, Is.Not.EqualTo(secondEntity.SteamAppUrl));
        }
    }
}
