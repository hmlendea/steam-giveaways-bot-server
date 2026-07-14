using System;
using System.Globalization;
using System.Security.Authentication;

using Moq;
using NuciDAL.Repositories;
using NuciSecurity.HMAC;
using NUnit.Framework;

using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Requests;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.UnitTests.Service
{
    [TestFixture]
    public class UserServiceTests
    {
        Mock<IFileRepository<UserEntity>> mockUserRepository;
        UserService userService;

        private static string TestUsername => "IlarionPintilie";
        private static string TestSharedSecretKey => "TestSharedSecretKey613";
        private static string TestIpAddress => "192.168.1.1";
        private static string TestAltIpAddress => "10.0.0.1";
        private static string TestTimestamp => "2012-09-05T00:00:00.0000000+00:00";

        [SetUp]
        public void SetUp()
        {
            mockUserRepository = new Mock<IFileRepository<UserEntity>>();
            mockUserRepository
                .Setup(repository => repository.Get(TestUsername))
                .Returns(BuildUserEntity());

            userService = new UserService(mockUserRepository.Object);
        }

        // ── SetIpAddress ──────────────────────────────────────────────────────

        [Test]
        public void GivenValidHmacToken_WhenSettingIpAddress_ThenCallsRepositoryUpdate()
        {
            SetIpAddressRequest request = BuildValidSetIpAddressRequest();

            userService.SetIpAddress(request);

            mockUserRepository.Verify(
                repository => repository.Update(It.IsAny<UserEntity>()),
                Times.Once);
        }

        [Test]
        public void GivenValidHmacToken_WhenSettingIpAddress_ThenSavesRepositoryChanges()
        {
            SetIpAddressRequest request = BuildValidSetIpAddressRequest();

            userService.SetIpAddress(request);

            mockUserRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Once);
        }

        [Test]
        public void GivenValidHmacToken_WhenSettingIpAddress_ThenUpdatesIpAddressOnEntity()
        {
            SetIpAddressRequest request = BuildValidSetIpAddressRequest();

            userService.SetIpAddress(request);

            mockUserRepository.Verify(
                repository => repository.Update(
                    It.Is<UserEntity>(entity => entity.IpAddress == TestIpAddress)),
                Times.Once);
        }

        [Test]
        public void GivenValidHmacToken_WhenSettingIpAddress_ThenUpdatesLastUpdateTimestampOnEntity()
        {
            SetIpAddressRequest request = BuildValidSetIpAddressRequest();
            DateTimeOffset before = DateTimeOffset.Now;

            userService.SetIpAddress(request);

            DateTimeOffset after = DateTimeOffset.Now;

            mockUserRepository.Verify(
                repository => repository.Update(
                    It.Is<UserEntity>(entity => IsTimestampWithinRange(
                        entity.LastUpdateTimestamp,
                        before,
                        after))),
                Times.Once);
        }

        [Test]
        public void GivenValidHmacToken_WhenSettingIpAddress_ThenPreservesUsernameOnEntity()
        {
            SetIpAddressRequest request = BuildValidSetIpAddressRequest();

            userService.SetIpAddress(request);

            mockUserRepository.Verify(
                repository => repository.Update(
                    It.Is<UserEntity>(entity => entity.Username == TestUsername)),
                Times.Once);
        }

        [Test]
        public void GivenInvalidHmacToken_WhenSettingIpAddress_ThenThrowsAuthenticationException()
        {
            SetIpAddressRequest request = BuildInvalidSetIpAddressRequest();

            Assert.Throws<AuthenticationException>(
                () => userService.SetIpAddress(request));
        }

        [Test]
        public void GivenInvalidHmacToken_WhenSettingIpAddress_ThenDoesNotCallRepositoryUpdate()
        {
            SetIpAddressRequest request = BuildInvalidSetIpAddressRequest();

            try
            {
                userService.SetIpAddress(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockUserRepository.Verify(
                repository => repository.Update(It.IsAny<UserEntity>()),
                Times.Never);
        }

        [Test]
        public void GivenInvalidHmacToken_WhenSettingIpAddress_ThenDoesNotSaveRepositoryChanges()
        {
            SetIpAddressRequest request = BuildInvalidSetIpAddressRequest();

            try
            {
                userService.SetIpAddress(request);
            }
            catch (AuthenticationException)
            {
                // Expected exception.
            }

            mockUserRepository.Verify(
                repository => repository.SaveChanges(),
                Times.Never);
        }

        [Test]
        public void GivenEmptyHmacToken_WhenSettingIpAddress_ThenThrowsAuthenticationException()
        {
            SetIpAddressRequest request = new()
            {
                Username = TestUsername,
                IpAddress = TestIpAddress,
                HmacToken = string.Empty
            };

            Assert.Throws<AuthenticationException>(
                () => userService.SetIpAddress(request));
        }

        [Test]
        public void GivenHmacTokenForDifferentIpAddress_WhenSettingIpAddress_ThenThrowsAuthenticationException()
        {
            SetIpAddressRequest tokenRequest = new()
            {
                Username = TestUsername,
                IpAddress = TestAltIpAddress
            };

            tokenRequest.HmacToken = HmacEncoder.GenerateToken(tokenRequest, TestSharedSecretKey);

            SetIpAddressRequest actualRequest = new()
            {
                Username = TestUsername,
                IpAddress = TestIpAddress,
                HmacToken = tokenRequest.HmacToken
            };

            Assert.Throws<AuthenticationException>(
                () => userService.SetIpAddress(actualRequest));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        static bool IsTimestampWithinRange(
            string timestamp,
            DateTimeOffset rangeStart,
            DateTimeOffset rangeEnd)
        {
            DateTimeOffset parsed = DateTimeOffset.Parse(
                timestamp,
                CultureInfo.InvariantCulture);

            return parsed >= rangeStart && parsed <= rangeEnd;
        }

        static SetIpAddressRequest BuildValidSetIpAddressRequest()
        {
            SetIpAddressRequest request = new()
            {
                Username = TestUsername,
                IpAddress = TestIpAddress
            };

            request.HmacToken = HmacEncoder.GenerateToken(request, TestSharedSecretKey);

            return request;
        }

        static SetIpAddressRequest BuildInvalidSetIpAddressRequest() => new()
        {
            Username = TestUsername,
            IpAddress = TestIpAddress,
            HmacToken = "completely-invalid-hmac-token"
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
    }
}
