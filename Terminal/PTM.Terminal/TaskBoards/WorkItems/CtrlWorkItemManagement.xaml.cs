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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PTM.PublicDataModel;
using PTM.Terminal.MainWindow;

namespace PTM.Terminal.TaskBoards.WorkItems
{
    /// <summary>
    /// Interaction logic for CtrlWorkItemManagement.xaml
    /// </summary>
    public partial class CtrlWorkItemManagement : UserControl
    {
        public CtrlWorkItemManagement(ITerminalContext context, TaskBoardPublic taskBoard)
        {
            InitializeComponent();
            this.DataContext = new WorkItemManagementViewModel(context, taskBoard);
        }
    }
}
