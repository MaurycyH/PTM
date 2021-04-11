using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace PTM.Terminal.WindowHelpers
{
    public interface IWindowManager
    {
        /// <summary>
        /// Słownik zawierający wszystkie otwarte okna względem ich ID, oraz uchwytu do nich.
        /// </summary>
        Dictionary<string, WindowModel> OpenedViews { get; }

        /// <summary>
        /// Otwiera wskazane okno i przekazuje mu focus.
        /// </summary>
        /// <param name="window">Okno do otworzenia</param>
        void OpenWindow(Window window);
        
        /// <summary>
        /// Otwiera wskazane okno dialogowe i przekazuje mu focus. Blokuje UI pod nim.
        /// </summary>
        /// <param name="window">Okno do otworzenia</param>
        void OpenDialog(Window window);

        /// <summary>
        /// Próbuje otworzyć okno o wskazanym identity
        /// </summary>
        /// <param name="strIdentity">Identity okna do otworzenia</param>
        bool ActivateWindow(string strIdentity);

        /// <summary>
        /// Chowa wskazane okno.
        /// </summary>
        /// <param name="strIdentity">Identity okna do ukrycia</param>
        void HideWindow(string strIdentity);

        /// <summary>
        /// Zamyka wskazane okno.
        /// </summary>
        /// <param name="window">Okno do zamknięcia</param>
        void CloseWindow(Window window);

        /// <summary>
        /// Zamyka okno o wskazanym identity.
        /// </summary>
        /// <param name="strIdentity">Identity okna</param>
        void CloseWindow(string strIdentity);
    }
}
