using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PTM.Terminal.TaskBoards
{
    /// <summary>
    /// Interaction logic for TaskBoardMainView.xaml
    /// </summary>
    public partial class TaskBoardMainView : UserControl
    {
        public ITerminalContext Context
        {
            get { return (ITerminalContext)GetValue(contextProperty); }
            set { SetValue(contextProperty, value); }
        }

        public static readonly DependencyProperty contextProperty =
            DependencyProperty.Register("Context", typeof(ITerminalContext), typeof(TaskBoardMainView), new PropertyMetadata(new PropertyChangedCallback(OnPropertySet)));

        private static void OnPropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TaskBoardMainView ctrl = d as TaskBoardMainView;
            if (ctrl.Context != null)
            {
                ctrl.DataContext = new TaskBoardMainViewModel(ctrl.Context);
            }
        }

        public TaskBoardMainView()
        {
            InitializeComponent();
        }
    }
}
