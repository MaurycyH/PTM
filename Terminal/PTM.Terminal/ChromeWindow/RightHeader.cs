using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PTM.Terminal.ChromeWindow
{
    /// <summary>
    /// Klasa odpowiadająca za ustawienie napisu dotyczącego najblizszego taska
    /// </summary>
    public class RightHeader
    {

        /// <summary>
        /// Odniesienie do bindingu 
        /// </summary>
        public string FirstTask { get; set; }

        /// <summary>
        /// Konstruktor w którym ustawiany jest napis do wyświetlenia
        /// </summary>
        public RightHeader()
        {
            FirstTask = Application.Current.Resources["IDS_WindowChrome_NextTask"] as string;
        }
    }
}
