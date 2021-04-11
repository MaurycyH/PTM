using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.PublicDataModel
{
    /// <summary>
    /// Publiczna encja Taskboardu
    /// </summary>
    public class TaskBoardPublic
    {
        /// <summary>
        /// ID Taskboardu
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Nazwa taskboardu
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID usera
        /// </summary>
        public int UserID { get; set; }
    }
}
