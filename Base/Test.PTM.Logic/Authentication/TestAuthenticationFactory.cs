using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Logic.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.PTM.Logic.Authentication
{
    /// <summary>
    /// Testuje <see cref="AuthenticationFactory"/>
    /// </summary>
    [TestClass]
    public class TestAuthenticationFactory
    {
        /// <summary>
        /// Przy wybranym providerze Google spodziewamy się typu <see cref="GoogleAuthentication"/>
        /// </summary>
        [TestMethod]
        public void CreateProvider_OnGoogle_ReturnsGoogleAuthentication()
        {
            // ARRANGE
            AuthenticationFactory factory = new AuthenticationFactory();

            // ACT
            IOAuthProvider provider = factory.CreateProvider(AuthenticationProvider.Google);

            // ASSERT
            provider.Should().BeOfType(typeof(GoogleAuthentication));
        }

        /// <summary>
        /// Przy wybranym providerze Microsoft spodziewamy się typu <see cref="MicrosoftAuthentication"/>
        /// </summary>
        [TestMethod]
        public void CreateProvider_OnMicrosoft_ReturnsMicrosoftAuthentication()
        {
            // TODO Implementacja
        }
    }
}
