using PTM.PublicDataModel;
using PTM.Services.Client.WorkItemClient;
using PTM.Terminal.Mediator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tesseract.Common;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.Schedule
{
    public class ScheduleViewModel : BindableBase
    {
        /// <summary>
        /// Wysokość wewnątrz kontrolki z suwakiem
        /// </summary>
        public const int ScheduleHeight = 2160;

        /// <summary>
        /// 24 godziny przedstawione w sekundach
        /// </summary>
        public const int MaxTime = 86400;

        private ITerminalContext mContext;

        private double mObservedHeight = 0;

        private int lastHour = 0;

        /// <summary>
        /// Pobiera odpowiednią wysokośc kontrolki, wykorzystywana w xaml
        /// </summary>
        public int GetScheduleHeight
        {
            get { return ScheduleHeight; }
        }

        /// <summary>
        /// Aktualizująca się wysokość kanwy
        /// </summary>
        public double ObservedHeight
        {
            //hmmm, chyba wartość jest nieużywana, ale Set dalej jest używany do aktualizacji wszystkiego. Na razie tak zostawię.
            get
            {
                return mObservedHeight;
            }
            set
            {
                mObservedHeight = value;
                Line.UpdateHeight();
            }
        }

        /// <summary>
        /// Linia Harmonogramu
        /// </summary>
        public ScheduleLine Line { get; }

        /// <summary>
        /// Kolekcja kanw na workitemy 
        /// </summary>
        public ObservableCollection<CtrlScheduleCanvas> Schedules { get; }

        /// <summary>
        /// Konstruktor ViewModelu dla Schedulera
        /// </summary>
        public ScheduleViewModel(ITerminalContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));
            mContext = context;
            mContext.WorkItemMediator.WorkItemUpdatedEvent += new WorkItemMediator.OnWorkItemUpdated(ReloadWorkItem);
            mContext.WorkItemMediator.WorkItemsUpdatedEvent += new WorkItemMediator.OnWorkItemsUpdated(OnWorkItemsUpdated);

            Schedules = new ObservableCollection<CtrlScheduleCanvas>
            {
                new CtrlScheduleCanvas(0, context),
                new CtrlScheduleCanvas(1, context),
                new CtrlScheduleCanvas(2, context)
            };

            DispatcherTimer LiveTime = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            LiveTime.Tick += TimerTickEvent;
            LiveTime.Start();

            Line = new ScheduleLine();
            LoadWorkItems();
        }

        /// <summary>
        /// Wysyła do ScheduleCanvasów nakaz wczytania WorkItemów
        /// </summary>
        public void LoadWorkItems()
        {
            foreach (CtrlScheduleCanvas canvas in Schedules)
            {
                ((ScheduleCanvasViewModel)canvas.DataContext).LoadWorkItems();
            }
        }

        /// <summary>
        /// Metoda odbierająca komunikat o zmianie stanu wielu Workitemów
        /// </summary>
        /// <param name="m"></param>
        public void OnWorkItemsUpdated(WorkItemMediator m)
        {
            LoadWorkItems();
        }

        /// <summary>
        /// Metoda odbierająca komunikat o zmianie stanu WorkItema
        /// </summary>
        public void ReloadWorkItem(WorkItemMediator m, WorkItemEventArgs e)
        {
            Ensure.ParamNotNull(e, nameof(e));

            WorkItemPublic workItem = e.WorkItem;
            DateTime now = DateTime.Now.Date;

            foreach (CtrlScheduleCanvas canvas in Schedules)
            {
                if (workItem.WorkItemStart.Date <= now.AddDays(((ScheduleCanvasViewModel)canvas.DataContext).DayOffset) && workItem.WorkItemEnd.Date >= now.AddDays(((ScheduleCanvasViewModel)canvas.DataContext).DayOffset))
                {
                    ((ScheduleCanvasViewModel)canvas.DataContext).LoadOrUpdateWorkItem(workItem);
                }
                else ((ScheduleCanvasViewModel)canvas.DataContext).DeleteWorkItem(workItem);
            }
        }

        /// <summary>
        /// Aktualizuje wskazówkę (linię) czasu
        /// </summary>
        public void TimerTickEvent(object sender, EventArgs e)
        {
            int currentHour = DateTime.Now.Hour;

            if (lastHour > currentHour)
            {
                LoadWorkItems();
            }

            lastHour = currentHour;

            Line.UpdateHeight();
        }
    }
}
