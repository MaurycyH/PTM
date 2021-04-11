using PTM.PublicDataModel;
using PTM.Terminal.WindowHelpers;
using PTM.Terminal.Mediator;

namespace PTM.Terminal
{
    /// <summary>
    /// Interfejs kontekstu terminala
    /// </summary>
    public interface ITerminalContext
    {
        /// <summary>
        /// Manager okien daialogowych
        /// </summary>
        IDialogBuilder DialogBuilder { get; }

        /// <summary>
        /// Manager okien
        /// </summary>
        IWindowManager WindowManager { get; }        
        
        /// <summary>
        /// Konto użytkownika
        /// </summary>
        UserPublic UserAccount { get; set; }

        WorkItemMediator WorkItemMediator { get; }
    }
}
