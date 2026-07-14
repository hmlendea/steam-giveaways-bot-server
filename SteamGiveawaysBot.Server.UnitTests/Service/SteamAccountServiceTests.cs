using System.Collections.Generic;
using System.Security.Authentication;

using Moq;
using NuciDAL.Repositories;
using NuciSecurity.HMAC;
using NUnit.Framework;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Responses;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.UnitTests.Service
{
    [TestFixture]
    public class SteamAccountServiceTests
    {
        Mock<IFileRepository<UserEntity>> mockUserRepository;
        Mock<IFileRepository<SteamAccountEntity>> mockSteamAccountRepository;
        SteamAccountService steamAccountService;

        private static string TestUsername => "IlarionPintilie";
        private static string TestSharedSecretKey => "TestSharedSecretKey613";
        private static string TestSteamAccountId => "SteamAcc1";
        private static string TestSteamAccountPassword => "P@ssw0rd!";
        private static string TestSuspendedAccountId => "SuspendedSteamAcc";
        private static string TestFreeAccountId => "FreeSteamAcc";
        private static string TestFreeAccountPassword => "SecurePa$$phrase!";
        private static string TestSteamGiftsProvider => "SteamGifts";
        private static string TestOtherProvider => "IndieGala";
        private static string TestTimestamp => "2012-09-05T00:00:00.0000000+00:00";

        [SetUp]
        public void SetUp()
        {
            mockUserRepository = new Mock<IFileRepository<UserEntity>>();
            mockSteamAccountRepository = new Mock<IFileRepository<SteamAccountEntity>>();

            steamAccountService = new SteamAccountService(
                mockUserRepository.Object,
                mockSteamAccountRepository.Object);
        }

        // ── GetAccount — authentication ───────────────────────────────────────

        [Test]
        public void GivenInvalidHmacToken_WhenGettingAccount_ThenThrowsAuthenticationException()
        {
            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(BuildUserEntityWithAssignedAccount(TestSteamAccountId));

            GetSteamAccountRequest request = BuildGetAccountRequest(
                TestSteamGiftsProvider,
                "completely-invalid-hmac-token");

            Assert.Throws<AuthenticationException>(
                () => steamAccountService.GetAccount(request));
        }

        [Test]
        public void GivenInvalidHmacToken_WhenGettingAccount_ThenDoesNotQuerySteamAccountRepository()
        {
            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(BuildUserEntityWithAssignedAccount(TestSteamAccountId));

            GetSteamAccountRequest request = BuildGetAccountRequest(
                TestSteamGiftsProvider,
                "completely-invalid-hmac-token");

            try
            {
                steamAccountService.GetAccount(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockSteamAccountRepository.Verify(
                repository => repository.Get(It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void GivenEmptyHmacToken_WhenGettingAccount_ThenThrowsAuthenticationException()
        {
            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(BuildUserEntityWithAssignedAccount(TestSteamAccountId));

            GetSteamAccountRequest request = BuildGetAccountRequest(
                TestSteamGiftsProvider,
                string.Empty);

            Assert.Throws<AuthenticationException>(
                () => steamAccountService.GetAccount(request));
        }

        [Test]
        public void GivenEmptyHmacToken_WhenGettingAccount_ThenDoesNotQuerySteamAccountRepository()
        {
            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(BuildUserEntityWithAssignedAccount(TestSteamAccountId));

            GetSteamAccountRequest request = BuildGetAccountRequest(
                TestSteamGiftsProvider,
                string.Empty);

            try
            {
                steamAccountService.GetAccount(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockSteamAccountRepository.Verify(
                repository => repository.Get(It.IsAny<string>()),
                Times.Never);
        }

        // ── GetAccount — no assigned account ─────────────────────────────────

        [Test]
        public void GivenUserWithNoAssignedAccount_WhenGettingAccount_ThenReturnsNewAccountUsername()
        {
            SetUpUserWithNoAssignedAccount();
            SetUpSingleAvailableAccount(TestSteamAccountId, TestSteamAccountPassword, isSuspended: false);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Username, Is.EqualTo(TestSteamAccountId));
        }

        [Test]
        public void GivenUserWithNoAssignedAccount_WhenGettingAccount_ThenReturnsNewAccountPassword()
        {
            SetUpUserWithNoAssignedAccount();
            SetUpSingleAvailableAccount(TestSteamAccountId, TestSteamAccountPassword, isSuspended: false);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Password, Is.EqualTo(TestSteamAccountPassword));
        }

        [Test]
        public void GivenUserWithNoAssignedAccount_WhenGettingAccount_ThenUpdatesUserWithAssignedAccount()
        {
            SetUpUserWithNoAssignedAccount();
            SetUpSingleAvailableAccount(TestSteamAccountId, TestSteamAccountPassword, isSuspended: false);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.Update(
                    It.Is<UserEntity>(entity =>
                        entity.AssignedSteamAccount == TestSteamAccountId)),
                Times.Once);
        }

        [Test]
        public void GivenUserWithNoAssignedAccount_WhenGettingAccount_ThenSavesUserChanges()
        {
            SetUpUserWithNoAssignedAccount();
            SetUpSingleAvailableAccount(TestSteamAccountId, TestSteamAccountPassword, isSuspended: false);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Once);
        }

        [Test]
        public void GivenUserWithNoAssignedAccount_WhenGettingAccount_ThenResponseContainsHmacToken()
        {
            SetUpUserWithNoAssignedAccount();
            SetUpSingleAvailableAccount(TestSteamAccountId, TestSteamAccountPassword, isSuspended: false);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.HmacToken, Is.Not.Null);
            Assert.That(response.HmacToken, Is.Not.Empty);
        }

        // ── GetAccount — existing valid assigned account ──────────────────────

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForSteamGifts_ThenReturnsExistingAccountUsername()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpNonSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Username, Is.EqualTo(TestSteamAccountId));
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForSteamGifts_ThenReturnsExistingAccountPassword()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpNonSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Password, Is.EqualTo(TestSteamAccountPassword));
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForSteamGifts_ThenDoesNotUpdateUserAssignment()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpNonSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.Update(It.IsAny<UserEntity>()),
                Times.Never);
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForSteamGifts_ThenDoesNotSaveUserChanges()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpNonSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Never);
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForOtherProvider_ThenReturnsExistingAccount()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestOtherProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Username, Is.EqualTo(TestSteamAccountId));
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForOtherProvider_ThenReturnsExistingAccountPassword()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestOtherProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Password, Is.EqualTo(TestSteamAccountPassword));
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForOtherProvider_ThenDoesNotSaveUserChanges()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestOtherProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Never);
        }

        [Test]
        public void GivenUserWithValidAssignedAccount_WhenGettingAccountForOtherProvider_ThenDoesNotUpdateUserAssignment()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestOtherProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.Update(It.IsAny<UserEntity>()),
                Times.Never);
        }

        // ── GetAccount — SteamGifts suspended account ─────────────────────────

        [Test]
        public void GivenUserWithSuspendedAccountForSteamGifts_WhenGettingAccount_ThenReturnsNewAccountUsername()
        {
            SetUpUserWithAssignedAccount(TestSuspendedAccountId);
            SetUpSuspendedAndFreeAccounts();

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Username, Is.EqualTo(TestFreeAccountId));
        }

        [Test]
        public void GivenUserWithSuspendedAccountForSteamGifts_WhenGettingAccount_ThenReturnsNewAccountPassword()
        {
            SetUpUserWithAssignedAccount(TestSuspendedAccountId);
            SetUpSuspendedAndFreeAccounts();

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Password, Is.EqualTo(TestFreeAccountPassword));
        }

        [Test]
        public void GivenUserWithSuspendedAccountForSteamGifts_WhenGettingAccount_ThenUpdatesUserWithNewAccount()
        {
            SetUpUserWithAssignedAccount(TestSuspendedAccountId);
            SetUpSuspendedAndFreeAccounts();

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.Update(
                    It.Is<UserEntity>(entity =>
                        entity.AssignedSteamAccount == TestFreeAccountId)),
                Times.Once);
        }

        [Test]
        public void GivenUserWithSuspendedAccountForSteamGifts_WhenGettingAccount_ThenSavesUserChanges()
        {
            SetUpUserWithAssignedAccount(TestSuspendedAccountId);
            SetUpSuspendedAndFreeAccounts();

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Once);
        }

        [Test]
        public void GivenUserWithSuspendedAccountForNonSteamGifts_WhenGettingAccount_ThenReturnsExistingAccountUsername()
        {
            SetUpUserWithAssignedAccount(TestSuspendedAccountId);
            SetUpSuspendedSteamAccount(TestSuspendedAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestOtherProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.Username, Is.EqualTo(TestSuspendedAccountId));
        }

        [Test]
        public void GivenUserWithSuspendedAccountForNonSteamGifts_WhenGettingAccount_ThenDoesNotReassignAccount()
        {
            SetUpUserWithAssignedAccount(TestSuspendedAccountId);
            SetUpSuspendedSteamAccount(TestSuspendedAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestOtherProvider);

            steamAccountService.GetAccount(request);

            mockUserRepository.Verify(
                repository => repository.Update(It.IsAny<UserEntity>()),
                Times.Never);
        }

        [Test]
        public void GivenValidRequest_WhenGettingAccount_ThenResponseContainsHmacToken()
        {
            SetUpUserWithAssignedAccount(TestSteamAccountId);
            SetUpNonSuspendedSteamAccount(TestSteamAccountId, TestSteamAccountPassword);

            GetSteamAccountRequest request = BuildValidGetAccountRequest(TestSteamGiftsProvider);

            GetSteamAccountResponse response = steamAccountService.GetAccount(request);

            Assert.That(response.HmacToken, Is.Not.Null);
            Assert.That(response.HmacToken, Is.Not.Empty);
        }

        // ── Setup helpers ─────────────────────────────────────────────────────

        void SetUpUserWithNoAssignedAccount()
        {
            UserEntity userEntity = BuildUserEntityWithAssignedAccount(null);

            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(userEntity);

            mockUserRepository
                .Setup(repository => repository.GetAll())
                .Returns([userEntity]);
        }

        void SetUpUserWithAssignedAccount(string assignedSteamAccountId)
        {
            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(BuildUserEntityWithAssignedAccount(assignedSteamAccountId));
        }

        void SetUpNonSuspendedSteamAccount(string accountId, string password)
        {
            mockSteamAccountRepository
                .Setup(repository => repository.Get(accountId))
                .Returns(BuildSteamAccountEntity(accountId, password, isSuspended: false));
        }

        void SetUpSuspendedSteamAccount(string accountId, string password)
        {
            mockSteamAccountRepository
                .Setup(repository => repository.Get(accountId))
                .Returns(BuildSteamAccountEntity(accountId, password, isSuspended: true));
        }

        void SetUpSingleAvailableAccount(
            string accountId,
            string password,
            bool isSuspended)
        {
            SteamAccountEntity accountEntity = BuildSteamAccountEntity(accountId, password, isSuspended);

            mockSteamAccountRepository
                .Setup(repository => repository.GetAll())
                .Returns([accountEntity]);
        }

        void SetUpSuspendedAndFreeAccounts()
        {
            SteamAccountEntity suspendedAccountEntity = BuildSteamAccountEntity(
                TestSuspendedAccountId,
                TestSteamAccountPassword,
                isSuspended: true);

            SteamAccountEntity freeAccountEntity = BuildSteamAccountEntity(
                TestFreeAccountId,
                TestFreeAccountPassword,
                isSuspended: false);

            mockSteamAccountRepository
                .Setup(repository => repository.Get(TestSuspendedAccountId))
                .Returns(suspendedAccountEntity);

            mockSteamAccountRepository
                .Setup(repository => repository.GetAll())
                .Returns([suspendedAccountEntity, freeAccountEntity]);

            UserEntity userWithSuspendedAccount = BuildUserEntityWithAssignedAccount(
                TestSuspendedAccountId);

            mockUserRepository
                .Setup(repository => repository.GetAll())
                .Returns([userWithSuspendedAccount]);
        }

        // ── Build helpers ─────────────────────────────────────────────────────

        GetSteamAccountRequest BuildValidGetAccountRequest(string giveawaysProvider)
        {
            GetSteamAccountRequest request = new()
            {
                Username = TestUsername,
                GiveawaysProvider = giveawaysProvider
            };

            request.HmacToken = HmacEncoder.GenerateToken(request, TestSharedSecretKey);

            return request;
        }

        static GetSteamAccountRequest BuildGetAccountRequest(
            string giveawaysProvider,
            string hmacToken) => new()
        {
            Username = TestUsername,
            GiveawaysProvider = giveawaysProvider,
            HmacToken = hmacToken
        };

        static UserEntity BuildUserEntityWithAssignedAccount(string assignedSteamAccount) => new()
        {
            Id = TestUsername,
            Username = TestUsername,
            SharedSecretKey = TestSharedSecretKey,
            AssignedSteamAccount = assignedSteamAccount,
            IpAddress = string.Empty,
            CreationTimestamp = TestTimestamp,
            LastUpdateTimestamp = TestTimestamp
        };

        static SteamAccountEntity BuildSteamAccountEntity(
            string accountId,
            string password,
            bool isSuspended) => new()
        {
            Id = accountId,
            Username = accountId,
            Password = password,
            IsSteamGiftsSuspended = isSuspended,
            CreationTimestamp = TestTimestamp,
            LastUpdateTimestamp = TestTimestamp
        };
    }
}
