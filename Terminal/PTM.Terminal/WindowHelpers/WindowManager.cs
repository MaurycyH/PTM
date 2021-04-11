using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Tesseract.Common;

namespace PTM.Terminal.WindowHelpers
{
    /// <summary>
    /// Klasa zarządzająca oknami w PTM
    /// </summary>
    /// <remarks>
    /// ViewModel każdego okna, które ma obługiwac WindowManager musi dziedziczyć po <see cref="WindowBaseViewModel"/>
    /// </remarks>
    public class WindowManager : IWindowManager
    {
        /// <inheritdoc/>
        public Dictionary<string, WindowModel> OpenedViews { get; }

        /// <summary>
        /// Domyślny ctor. Tworzy instancje OpenedWindows.
        /// </summary>
        public WindowManager()
        {
            OpenedViews = new Dictionary<string, WindowModel>();
        }

        /// <inheritdoc/>
        public void OpenWindow(Window window)
        {
            Ensure.ParamNotNull(window, nameof(window));

            try
            {
                WindowModel model = this.RegisterWindow(window);
                model?.OpenedWindow.Show();
            }
            catch (Exception)
            {

                //
                // TODO - zapisać wyjątek loggerem
                //

                throw;
            }
        }

        /// <inheritdoc/>
        public bool ActivateWindow(string strIdentity)
        {
            Ensure.ParamNotNullOrEmpty(strIdentity, nameof(strIdentity));

            try
            {
                // Jeśli występuje okno w kolekcji o takim kluczu, to je otwieramy
                if (OpenedViews.Any(c => c.Key == strIdentity))
                {
                    OpenedViews[strIdentity].OpenedWindow.WindowState = OpenedViews[strIdentity].LastWindowState;
                    OpenedViews[strIdentity].OpenedWindow.Activate();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                //
                // TODO - zapisać wyjątek loggerem
                //

                throw;
            }
        }

        /// <inheritdoc/>
        public void OpenDialog(Window window)
        {
            Ensure.ParamNotNull(window, nameof(window));

            try
            {
                WindowModel model = this.RegisterWindow(window);
                model?.OpenedWindow.ShowDialog();
            }
            catch (Exception)
            {
                //
                // TODO - zapisać wyjątek loggerem
                //
                throw;
            }
        }

        /// <inheritdoc/>
        public void CloseWindow(Window window)
        {
            Ensure.ParamNotNull(window, nameof(window));

            if (window.IsLoaded)
            {
                window.Close();
            }

            OpenedViews.Remove(((WindowBaseViewModel)window.DataContext).Identity);
        }

        /// <inheritdoc/>
        public void CloseWindow(string strIdentity)
        {
            Ensure.ParamNotNullOrEmpty(strIdentity, nameof(strIdentity));

            Window window = OpenedViews[strIdentity].OpenedWindow;

            if (window.IsLoaded)
            {
                window.Close();
            }

            OpenedViews.Remove(strIdentity);
        }

        /// <inheritdoc/>
        public void HideWindow(string strIdentity)
        {
            Ensure.ParamNotNullOrEmpty(strIdentity, nameof(strIdentity));

            Window window = OpenedViews[strIdentity].OpenedWindow;

            // Możemy jedynie schować okno jeśli jest załadowane
            if (window.IsLoaded)
            {
                window.Hide();
            }
        }

        /// <summary>
        /// Przy zamykaniu wyrejstrowuje okno ze słownika.
        /// </summary>
        private void OnWindowClosing(object sender, EventArgs e)
        {
            this.CloseWindow((Window)sender);
        }

        /// <summary>
        /// Rejstruje wskazane okno jeśli nie ma go w słowniku. 
        /// </summary>
        /// <remarks>
        /// Jeśli okno jest juz zarejstrowane, to zostanie przywrócone na górę.
        /// </remarks>
        /// <param name="window">Okno do otworzenia. Jeśli okno jest otwarte, to zwracany jest null.</param>
        private WindowModel RegisterWindow(Window window)
        {
            Ensure.ParamNotNull(window, nameof(window));

            WindowModel model = new WindowModel()
            {
                OpenedWindow = window,
                ViewModel = window?.DataContext
            };

            WindowBaseViewModel viewModel = model.ViewModel as WindowBaseViewModel;

            if (viewModel == null)
            {
                throw new ArgumentException(Application.Current.TryFindResource("IDS_Exception_IncorretViewModelBase") as string);
            }

            string strIdentity = viewModel.Identity;

            if (this.ActivateWindow(strIdentity))
            {
                return null;
            }

            // Przypinamy zdarzenia do okna
            model.OpenedWindow.StateChanged += OnWindowStateChanged;
            model.OpenedWindow.Closed += OnWindowClosing;

            // Dodajemy okna do kolekcji i je pokazujemy
            OpenedViews.Add(strIdentity, model);

            return model;
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany stanu okna
        /// </summary>
        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            Window window = (Window)sender;

            if (window.WindowState != WindowState.Minimized)
            {
                OpenedViews.First(ov => ov.Value.OpenedWindow.GetHashCode() == window.GetHashCode()).Value.LastWindowState = window.WindowState;
            }
        }

        
    }
}
