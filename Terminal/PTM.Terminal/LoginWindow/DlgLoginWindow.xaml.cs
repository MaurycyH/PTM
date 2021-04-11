using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PTM.Terminal.LoginWindow
{
    /// <summary>
    /// Interaction logic for DlgLoginWindow.xaml
    /// </summary>
    public partial class DlgLoginWindow : Window
    {
        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="context">Kontekst terminala.</param>
        public DlgLoginWindow(ITerminalContext context)
        {
            InitializeComponent();
            this.DataContext = new LoginWindowViewModel(context);
        }

        /// <summary>
        /// Usuwa zasoby niezarządzalne po zamknięciu okna.
        /// </summary>
        private void OnShutDownStarted(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IDisposable disposable = DataContext as IDisposable;

            if (!ReferenceEquals(null, disposable))
            {
                disposable.Dispose();
            }
        }
    }
}
