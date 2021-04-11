using System;
using System.Windows;
using System.Windows.Controls;

namespace PTM.Terminal.Schedule
{
    /// <summary>
    /// Interaction logic for CtrlSchedule.xaml
    /// </summary>
    public partial class CtrlSchedule : UserControl
    {
        public ITerminalContext Context
        {
            get { return (ITerminalContext)GetValue(contextProperty); }
            set { SetValue(contextProperty, value); }
        }

        public static readonly DependencyProperty contextProperty =
            DependencyProperty.Register("Context", typeof(ITerminalContext), typeof(CtrlSchedule), new PropertyMetadata(new PropertyChangedCallback(OnPropertySet)));

        private static void OnPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CtrlSchedule ctrl = d as CtrlSchedule;
            if (ctrl.Context != null)
            {
                ctrl.DataContext = new ScheduleViewModel(ctrl.Context);
            }
        }

        public CtrlSchedule()
        {
            InitializeComponent();

            //"150" w ScrollToVerticalOffset przesuwa suwak tak, żeby linia czasu była wyśrodkowana. Jest to niezależne od SchedulerHeight.
            DateTime now = DateTime.Now;
            ScrollControl.ScrollToVerticalOffset((now.Subtract(now.Date).TotalSeconds / ScheduleViewModel.MaxTime * ScheduleViewModel.ScheduleHeight) - 150);
        }
    }
}
