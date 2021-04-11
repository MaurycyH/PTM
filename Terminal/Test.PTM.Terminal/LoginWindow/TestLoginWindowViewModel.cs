using Microsoft.VisualStudio.TestTools.UnitTesting;
using PTM.Terminal;
using PTM.Terminal.LoginWindow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test.PTM.Terminal.LoginWindow
{
    /// <summary>
    /// Testuje <see cref="LoginWindowViewModel"/>
    /// </summary>
    [TestClass]
    public class TestLoginWindowViewModel
    {
        /// <summary>
        /// Sprawdza, czy po zalogwaniu przez google otwiera się główne okno terminala
        /// </summary>
        [TestMethod]
        public async Task AuthenticateWithGoogleAsync()
        {
            // Arrange
            ITerminalContext context = new TestTerminalContext();
            LoginWindowViewModel viewModel = new LoginWindowViewModel(context);

            // Act

            // Assert

        }

        /// <summary>
        /// Sprawdza, czy po zalogwaniu przez google otwiera się główne okno terminala
        /// </summary>
        [TestMethod]
        public async Task AuthenticateWithMicrosoftAsync()
        {
            // Arrange
            ITerminalContext context = new TestTerminalContext();
            LoginWindowViewModel viewModel = new LoginWindowViewModel(context);

            // Act

            // Assert

        }

    }
}
