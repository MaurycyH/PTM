using PTM.Logic;
using PTM.Logic.Authentication;
using PTM.PublicDataModel;
using PTM.Terminal.WindowHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tesseract.Common;
using Tesseract.Common.MVVM;
using PTM.Terminal.ChromeWindow;
using System.Windows;
using System.Collections.ObjectModel;
using PTM.Utilities;
using System.ComponentModel;

namespace PTM.Terminal.MainWindow
{
    public class MainWindowViewModel : WindowBaseViewModel
    {

        /// <summary>
        /// Content przycisku, wartość bierze się z przekszatłecenia kodu z utf na unicode (jest do domyslny dla okna windowsowy znak)
        /// </summary>
        private string mRestoreButtonContent;

        /// <summary>
        /// Enumeracja dla łatwiejszego ustawienia contentu Buttona do maksymalizacji  i minimalizacji
        /// </summary>
        private enum mRestoreButtonState
        {
            [EnumDescription("\xE739")]
            Restore,
            [EnumDescription("\xE923")]
            Maximize
        }
        

        /// <summary>
        /// property ustawiająca content przycisku do maksymalizacji i minimalizacji
        /// </summary>
        public string RestoreButtonContent
        {
            get
            {
                return mRestoreButtonContent;
            }
            set
            {
                mRestoreButtonContent = value;
                OnPropertyChanged(nameof(RestoreButtonContent));

            }
        }

        public ITerminalContext Context { get; set; }

        /// <summary>
        /// Komenda odpowiadajaca za zamkniecie okna
        /// </summary>
        public ICommand CloseWindow { get; }

        /// <summary>
        /// Komenda odpowiadajaca za zminimalizowanie okna
        /// </summary>
        public ICommand MinimizeWindow { get; }

        /// <summary>
        /// Komenda odpowiadajaca za zmaksylaizowanie / przywrocenie okna za pomoca przycisku
        /// </summary>
        public ICommand RestoreWindow { get; }

        /// <summary>
        /// Komenda odpowiadająca za łapanie eventu StateChanged
        /// </summary>
        public ICommand RestoreOnWindowSnap { get; }

        /// <summary>
        /// Przekazuje nazwe okna do WindowManagera
        /// </summary>
        public override string Identity => nameof(MainWindowViewModel);

        /// <summary>
        /// Binding dla lewego Nagłówka
        /// </summary>
        public LeftHeader LeftHeader { get; }

        /// <summary>
        /// Binding dla środkowego Nagłówka
        /// </summary>
        public MiddleHeader MiddleHeader { get; } = new MiddleHeader();

        /// <summary>
        /// Binding dla prawego Nagłówka
        /// </summary>
        public RightHeader RightHeader { get; } = new RightHeader();

        /// <summary>
        /// Przekazanie contextu oraz przypisanie metod do przycisków
        /// </summary>
        /// <param name="context"></param>
        public MainWindowViewModel(ITerminalContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            Context = context;

            CloseWindow = new BasicCommand(CloseClick);
            MinimizeWindow = new BasicCommand(MinimizeClicked);
            RestoreWindow = new BasicCommand(RestoreClick);
            LeftHeader = new LeftHeader(" " + context.UserAccount.FirstName + " " + context.UserAccount.LastName);
            RestoreOnWindowSnap = new BasicCommand(WindowSnap);
            RestoreButtonContent = mRestoreButtonState.Restore.GetDescription();
        }


        /// <summary>
        /// Pobiera aktualne okno bazujac na Identity
        /// </summary>
        /// <returns>Zwraca WindowModel aktualnego okna</returns>
        public WindowModel GetWindowHandler()
        {
            WindowModel actualWindowOpened;
            Context.WindowManager.OpenedViews.TryGetValue(Identity, out actualWindowOpened);
            return actualWindowOpened;
        }

        /// <summary>
        /// Minimalizuje aktualne okno
        /// </summary>
        public void MinimizeClicked()
        {
            GetWindowHandler().OpenedWindow.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Maksymalizuje aktualne okno bądz wraca do poprzedniego stanu. Dodatkowo zmienia wartosc border ze wzgledu na zbugowany chromeclass
        /// </summary>
        public void RestoreClick()
        {
            if (GetWindowHandler().OpenedWindow.WindowState == WindowState.Normal)
            {
                GetWindowHandler().OpenedWindow.WindowState = WindowState.Maximized;
                GetWindowHandler().OpenedWindow.BorderThickness = new Thickness(8);
                RestoreButtonContent = mRestoreButtonState.Maximize.GetDescription();
            }
            else
            {
                GetWindowHandler().OpenedWindow.WindowState = WindowState.Normal;
                GetWindowHandler().OpenedWindow.BorderThickness = new Thickness(0);
                RestoreButtonContent = mRestoreButtonState.Restore.GetDescription();
            }
        }

        /// <summary>
        /// Zmienia border oraz content przycisku bazując na Evencie StateChanged (aero snap)
        /// </summary>
        public void WindowSnap()
        {
            if (GetWindowHandler().OpenedWindow.WindowState == WindowState.Maximized)
            {
                GetWindowHandler().OpenedWindow.BorderThickness = new Thickness(8);
                RestoreButtonContent = mRestoreButtonState.Maximize.GetDescription();
            }
            else
            {
                GetWindowHandler().OpenedWindow.BorderThickness = new Thickness(0);
                RestoreButtonContent = mRestoreButtonState.Restore.GetDescription();
            }
        }

        /// <summary>
        /// zamyka aktualne okno
        /// </summary>
        public void CloseClick()
        {
            Context.WindowManager.CloseWindow(GetWindowHandler().OpenedWindow);
        }
    }
}
