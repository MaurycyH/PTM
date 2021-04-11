using PTM.Entities;
using PTM.Terminal.LoginWindow;
using System;
using System.Windows;

namespace PTM.Terminal
{
    class Program
    {
        [STAThread]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Jedyne miejsce programu, które powinno zbierać wszystkie typy wyjątków")]
        public static void Main()
        {
            ITerminalContext context = new TerminalContext();

            try
            {
                using (SingleInstanceController instanceController = new SingleInstanceController())
                {
                    if (null == Application.Current)
                    {
                        // Towrzenie nowej instacji aplikacji, żeby móc w prosty sposób przechowywać resources.
                        _ = new Application();
                        Application.Current.Resources.Source = new Uri("/PTM.Terminal;component/Resources/MergedDictionary.xaml", UriKind.Relative);
                    }

                    if (instanceController.CheckIfRunning())
                    {
                        context.WindowManager.OpenDialog(new DlgLoginWindow(context));
                    }
                    else
                    {
                        context.DialogBuilder.ErrorDialog(Application.Current.TryFindResource("IDS_Program_InstanceExists") as string);
                    }
                }

            }
            catch (Exception ex)
            {
                context.DialogBuilder.ErrorDialog((Application.Current.TryFindResource("IDS_Program_CriticalError") as string) + Environment.NewLine + ex.Message);
            }
        }
    }
}
