using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Timers;
using System.Windows.Threading;
using Tesseract.Common.MVVM;

namespace PTM.Terminal.ChromeWindow
{
    /// <summary>
    /// Klasa odpowiadjaca za stworzenie Timera oraz odswieżanie go
    /// </summary>
    public class MiddleHeader : BindableBase
    {
        /// <summary>
        /// Konstruktor w którym tworzony jest obiekt Timer który odpowiada za "liczenie" czasu oraz przechowywanie jego wartosci
        /// </summary>
        public MiddleHeader()
        {
            Timer timer = new Timer();
            timer.Interval = 1000; // Aktualizacja co sekunde
            timer.Elapsed += TimerElapsed;
            timer.Start();
         }

        /// <summary>
        /// Konwersja czasu na string oraz wyswietlanie go
        /// </summary>
        public string  Now
        {
            get 
            {
                return DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture); 
            }
        }

        /// <summary>
        /// Event odpowadajacy za odswieżanie czasu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="elapsedEventArg"></param>
       public void TimerElapsed(object sender, ElapsedEventArgs elapsedEventArg)
       {
            OnPropertyChanged(nameof(Now));
       }
    }
}
 