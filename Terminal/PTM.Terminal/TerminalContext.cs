using PTM.PublicDataModel;
using PTM.Terminal.Mediator;
using PTM.Terminal.Schedule;
using PTM.Terminal.WindowHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTM.Terminal
{
    /// <summary>
    /// Context terminala. Przekazywane są w nim najważniejsze obiekty do których powinien mieć dostęp każdy obiekt w terminalu.
    /// </summary>
    public class TerminalContext : ITerminalContext
    {
        private WindowManager mWindowManager = new WindowManager();
        private WorkItemMediator mWorkItemMediator = new WorkItemMediator();

        /// <summary>
        /// Manager okien dialogowych
        /// </summary>
        public IDialogBuilder DialogBuilder => new DialogBuilder();

        /// <summary>
        /// Manager okien niemodalnych
        /// </summary>
        public IWindowManager WindowManager => mWindowManager;

        /// <summary>
        /// Konto użytkownika
        /// </summary>
        public UserPublic UserAccount { get; set; }

        public WorkItemMediator WorkItemMediator => mWorkItemMediator;
    }
}
