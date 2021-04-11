using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.PublicDataModel
{
    public class WorkItemCollectionPublic
    {
        /// <summary>
        /// ID WorkItemCollection
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Nazwa kolekcji
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID taskboardu z którym kolekcja jest powiązana
        /// </summary>
        public int TaskBoardID { get; set; }

        /// <summary>
        /// Kolekcja WorkItemów nad 
        /// </summary>
        public ICollection<WorkItemPublic> WorkItems { get; set; }
    }
}
