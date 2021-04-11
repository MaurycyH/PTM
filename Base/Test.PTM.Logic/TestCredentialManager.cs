using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Logic.Authentication;
using PTM.Logic;
using System;
using System.Linq;

namespace Test.PTM.Logic
{
    [TestClass]
    public class TestCredentialManager
    {
        /// <summary>
        /// Sprawdza czy token zapisuje siê i wczytuje, po czym go usuwa
        /// </summary>
        [TestMethod]
        public void SaveToken_SavesCredentials_LoadsCorectly()
        {
            //Arrange
            string Token = "test";
            var Providers = Enum.GetValues(typeof(AuthenticationProvider));

            //Act, Assert
            foreach (AuthenticationProvider provider in Providers)
            {
                CredentialsManager CredentialsManager = new CredentialsManager(provider);
                CredentialsManager.SaveToken(Token);
                Assert.AreEqual(CredentialsManager.LoadToken(), Token);
                CredentialsManager.DeleteToken();
            }
        }

        /// <summary>
        /// Sprawdza czy token usuwa siê, po uprzednim zapisaniu i wczytaniu
        /// </summary>
        [TestMethod]
        public void DeleteToken_DeleteCredentials_AreDeleted()
        {
            //Arrange
            string Token = "test";
            var Providers = Enum.GetValues(typeof(AuthenticationProvider));

            //Act, Assert
            foreach (AuthenticationProvider provider in Providers)
            {
                CredentialsManager CredentialsManager = new CredentialsManager(provider);
                CredentialsManager.SaveToken(Token);
                CredentialsManager.DeleteToken();
                Assert.AreEqual(CredentialsManager.LoadToken(), string.Empty);
            }
        }
    }
}
