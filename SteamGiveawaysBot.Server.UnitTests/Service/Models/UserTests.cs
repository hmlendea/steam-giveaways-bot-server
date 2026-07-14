using System;

using NUnit.Framework;

using SteamGiveawaysBot.Server.Service.Models;

namespace SteamGiveawaysBot.Server.UnitTests.Service.Models
{
    [TestFixture]
    public class UserTests
    {
        // ── Constructor ───────────────────────────────────────────────────────

        [Test]
        public void GivenNewInstance_WhenCreated_ThenCreationTimeIsNearNow()
        {
            DateTimeOffset before = DateTimeOffset.Now;
            User user = new();
            DateTimeOffset after = DateTimeOffset.Now;

            Assert.That(user.CreationTime, Is.GreaterThanOrEqualTo(before));
            Assert.That(user.CreationTime, Is.LessThanOrEqualTo(after));
        }

        [Test]
        public void GivenNewInstance_WhenCreated_ThenLastUpdateTimeEqualsCreationTime()
        {
            User user = new();

            Assert.That(user.LastUpdateTime, Is.EqualTo(user.CreationTime));
        }

        [Test]
        public void GivenNewInstance_WhenCreated_ThenAssignedSteamAccountIsNull()
        {
            User user = new();

            Assert.That(user.AssignedSteamAccount, Is.Null);
        }

        [Test]
        public void GivenNewInstance_WhenCreated_ThenIpAddressIsNull()
        {
            User user = new();

            Assert.That(user.IpAddress, Is.Null);
        }

        [Test]
        public void GivenTwoSeparateInstances_WhenCreated_ThenCreationTimesAreIndependent()
        {
            User firstUser = new();
            User secondUser = new();

            Assert.That(
                secondUser.CreationTime,
                Is.GreaterThanOrEqualTo(firstUser.CreationTime));
        }
    }
}
