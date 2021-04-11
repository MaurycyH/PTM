using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Logic;
using PTM.Logic.Authentication;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.PTM.Logic.Authentication
{
    /// <summary>
    /// Test funkcjonalności <see cref="AuthenticationProcessor"/>
    /// </summary>
    [TestClass]
    public class TestAuthenticationProcessor
    {
        /// <summary>
        /// Testuje autentykacje przez Google przy prawidłowym tokenie.
        /// </summary>
        [TestMethod]
        public async Task AuthenticateAsync_OnGoogleRefreshToken_SuccessfulAuthentication()
        {
            // ARRANGE
            AuthenticationProcessor processor = new AuthenticationProcessor();
            CredentialsManager manager = new CredentialsManager(AuthenticationProvider.Google);
            manager.SaveToken("*TOKEN*");

            // ACT
            UserPublic account = await processor.AuthenticateAsync(AuthenticationProvider.Google, CancellationToken.None);

            // ASSERT
            account.Should().NotBeNull();

            manager.DeleteToken();
        }

        /// <summary>
        /// Testuje autentykacje przez Microsoft przy prawidłowym tokenie.
        /// </summary>
        [TestMethod]
        public async Task AuthenticateAsync_OnMicrosoftRefreshToken_SuccessfulAuthentication()
        {
            // TODO implementacja
        }
    }
}
