using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PTM.Terminal.CustomControls
{
    /// <summary>
    /// Interaction logic for CtrlDateTime.xaml
    /// </summary>
    public partial class CtrlDateTime : UserControl
    {
        public List<int> HoursList => new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        public List<int> MinutesList => new List<int>() { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

        private int backupMinute, backupHour;

        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime", typeof(DateTime), typeof(CtrlDateTime));

        public CtrlDateTime()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            SelectedDateTime = SelectedDateTime.Date.AddHours((int)Hours.SelectedValue).AddMinutes((int)Minutes.SelectedValue);
        }

        private void Date_CalendarOpened(object sender, RoutedEventArgs e)
        {
            backupMinute = (int)Minutes.SelectedValue;
            backupHour = (int)Hours.SelectedValue;
        }

        private void Date_CalendarClosed(object sender, RoutedEventArgs e)
        {
            SelectedDateTime = SelectedDateTime.Date.AddHours(backupHour).AddMinutes(backupMinute);
        }
    }
}
