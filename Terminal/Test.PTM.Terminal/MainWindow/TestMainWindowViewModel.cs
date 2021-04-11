using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using PTM.Terminal.MainWindow;
using System.Windows.Input;
using Tesseract.Common.MVVM;
using PTM.Terminal.WindowHelpers;
using PTM.Terminal;
using System.Windows;
using FluentAssertions;
using System.Threading;
using FluentAssertions.Equivalency;
using System.Diagnostics;
using System.Threading.Tasks;
using PTM.PublicDataModel;

namespace Test.PTM.Terminal.MainWindow
{
    /// <summary>
    /// Testuje <see cref="MainWindowViewModel"/>
    /// </summary>
    [TestClass]
    public class TestMainWindowViewModel
    {
        /// <summary>
        /// Metoda testujaca zmiane thickness border z Maksymilizacji do Normal state
        /// </summary>

        [TestMethod]
        public void WindowSnap_OnWindowStateNormalWindowBorderThicknessChange_NewThicknessEqualsZero()
        {
            // Arrange
            ITerminalContext context = new TestTerminalContext();
            context.UserAccount = new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" };
            MainWindowViewModel viewModel = new MainWindowViewModel(context);
            var thread = new Thread(() =>
            {

                WindowModel windowModel = new WindowModel()
                {
                    OpenedWindow = new Window()
                };

                context.WindowManager.OpenedViews.Add(nameof(MainWindowViewModel), windowModel);

                // ACT
                viewModel.WindowSnap();

                // ASSERT
                viewModel.GetWindowHandler().OpenedWindow.BorderThickness.Should().BeEquivalentTo(new Thickness(0));
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// Sprawdza czy Content przycisku jest prawidłowy kiedy okno jest w Normal State
        /// </summary>
        [TestMethod]
        public void WindowSnap_OnWindowStateNormalRestoreButtonContentChange_NewContentEqualsRestore()
        {
            // Arrange
            ITerminalContext context = new TestTerminalContext();
            context.UserAccount = new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" };
            MainWindowViewModel viewModel = new MainWindowViewModel(context);
            var thread = new Thread(() =>
            {

                WindowModel windowModel = new WindowModel()
                {
                    OpenedWindow = new Window()
                };

                context.WindowManager.OpenedViews.Add(nameof(MainWindowViewModel), windowModel);

                // ACT
                viewModel.WindowSnap();

                // ASSERT
                viewModel.RestoreButtonContent.Should().Be("\xE739");
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// Sprawdza content przycisku kiedy okno jest Zmaksylizowane
        /// </summary>
        [TestMethod]
        public void WindowSnap_OnWindowStateMaximizedRestoreButtonContentChange_NewContentEqualsMaximize()
        {
            // Arrange
            ITerminalContext context = new TestTerminalContext();
            context.UserAccount = new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" };
            MainWindowViewModel viewModel = new MainWindowViewModel(context);
            var thread = new Thread(() =>
            {

                WindowModel windowModel = new WindowModel()
                {
                    OpenedWindow = new Window()
                };

                context.WindowManager.OpenedViews.Add(nameof(MainWindowViewModel), windowModel);
                viewModel.GetWindowHandler().OpenedWindow.WindowState = WindowState.Maximized;

                // ACT
                viewModel.WindowSnap();

                // ASSERT
                viewModel.RestoreButtonContent.Should().Be("\xE923");
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// Sprawdza czy ThiknessBorder jest odpowiedni kiedy okno jest w stanie maksymlizacji
        /// </summary>
        [TestMethod]
        public void WindowSnap_OnWindowStateMaximizedWindowBorderThicknessChange_NewThicknessEqualsEight()
        {
            // Arrange
            ITerminalContext context = new TestTerminalContext();
            context.UserAccount = new UserPublic() { ID = 1, FirstName = "Adam", LastName = "A", OAuthID = "12345" };
            MainWindowViewModel viewModel = new MainWindowViewModel(context);
            var thread = new Thread(() =>
            {

                WindowModel windowModel = new WindowModel()
                {
                    OpenedWindow = new Window()
                };

                context.WindowManager.OpenedViews.Add(nameof(MainWindowViewModel), windowModel);
                viewModel.GetWindowHandler().OpenedWindow.WindowState = WindowState.Maximized;

                // ACT
                viewModel.WindowSnap();

                // ASSERT
                viewModel.GetWindowHandler().OpenedWindow.BorderThickness.Should().BeEquivalentTo(new Thickness(8));
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

        }
    }
}

