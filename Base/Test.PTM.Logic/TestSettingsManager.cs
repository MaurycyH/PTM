using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Logic;
using PTM.Logic.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.PTM.Logic
{
    [TestClass]
    public class TestSettingsManager
    {
        /// <summary>
        /// Sprawdza czy provider google zapisuje się i wczytuje, po czym go usuwa
        /// </summary>
        [TestMethod]
        public void SaveProvider_SavesProvider_GoogleLoadsCorectly()
        {
            //Arrange
            AuthenticationProvider provider = AuthenticationProvider.Google;
            GoogleAuthentication authentication = new GoogleAuthentication();
            SettingsManager settingsManager = new SettingsManager();

            //Act
            settingsManager.SaveProvider(provider);

            //Assert
            Assert.AreEqual(settingsManager.LoadProvider(), provider.ToString());

            //Cleanup
            settingsManager.DeleteProvider();
        }

        /// <summary>
        /// Sprawdza czy provider Microsoft zapisuje się i wczytuje, po czym go usuwa
        /// </summary>
        public void SaveProvider_SavesProvider_MicrosoftLoadsCorectly()
        {
            //Arrange
            AuthenticationProvider provider = AuthenticationProvider.Microsoft;
            MicrosoftAuthentication authentication = new MicrosoftAuthentication();
            SettingsManager settingsManager = new SettingsManager();

            //Act
            settingsManager.SaveProvider(provider);

            //Assert
            Assert.AreEqual(settingsManager.LoadProvider(), provider.ToString());

            //Cleanup
            settingsManager.DeleteProvider();
        }
    }
}