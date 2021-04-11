using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Collections.Specialized;

namespace PTM.Logic
{

    /// <summary>
    /// Klasa odpowiaadajaca za odczyt danych z app.config w class library
    /// </summary>
    public class ConfigurationReader
    {
        private UriBuilder mUri;
        private Configuration mMyDllConfig; 
        private ConnectionStringsSection mConnectionStringsSection;
        public ConfigurationReader()
        {
            mUri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            mMyDllConfig = ConfigurationManager.OpenExeConfiguration(mUri.Path);
            mConnectionStringsSection = (ConnectionStringsSection)mMyDllConfig.GetSection("connectionStrings");
        }
        /// <summary>
        /// Metoda zwracająca wartość danego settingu
        /// </summary>
        /// <typeparam name="T"> Rodzaj zmiennej</typeparam>
        /// <param name="name"> Nazwa poszukiwanego settingu</param>
        /// <returns></returns>
        public T Setting<T>(string name)
        { 
            return (T)Convert.ChangeType(mConnectionStringsSection.ConnectionStrings[name].ConnectionString, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
