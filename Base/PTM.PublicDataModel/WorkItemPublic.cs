using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.PublicDataModel
{
    public class WorkItemPublic
    {
        /// <summary>
        /// Pobiera ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Nazwa WorkItema
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Opis WorkItema
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kolor WorkItema
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Data startu WorkItema
        /// </summary>
        public DateTime WorkItemStart { get; set; }

        /// <summary>
        /// Data końca WorkItema
        /// </summary>
        public DateTime WorkItemEnd { get; set; }

        /// <summary>
        /// ID powiązanej kolekcji
        /// </summary>
        public int WorkItemCollectionID { get; set; }
    }
}
