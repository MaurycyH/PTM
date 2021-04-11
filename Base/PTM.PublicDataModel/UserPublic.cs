using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.PublicDataModel
{
    /// <summary>
    /// Reprezentcja encji konta usera
    /// </summary>
    public class UserPublic
    {
        /// <summary>
        /// Unikalny indetyfikator użytkownika
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Imie użytkownika
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Nazwisko użytkownika
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Kod konta MS/Google użytkownika
        /// </summary>
        public string OAuthID { get; set; }
    }
}
