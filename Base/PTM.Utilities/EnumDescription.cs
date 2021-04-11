using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.Utilities
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumDescription : Attribute
    {
        /// <summary>
        /// Opis wartości enuma
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Dodaje do enuma dodatkowy atrybut typu string
        /// </summary>
        public EnumDescription(string description)
        {
            this.Description = description;
        }
    }
}
