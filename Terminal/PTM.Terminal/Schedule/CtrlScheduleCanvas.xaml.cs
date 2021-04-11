using System.Windows.Controls;

namespace PTM.Terminal.Schedule
{
    /// <summary>
    /// Interaction logic for CtrlScheduleCanvas.xaml
    /// </summary>
    public partial class CtrlScheduleCanvas : UserControl
    {
        /// <summary>
        /// Tworzy podwidok harmonogramu, wiedzący jaki ma dzień w odniesieniu do dzisiejszej daty
        /// </summary>
        public CtrlScheduleCanvas(int dayOffset, ITerminalContext context)
        {
            InitializeComponent();

            this.DataContext = new ScheduleCanvasViewModel(dayOffset, context);
        }
    }
}
