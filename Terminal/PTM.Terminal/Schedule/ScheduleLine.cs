using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.Schedule
{
    public class ScheduleLine : BindableBase
    {
        private double mVerticalPos;

        /// <summary>
        /// Współrzędna Y Linii
        /// </summary>
        public double VerticalPos
        {
            get
            {
                return mVerticalPos;
            }
            set
            {
                mVerticalPos = value; OnPropertyChanged(nameof(VerticalPos));
            }
        }

        /// <summary>
        /// Odświeżenie położenia linii
        /// </summary>
        public void UpdateHeight()
        {
            DateTime now = DateTime.Now;
            float timeNow = (float)now.Subtract(now.Date).TotalSeconds / ScheduleViewModel.MaxTime; //liczba z zakresu 0-1
            VerticalPos = timeNow * ScheduleViewModel.ScheduleHeight; //razy rozmiar canvasu
        }
    }
}
