using System.Security.Authentication;

using Moq;
using NuciDAL.Repositories;
using NuciLog.Core;
using NuciNotifications.Client;
using NuciSecurity.HMAC;
using NUnit.Framework;

using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.Client.Models;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.UnitTests.Service
{
    [TestFixture]
    public class RewardServiceTests
    {
        Mock<INuciNotificationsClient> mockNotificationsClient;
        Mock<IFileRepository<UserEntity>> mockUserRepository;
        Mock<IFileRepository<RewardEntity>> mockRewardRepository;
        Mock<IStorefrontDataRetriever> mockStorefrontDataRetriever;
        Mock<ILogger> mockLogger;
        NotificationSettings notificationSettings;
        RewardService rewardService;

        private static string TestUsername => "IlarionPintilie";
        private static string TestSharedSecretKey => "TestSharedSecretKey613";
        private static string TestSteamUsername => "solaire_of_astora";
        private static string TestGiveawayId => "gYeA3";
        private static string TestSteamAppId => "873";
        private static string TestActivationKey => "ABCDE-FGHIJ-KLMNO";
        private static string TestGiveawaysProvider => "SteamGifts";
        private static string TestEmailAddress => "ilarion.pintilie@nucilandia.ro";
        private static string TestSteamAppName => "Cornova Adventures";
        private static string TestTimestamp => "2012-09-05T00:00:00.0000000+00:00";

        [SetUp]
        public void SetUp()
        {
            mockNotificationsClient = new Mock<INuciNotificationsClient>();
            mockUserRepository = new Mock<IFileRepository<UserEntity>>();
            mockRewardRepository = new Mock<IFileRepository<RewardEntity>>();
            mockStorefrontDataRetriever = new Mock<IStorefrontDataRetriever>();
            mockLogger = new Mock<ILogger>();

            notificationSettings = new NotificationSettings
            {
                EmailAddress = TestEmailAddress
            };

            mockUserRepository
                .Setup(repository => repository.TryGet(TestUsername))
                .Returns(BuildUserEntity());

            mockStorefrontDataRetriever
                .Setup(retriever => retriever.GetAppData(TestSteamAppId))
                .Returns(BuildSteamAppEntity());

            rewardService = new RewardService(
                mockNotificationsClient.Object,
                mockUserRepository.Object,
                mockRewardRepository.Object,
                mockStorefrontDataRetriever.Object,
                notificationSettings,
                mockLogger.Object);
        }

        // ── RecordReward ──────────────────────────────────────────────────────

        [Test]
        public void GivenNonExistentUser_WhenRecordingReward_ThenThrowsAuthenticationException()
        {
            mockUserRepository
                .Setup(repository => repository.TryGet(TestUsername))
                .Returns((UserEntity)null);

            RecordRewardRequest request = BuildRecordRewardRequest("any-invalid-token");

            Assert.Throws<AuthenticationException>(
                () => rewardService.RecordReward(request));
        }

        [Test]
        public void GivenNonExistentUser_WhenRecordingReward_ThenDoesNotAddRewardToRepository()
        {
            mockUserRepository
                .Setup(repository => repository.TryGet(TestUsername))
                .Returns((UserEntity)null);

            RecordRewardRequest request = BuildRecordRewardRequest("any-invalid-token");

            try
            {
                rewardService.RecordReward(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockRewardRepository.Verify(
                repository => repository.Add(It.IsAny<RewardEntity>()),
                Times.Never);
        }

        [Test]
        public void GivenNonExistentUser_WhenRecordingReward_ThenDoesNotSendEmailNotification()
        {
            mockUserRepository
                .Setup(repository => repository.TryGet(TestUsername))
                .Returns((UserEntity)null);

            RecordRewardRequest request = BuildRecordRewardRequest("any-invalid-token");

            try
            {
                rewardService.RecordReward(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockNotificationsClient.Verify(
                client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void GivenInvalidHmacToken_WhenRecordingReward_ThenThrowsAuthenticationException()
        {
            RecordRewardRequest request = BuildRecordRewardRequest("completely-invalid-hmac-token");

            Assert.Throws<AuthenticationException>(
                () => rewardService.RecordReward(request));
        }

        [Test]
        public void GivenInvalidHmacToken_WhenRecordingReward_ThenDoesNotAddRewardToRepository()
        {
            RecordRewardRequest request = BuildRecordRewardRequest("completely-invalid-hmac-token");

            try
            {
                rewardService.RecordReward(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockRewardRepository.Verify(
                repository => repository.Add(It.IsAny<RewardEntity>()),
                Times.Never);
        }

        [Test]
        public void GivenInvalidHmacToken_WhenRecordingReward_ThenDoesNotSendEmailNotification()
        {
            RecordRewardRequest request = BuildRecordRewardRequest("completely-invalid-hmac-token");

            try
            {
                rewardService.RecordReward(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockNotificationsClient.Verify(
                client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenAddsRewardToRepository()
        {
            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            mockRewardRepository.Verify(
                repository => repository.Add(It.IsAny<RewardEntity>()),
                Times.Once);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenAddsRewardWithCorrectFields()
        {
            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            mockRewardRepository.Verify(
                repository => repository.Add(
                    It.Is<RewardEntity>(entity =>
                        entity.GiveawaysProvider == TestGiveawaysProvider &&
                        entity.GiveawayId == TestGiveawayId &&
                        entity.SteamUsername == TestSteamUsername &&
                        entity.ActivationKey == TestActivationKey &&
                        entity.SteamAppId == TestSteamAppId)),
                Times.Once);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenSavesRewardRepositoryChanges()
        {
            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            mockRewardRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Once);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenSendsEmailNotification()
        {
            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            mockNotificationsClient.Verify(
                client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenSendsEmailToConfiguredAddress()
        {
            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            mockNotificationsClient.Verify(
                client => client.SendEmail(
                    TestEmailAddress,
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenFetchesAppDataFromStorefront()
        {
            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            mockStorefrontDataRetriever.Verify(
                retriever => retriever.GetAppData(TestSteamAppId),
                Times.Once);
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenEmailBodyContainsSteamAppName()
        {
            string capturedBody = null;

            mockNotificationsClient
                .Setup(client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string>(
                    (recipient, subject, body) => capturedBody = body);

            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            Assert.That(capturedBody, Does.Contain(TestSteamAppName));
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenEmailBodyContainsActivationKey()
        {
            string capturedBody = null;

            mockNotificationsClient
                .Setup(client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string>(
                    (recipient, subject, body) => capturedBody = body);

            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            Assert.That(capturedBody, Does.Contain(TestActivationKey));
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenEmailBodyContainsSteamUsername()
        {
            string capturedBody = null;

            mockNotificationsClient
                .Setup(client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string>(
                    (recipient, subject, body) => capturedBody = body);

            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            Assert.That(capturedBody, Does.Contain(TestSteamUsername));
        }

        [Test]
        public void GivenValidRequest_WhenRecordingReward_ThenEmailSubjectIsSteamKeyWon()
        {
            string capturedSubject = null;

            mockNotificationsClient
                .Setup(client => client.SendEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Callback<string, string, string>(
                    (recipient, subject, body) => capturedSubject = subject);

            RecordRewardRequest request = BuildValidRecordRewardRequest();

            rewardService.RecordReward(request);

            Assert.That(capturedSubject, Is.EqualTo("Steam Key won!"));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        static RecordRewardRequest BuildValidRecordRewardRequest()
        {
            RecordRewardRequest request = new()
            {
                Username = TestUsername,
                GiveawaysProvider = TestGiveawaysProvider,
                GiveawayId = TestGiveawayId,
                SteamUsername = TestSteamUsername,
                SteamAppId = TestSteamAppId,
                ActivationKey = TestActivationKey
            };

            request.HmacToken = HmacEncoder.GenerateToken(request, TestSharedSecretKey);

            return request;
        }

        static RecordRewardRequest BuildRecordRewardRequest(string hmacToken) => new()
        {
            Username = TestUsername,
            GiveawaysProvider = TestGiveawaysProvider,
            GiveawayId = TestGiveawayId,
            SteamUsername = TestSteamUsername,
            SteamAppId = TestSteamAppId,
            ActivationKey = TestActivationKey,
            HmacToken = hmacToken
        };

        static UserEntity BuildUserEntity() => new()
        {
            Id = TestUsername,
            Username = TestUsername,
            SharedSecretKey = TestSharedSecretKey,
            AssignedSteamAccount = string.Empty,
            IpAddress = string.Empty,
            CreationTimestamp = TestTimestamp,
            LastUpdateTimestamp = TestTimestamp
        };

        static SteamAppEntity BuildSteamAppEntity() => new()
        {
            Id = TestSteamAppId,
            Name = TestSteamAppName
        };
    }
}
