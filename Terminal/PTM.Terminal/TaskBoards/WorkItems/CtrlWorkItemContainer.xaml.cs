using PTM.PublicDataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
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

namespace PTM.Terminal.TaskBoards.WorkItems
{
    /// <summary>
    /// Interaction logic for CtrlWorkItemContainer.xaml
    /// </summary>
    public partial class CtrlWorkItemContainer : UserControl
    {
        public CtrlWorkItemContainer(ITerminalContext context, WorkItemCollectionPublic wic)
        {
            InitializeComponent();
            this.DataContext = new WorkItemContainerViewModel(context, wic);
        }
    }
}
