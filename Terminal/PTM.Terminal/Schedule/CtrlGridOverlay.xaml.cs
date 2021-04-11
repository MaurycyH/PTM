using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PTM.Terminal.Schedule
{
    /// <summary>
    /// Interaction logic for CtrlTimetable.xaml
    /// </summary>
    public partial class CtrlGridOverlay : UserControl
    {
        public CtrlGridOverlay()
        {
            InitializeComponent();

            this.DataContext = new GridOverlayViewModel();
        }
    }
}
