using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace PTM.Terminal
{
    /// <summary>
    /// Interfejs dla kreatorów okien dialogowych
    /// </summary>
    public interface IDialogBuilder
    {
        /// <summary>
        /// Tworzy okno dialogowe z przyciskami OK/Anuluj
        /// </summary>
        MessageBoxResult WarningDialog(string dialogText);

        /// <summary>
        /// Tworzy okno dialogowe z przyciskami Tak/Nie/Anuluj
        /// </summary>
        MessageBoxResult ConfirmDialog(string dialogText);

        /// <summary>
        /// Tworzy okno dialogowe z przyciskami Tak/Nie
        /// </summary>
        MessageBoxResult ChoiceDialog(string dialogText);

        /// <summary>
        /// Tworzy okno dialogowe z przyciskiem OK
        /// </summary>
        MessageBoxResult ErrorDialog(string dialogText);

        /// <summary>
        /// Tworzy okno dialogowe z przyciskiem OK. Pozwala w łatwy sposób wyświetlić wyjątek.
        /// </summary>
        MessageBoxResult ErrorDialog(string dialogText, Exception ex);

        /// <summary>
        /// Tworzy okno dialogowe z przyciskiem OK
        /// </summary>
        MessageBoxResult SuccessDialog(string dialogText);
    }
}
