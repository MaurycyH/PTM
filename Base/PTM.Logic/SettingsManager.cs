using PTM.Logic.Authentication;
using System;
using System.IO;

namespace PTM.Logic
{
    public class SettingsManager
    {
        private string mFilePath;
        private string mAppDataPath;
        public string PathToAppData
        {
            set 
            {
                mAppDataPath = value;
            }
            get 
            {
                return mAppDataPath;
            }
        }
        public SettingsManager()
        {
            PathToAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            mFilePath = Path.Combine(PathToAppData, "PTM2020", "DefaultProvider.ptm");
            Directory.CreateDirectory(Path.GetDirectoryName(mFilePath));
        }

        /// <summary>
        /// Wczytuje providera w formie stringu z pliku w appdata
        /// </summary>
        public string LoadProvider()
        {
            if (File.Exists(mFilePath))
            {
                using (BinaryReader lineReader = new BinaryReader(File.Open(mFilePath, FileMode.Open)))
                {
                    return lineReader.ReadString();
                }
            }
            else return null;
        }

        /// <summary>
        /// Zapisuje providera do pliku w appdata
        /// </summary>
        public void SaveProvider(AuthenticationProvider provider)
        {
            try
            {
                using (BinaryWriter lineWriter = new BinaryWriter(File.Open(mFilePath, FileMode.OpenOrCreate)))
                {
                    lineWriter.Write(provider.ToString());
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
        }

        /// <summary>
        /// Usuwa plik z appdata
        /// </summary>
        public void DeleteProvider()
        {
            if (File.Exists(mFilePath))
            {
                File.Delete(mFilePath);
            }
        }
    }
}
