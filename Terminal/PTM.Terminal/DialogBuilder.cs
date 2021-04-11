using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Tesseract.Common;

namespace PTM.Terminal
{
    /// <summary>
    /// Zbiór kreatorów okien dialogowych
    /// </summary>
    public class DialogBuilder : IDialogBuilder
    {
        /// <inheritdoc/>
        public MessageBoxResult WarningDialog(string dialogText)
        {
            return MessageBox.Show(dialogText, GetResource("IDS_DialogBuilder_WarningTitle"), MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        }

        /// <inheritdoc/>
        public MessageBoxResult ConfirmDialog(string dialogText)
        {
            return MessageBox.Show(dialogText, GetResource("IDS_DialogBuilder_ConfirmTitle"), MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
        }

        /// <inheritdoc/>
        public MessageBoxResult ChoiceDialog(string dialogText)
        {
            return MessageBox.Show(dialogText, GetResource("IDS_DialogBuilder_ChoiceTitle"), MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        /// <inheritdoc/>
        public MessageBoxResult ErrorDialog(string dialogText)
        {
            return MessageBox.Show(dialogText, GetResource("IDS_DialogBuilder_ErrorTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <inheritdoc/>
        public MessageBoxResult ErrorDialog(string dialogText, Exception ex)
        {
            Ensure.ParamNotNull(ex, nameof(ex));

            return this.ErrorDialog(dialogText + Environment.NewLine + ex.Message + Environment.NewLine + "Stack trace: " + Environment.NewLine + ex.StackTrace);
        }

        /// <inheritdoc/>
        public MessageBoxResult SuccessDialog(string dialogText)
        {
            return MessageBox.Show(dialogText, GetResource("IDS_DialogBuilder_SuccessTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string GetResource(string resource)
        {
            string strTemp;

            try 
            { 
                strTemp = (string)Application.Current.FindResource(resource); 
            }
            catch (ResourceReferenceKeyNotFoundException) 
            { 
                strTemp = "Dialog"; 
            }

            return strTemp;
        }
    }
}
