using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using PTM.Terminal.WindowHelpers;

namespace Test.PTM.Terminal
{
    class TestWindowManager : IWindowManager
    {
        public Dictionary<string, WindowModel> OpenedViews { get; }

        public TestWindowManager()
        {
            OpenedViews = new Dictionary<string, WindowModel>();
        }
        public bool ActivateWindow(string strIdentity)
        {
            throw new NotImplementedException();
        }

        public void CloseWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void CloseWindow(string strIdentity)
        {
            throw new NotImplementedException();
        }

        public void HideWindow(string strIdentity)
        {
            throw new NotImplementedException();
        }

        public void OpenDialog(Window window)
        {
            throw new NotImplementedException();
        }

        public void OpenWindow(Window window)
        {
            throw new NotImplementedException();
        }

    }

}
