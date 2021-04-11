using System;
using System.Windows;

namespace PTM.Terminal.MainWindow
{
    /// <summary>
    /// Interaction logic for DlgMainWindow.xaml
    /// </summary>
    public partial class DlgMainWindow : Window
    {
        public DlgMainWindow(ITerminalContext context)
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel(context);
        }

    }
}
