using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Logic.Authentication;
using PTM.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.PTM.Logic.Authentication
{
    /// <summary>
    /// Sprawdza, czy nazwy w <see cref="AuthenticationProvider"/> są właściwe. 
    /// Test jest o tyle ważny, że przy zmianie nazw mogą pojawić się rozjady w <see cref="CredentialManager"/>
    /// </summary>
    [TestClass]
    public class TestAuthenticationProvider
    {
        /// <summary>
        /// Sprawdza, czy nazwa dla Google jest prawidłowa.
        /// </summary>
        [TestMethod]
        public void AuthenticationProvider_GoogleGetDescription_NameIsCorrect()
        {
            // ARRANGE
            AuthenticationProvider provider = AuthenticationProvider.Google;

            // ACT
            string providerName = provider.GetDescription();

            // ASSERT
            providerName.Should().Be("PTM.Oauth.Google");
        }

        /// <summary>
        /// Sprawdza, czy nazwa dla Microsoft jest prawidłowa.
        /// </summary>
        [TestMethod]
        public void AuthenticationProvider_MicrosoftGetDescription_NameIsCorrect()
        {
            // ARRANGE
            AuthenticationProvider provider = AuthenticationProvider.Microsoft;

            // ACT
            string providerName = provider.GetDescription();

            // ASSERT
            providerName.Should().Be("PTM.Oauth.Microsoft");
        }
    }
}
