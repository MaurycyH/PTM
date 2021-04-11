using System;
using System.Collections.Generic;
using System.Text;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.WindowHelpers
{
    /// <summary>
    /// Domyślna klasa dla bindowania ViewModelu
    /// </summary>
    public abstract class WindowBaseViewModel : BindableBase
    {
        /// <summary>
        /// Unikalne identity dla okna.
        /// </summary>
        /// <remarks>
        /// Identity należy ustawić indywidualnie dla każdego okna.
        /// </remarks>
        public abstract string Identity { get; }
    }
}
