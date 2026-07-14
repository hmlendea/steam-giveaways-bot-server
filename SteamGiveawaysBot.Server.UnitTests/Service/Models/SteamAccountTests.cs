using System;

using NUnit.Framework;

using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.UnitTests.Service.Models
{
    [TestFixture]
    public class SteamAccountTests
    {
        // ── Constructor ───────────────────────────────────────────────────────

        [Test]
        public void GivenNewInstance_WhenCreated_ThenCreationTimeIsNearNow()
        {
            DateTimeOffset before = DateTimeOffset.Now;
            SteamAccount account = new();
            DateTimeOffset after = DateTimeOffset.Now;

            Assert.That(account.CreationTime, Is.GreaterThanOrEqualTo(before));
            Assert.That(account.CreationTime, Is.LessThanOrEqualTo(after));
        }

        [Test]
        public void GivenNewInstance_WhenCreated_ThenLastUpdateTimeEqualsCreationTime()
        {
            SteamAccount account = new();

            Assert.That(account.LastUpdateTime, Is.EqualTo(account.CreationTime));
        }

        [Test]
        public void GivenNewInstance_WhenCreated_ThenIsSteamGiftsSuspendedDefaultsToFalse()
        {
            SteamAccount account = new();

            Assert.That(account.IsSteamGiftsSuspended, Is.False);
        }

        [Test]
        public void GivenTwoSeparateInstances_WhenCreated_ThenCreationTimesAreIndependent()
        {
            SteamAccount firstAccount = new();
            SteamAccount secondAccount = new();

            Assert.That(
                secondAccount.CreationTime,
                Is.GreaterThanOrEqualTo(firstAccount.CreationTime));
        }
    }
}
