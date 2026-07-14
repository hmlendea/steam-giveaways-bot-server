using NUnit.Framework;

using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.UnitTests.Service.Models
{
    [TestFixture]
    public class RewardTests
    {
        // ── GiveawayUrl ───────────────────────────────────────────────────────

        [TestCase("steamgifts")]
        [TestCase("STEAMGIFTS")]
        [TestCase("SteamGifts")]
        [TestCase("SteamGIFTS")]
        [TestCase("steamGIFTS")]
        public void GivenSteamGiftsProvider_WhenGettingGiveawayUrl_ThenReturnsSteamGiftsUrl(
            string provider)
        {
            Reward reward = BuildRewardWithProvider(provider);

            Assert.That(
                reward.GiveawayUrl,
                Is.EqualTo("https://steamgifts.com/giveaway/gYeA3/ga/"));
        }

        [Test]
        public void GivenUnknownProvider_WhenGettingGiveawayUrl_ThenReturnsUnknownFormatString()
        {
            Reward reward = BuildRewardWithProvider("NucilandiaPlatform");

            Assert.That(
                reward.GiveawayUrl,
                Is.EqualTo("[UNKNOWN] Provider=NucilandiaPlatform, Id=gYeA3"));
        }

        [Test]
        public void GivenEmptyProvider_WhenGettingGiveawayUrl_ThenReturnsUnknownFormatString()
        {
            Reward reward = BuildRewardWithProvider(string.Empty);

            Assert.That(
                reward.GiveawayUrl,
                Is.EqualTo("[UNKNOWN] Provider=, Id=gYeA3"));
        }

        [Test]
        public void GivenAnotherUnknownProvider_WhenGettingGiveawayUrl_ThenPreservesOriginalCasingInOutput()
        {
            Reward reward = BuildRewardWithProvider("IndieGala");

            Assert.That(reward.GiveawayUrl, Does.Contain("Provider=IndieGala"));
        }

        // ── IsKeySteamCode ────────────────────────────────────────────────────

        [TestCase("ABCDE")]
        [TestCase("ABCDE-FGHIJ")]
        [TestCase("ABCDE-FGHIJ-KLMNO")]
        [TestCase("ABCDE-FGHIJ-KLMNO-PQRST")]
        [TestCase("ABCDE-FGHIJ-KLMNO-PQRST-UVWXY")]
        [TestCase("12345-67890-ABCDE")]
        [TestCase("00000-11111-22222-33333-44444")]
        [TestCase("A1B2C-D3E4F-G5H6I")]
        public void GivenValidSteamKeyFormat_WhenCheckingIsKeySteamCode_ThenReturnsTrue(
            string activationKey)
        {
            Reward reward = BuildRewardWithKey(activationKey);

            Assert.That(reward.IsKeySteamCode);
        }

        [TestCase("abcde-fghij-klmno")]
        [TestCase("Abcde-Fghij-Klmno")]
        [TestCase("https://store.steampowered.com/account/registerkey?key=ABCDE")]
        [TestCase("ABCDE-FGHIJ-")]
        [TestCase("-ABCDE-FGHIJ")]
        [TestCase("ABCD-EFGHI-JKLMN")]
        [TestCase("ABCDEF-GHIJKL-MNOPQR")]
        [TestCase("ABCDE FGHIJ KLMNO")]
        [TestCase("ABCDE_FGHIJ_KLMNO")]
        [TestCase("")]
        public void GivenInvalidSteamKeyFormat_WhenCheckingIsKeySteamCode_ThenReturnsFalse(
            string activationKey)
        {
            Reward reward = BuildRewardWithKey(activationKey);

            Assert.That(reward.IsKeySteamCode, Is.False);
        }

        // ── ActivationLink ────────────────────────────────────────────────────

        [Test]
        public void GivenThreeGroupSteamCode_WhenGettingActivationLink_ThenReturnsSteamActivationUrl()
        {
            Reward reward = BuildRewardWithKey("ABCDE-FGHIJ-KLMNO");

            Assert.That(
                reward.ActivationLink,
                Is.EqualTo(
                    "https://store.steampowered.com/account/registerkey?key=ABCDE-FGHIJ-KLMNO"));
        }

        [Test]
        public void GivenFiveGroupSteamCode_WhenGettingActivationLink_ThenReturnsSteamActivationUrl()
        {
            Reward reward = BuildRewardWithKey("ABCDE-FGHIJ-KLMNO-PQRST-UVWXY");

            Assert.That(
                reward.ActivationLink,
                Is.EqualTo(
                    "https://store.steampowered.com/account/registerkey"
                    + "?key=ABCDE-FGHIJ-KLMNO-PQRST-UVWXY"));
        }

        [Test]
        public void GivenHttpUrlKey_WhenGettingActivationLink_ThenReturnsUrlDirectly()
        {
            Reward reward = BuildRewardWithKey("http://example.com/key");

            Assert.That(reward.ActivationLink, Is.EqualTo("http://example.com/key"));
        }

        [Test]
        public void GivenHttpsUrlKey_WhenGettingActivationLink_ThenReturnsUrlDirectly()
        {
            Reward reward = BuildRewardWithKey("https://giveaway.nucilandia.ro/claim?code=XYZ");

            Assert.That(
                reward.ActivationLink,
                Is.EqualTo("https://giveaway.nucilandia.ro/claim?code=XYZ"));
        }

        [Test]
        public void GivenNonSteamNonUrlKey_WhenGettingActivationLink_ThenReturnsUnknown()
        {
            Reward reward = BuildRewardWithKey("some-random-platform-code-not-steam");

            Assert.That(reward.ActivationLink, Is.EqualTo("UNKNOWN"));
        }

        [Test]
        public void GivenEmptyKey_WhenGettingActivationLink_ThenReturnsUnknown()
        {
            Reward reward = BuildRewardWithKey(string.Empty);

            Assert.That(reward.ActivationLink, Is.EqualTo("UNKNOWN"));
        }

        [Test]
        public void GivenLowercaseKey_WhenGettingActivationLink_ThenReturnsUnknown()
        {
            Reward reward = BuildRewardWithKey("abcde-fghij-klmno");

            Assert.That(reward.ActivationLink, Is.EqualTo("UNKNOWN"));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        static Reward BuildRewardWithProvider(string giveawaysProvider) => new()
        {
            Id = "test-reward-id",
            GiveawaysProvider = giveawaysProvider,
            GiveawayId = "gYeA3",
            ActivationKey = "ABCDE-FGHIJ-KLMNO",
            SteamApp = new SteamApp { Id = "873" }
        };

        static Reward BuildRewardWithKey(string activationKey) => new()
        {
            Id = "test-reward-id",
            GiveawaysProvider = "SteamGifts",
            GiveawayId = "gYeA3",
            ActivationKey = activationKey,
            SteamApp = new SteamApp { Id = "873" }
        };
    }
}
