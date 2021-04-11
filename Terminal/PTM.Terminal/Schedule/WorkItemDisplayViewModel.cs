using PTM.PublicDataModel;
using PTM.Terminal.WorkItemDetailWindow;
using System;
using System.Windows.Input;
using System.Windows.Media;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.Schedule
{
    public class WorkItemDisplayViewModel : BindableBase
    {
        private ITerminalContext mContext;

        private WorkItemPublic mWorkItem;

        private double mTop;

        private double mBottom;

        private int mDayOffset;

        /// <summary>
        /// Odpala komendę wyświetlenia okienka edycji workItema
        /// </summary>
        public ICommand ShowEditDialogCommand { get; }

        /// <summary>
        /// Workitem przypisany do kontrolki
        /// </summary>
        public WorkItemPublic WorkItem
        {
            get
            {
                return mWorkItem;
            }
            set
            {
                mWorkItem=value; OnPropertyChanged(nameof(WorkItem));
            }
        }

        /// <summary>
        /// Kolor kontrolki
        /// </summary>
        public Brush Color { get; set; }

        /// <summary>
        /// Współrzędna Y górnej krawędzi kontrolki
        /// </summary>
        public double Top
        {
            get
            {
                return mTop;
            }
            set
            {
                mTop = value; OnPropertyChanged(nameof(Top));
            }
        }

        /// <summary>
        /// Wysokość kontrolki
        /// </summary>
        public double Bottom
        {
            get
            {
                return mBottom;
            }
            set
            {
                mBottom = value; OnPropertyChanged(nameof(Bottom));
            }
        }

        public WorkItemDisplayViewModel(WorkItemPublic workItem, int dayOffset, ITerminalContext context)
        {
            WorkItem = workItem;
            mDayOffset = dayOffset;
            mContext = context;
            ShowEditDialogCommand = new BasicCommand(this.ShowEditDialog);
            UpdateDisplayProperties();
        }

        /// <summary>
        /// Pokazuje okno edycji Work Itema z odniesieniem do niego
        /// </summary>
        private void ShowEditDialog()
        {
            mContext.WindowManager.OpenWindow(new DlgWorkItemDetails(WorkItem, mContext));
        }

        /// <summary>
        /// Odświeżenie parametrów kontrolki
        /// </summary>
        public void UpdateDisplayProperties()
        {
            //odświeżają wysokość i położenie okna
            //zakładam że workItem nie może być dłuższy niż 24h, chyba sensowne ograniczenie
            if (WorkItem.WorkItemStart.Date == DateTime.Now.Date.AddDays(mDayOffset))
            {
                Top = (double)(WorkItem.WorkItemStart.Hour * 60 + WorkItem.WorkItemStart.Minute) / (ScheduleViewModel.MaxTime / 60) * ScheduleViewModel.ScheduleHeight;
                Bottom = WorkItem.WorkItemEnd.Subtract(WorkItem.WorkItemStart).TotalMinutes / (ScheduleViewModel.MaxTime / 60) * ScheduleViewModel.ScheduleHeight;
            }
            else if(WorkItem.WorkItemStart.Hour >= WorkItem.WorkItemEnd.Hour)
            {
                Top = 0;
                Bottom = (double)(WorkItem.WorkItemEnd.Hour * 60 + WorkItem.WorkItemEnd.Minute) / (ScheduleViewModel.MaxTime / 60) * ScheduleViewModel.ScheduleHeight;
            }

            if (string.IsNullOrEmpty(WorkItem.Color))
            {
                Color = Brushes.Gray;
            }
            else
            {
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(WorkItem.Color));
            }

            OnPropertyChanged(nameof(WorkItem.Name));
        }
    }
}
