using PTM.PublicDataModel;
using System.Windows.Controls;

namespace PTM.Terminal.Schedule
{
    /// <summary>
    /// Interaction logic for CtrlTaskContainer.xaml
    /// </summary>
    public partial class CtrlWorkItemDisplay : UserControl
    {
        public CtrlWorkItemDisplay(WorkItemPublic workItem, int dayOffset, ITerminalContext context)
        {
            InitializeComponent();

            DataContext = new WorkItemDisplayViewModel(workItem, dayOffset, context);
        }
    }
}