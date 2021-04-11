using System.Windows;

namespace PTM.Terminal.WindowHelpers
{
    public class WindowModel
    {
        /// <summary>
        /// Uchwyt do okna
        /// </summary>
        public Window OpenedWindow { get; set; }

        /// <summary>
        /// Ostatni stan okna przed minimalizacją
        /// </summary>
        public WindowState LastWindowState { get; set; }

        /// <summary>
        /// Uchwyt do VM
        /// </summary>
        public object ViewModel { get; set; }
    }
}
