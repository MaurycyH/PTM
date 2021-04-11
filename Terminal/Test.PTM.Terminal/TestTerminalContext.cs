using Moq;
using PTM.Entities;
using PTM.PublicDataModel;
using PTM.Terminal;
using PTM.Terminal.Mediator;
using PTM.Terminal.Schedule;
using PTM.Terminal.WindowHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Test.PTM.Terminal
{
    public class TestTerminalContext : ITerminalContext
    {
        private TestWindowManager mWindowManager = new TestWindowManager();
        public IDialogBuilder DialogBuilder => throw new NotImplementedException();
        UserPublic ITerminalContext.UserAccount { get; set; }
        public ScheduleViewModel ScheduleViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public WorkItemMediator WorkItemMediator => throw new NotImplementedException();

        public IWindowManager WindowManager => mWindowManager;

        public TestTerminalContext()
        {
            if  (Application.Current is null)
            {
                _ = new Application();
                Application.Current.Resources.Source = new Uri("/PTM.Terminal;component/Resources/MergedDictionary.xaml", UriKind.Relative);
            }
        }
    }
}
