using NUnit.Framework;

using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.UnitTests.Service.Models
{
    [TestFixture]
    public class SteamAppTests
    {
        // ── StoreUrl ──────────────────────────────────────────────────────────

        [Test]
        public void GivenNumericAppId_WhenGettingStoreUrl_ThenReturnsCorrectSteamStoreUrl()
        {
            SteamApp app = new() { Id = "873" };

            Assert.That(
                app.StoreUrl,
                Is.EqualTo("https://store.steampowered.com/app/873"));
        }

        [Test]
        public void GivenAnotherNumericAppId_WhenGettingStoreUrl_ThenReturnsCorrectSteamStoreUrl()
        {
            SteamApp app = new() { Id = "613" };

            Assert.That(
                app.StoreUrl,
                Is.EqualTo("https://store.steampowered.com/app/613"));
        }

        [Test]
        public void GivenAppId_WhenGettingStoreUrl_ThenUrlContainsAppId()
        {
            SteamApp app = new() { Id = "730" };

            Assert.That(app.StoreUrl, Does.Contain("730"));
        }

        [Test]
        public void GivenAppId_WhenGettingStoreUrl_ThenUrlStartsWithSteamStoreBase()
        {
            SteamApp app = new() { Id = "12345" };

            Assert.That(
                app.StoreUrl,
                Does.StartWith("https://store.steampowered.com/app/"));
        }

        [Test]
        public void GivenTwoDifferentApps_WhenGettingStoreUrls_ThenUrlsAreDifferent()
        {
            SteamApp firstApp = new() { Id = "873" };
            SteamApp secondApp = new() { Id = "613" };

            Assert.That(firstApp.StoreUrl, Is.Not.EqualTo(secondApp.StoreUrl));
        }
    }
}
