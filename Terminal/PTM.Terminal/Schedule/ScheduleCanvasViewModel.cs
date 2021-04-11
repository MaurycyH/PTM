using PTM.PublicDataModel;
using PTM.Services.Client.WorkItemClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.Schedule
{
    public class ScheduleCanvasViewModel : BindableBase
    {
        private ITerminalContext mContext;

        public int DayOffset { get; set; }

        /// <summary>
        /// Kolekcja WorkItemów
        /// </summary>
        public ObservableCollection<CtrlWorkItemDisplay> TaskBoard { get; }

        /// <summary>
        /// Konstruktor ViewModelu dla wykresu Harmonogramu
        /// </summary>
        public ScheduleCanvasViewModel(int dayOffset, ITerminalContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));
            DayOffset = dayOffset;
            mContext = context;
            TaskBoard = new ObservableCollection<CtrlWorkItemDisplay>();
        }

        /// <summary>
        /// Tworzy WorkItemDisplaye i dodaje je do kolekcji
        /// </summary>
        public void LoadWorkItems()
        {
            TaskBoard.Clear();
            Task.Run(() => GetWorkItems());
        }

        /// <summary>
        /// Dodaje pojedynczy WorkItem do Kolekcji, lub aktualizuje jego parametry, jeżeli już istnieje.
        /// </summary>
        /// <param name="workItem"></param>
        public void LoadOrUpdateWorkItem(WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            CtrlWorkItemDisplay toUpdate = TaskBoard.FirstOrDefault(tb => ((WorkItemDisplayViewModel)tb.DataContext).WorkItem.ID == workItem.ID);

            if (toUpdate != null)
            {
                WorkItemPublic workItemPublic = ((WorkItemDisplayViewModel)toUpdate.DataContext).WorkItem = workItem;
                ((WorkItemDisplayViewModel)toUpdate.DataContext).UpdateDisplayProperties();
            }
            else
            {
                TaskBoard.Add(new CtrlWorkItemDisplay(workItem, DayOffset, mContext));
            }
        }

        /// <summary>
        /// Usuwa pojedynczy WorkItem z Kolekcji, jeżeli istnieje.
        /// </summary>
        /// <param name="workItem"></param>
        public void DeleteWorkItem(WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            CtrlWorkItemDisplay toRemove = TaskBoard.FirstOrDefault(tb => ((WorkItemDisplayViewModel)tb.DataContext).WorkItem.ID == workItem.ID);

            if (toRemove != null)
            {
                TaskBoard.Remove(toRemove);
            }
        }

        /// <summary>
        /// Pobiera wszystkie workItemy użytkownika
        /// </summary>
        private async Task GetWorkItems()
        {
            DateTime today = DateTime.Now.AddDays(DayOffset);

            try
            {
                HttpWorkItemClient client = new HttpWorkItemClient();

                IEnumerable<WorkItemPublic> response = await client.GetAllWorkItemsFromDay(mContext.UserAccount.ID, today).ConfigureAwait(false);

                await Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    foreach (WorkItemPublic workItem in response)
                    {
                        TaskBoard.Add(new CtrlWorkItemDisplay(workItem, DayOffset, mContext));
                    }
                });
            }
            catch (Exception)
            {
                mContext.DialogBuilder.ErrorDialog("Work Items couldn't be retrieved, due to server error.");
            }
        }
    }
}