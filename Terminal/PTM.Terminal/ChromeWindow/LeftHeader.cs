using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using PTM.Logic;
using Tesseract.Common;

namespace PTM.Terminal.ChromeWindow
{
    /// <summary>
    /// Klasa pobierająca nazwe użytkownika
    /// </summary>
    public class LeftHeader
    {

        private SettingsManager mSettingsManager = new SettingsManager();
        /// <summary>
        /// Odwłowanie do bindigu
        /// </summary>
        public string UserName { get; set; }
        public string AvatarPath { get; set; }

        /// <summary>
        /// Konstruktor który ustawia napis do wyświetlenia
        /// </summary>
        public LeftHeader(string name)
        {
            Ensure.ParamNotNullOrEmpty(name, nameof(name));
            UserName = Application.Current.Resources["IDS_WindowChrome_UserLogged"] as string;
            UserName += " " + name;
            CheckIfAvatarExist();
        }

        /// <summary>
        /// Sprawdza czy jakis plik istnieje, jesli tak to go ustawia (bo to awatar) a jesli nie to domyslny
        /// </summary>
        public void CheckIfAvatarExist()
        {
            if (File.Exists(Path.Combine(mSettingsManager.PathToAppData , "PTM2020" , "UserAvatar.png")) == true )
            {
                AvatarPath = Path.Combine(mSettingsManager.PathToAppData, "PTM2020", "UserAvatar.png");
            }
            else
            {
                AvatarPath = Application.Current.FindResource("IMG_MainWindowViewModel_AvatarIcon").ToString();
            }
        }
    }
}
