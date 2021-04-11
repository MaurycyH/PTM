using PTM.Entities;
using PTM.PublicDataModel;
using System.Windows;

namespace PTM.Terminal.WorkItemDetailWindow
{
    /// <summary>
    /// Interaction logic for DlgWorkItemDetails.xaml
    /// </summary>
    public partial class DlgWorkItemDetails : Window
    {
        public DlgWorkItemDetails(WorkItemPublic workItem, ITerminalContext context)
        {
            InitializeComponent();
            this.DataContext = new WorkItemDetailsViewModel(workItem, context);
        }
    }
}
