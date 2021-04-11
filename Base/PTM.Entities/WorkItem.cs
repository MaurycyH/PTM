using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PTM.Entities
{

    /// <summary>
    /// Kolumny tabeli WorkItem
    /// </summary>
    public class WorkItem
    {
        /// <summary>
        /// Klucz główny
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Nazwa WorkItema
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Opis WorkItema, niewymagany
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kolor Work Itema
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Data początkowa Work Itema
        /// </summary>
        public DateTime WorkItemStart { get; set; }

        /// <summary>
        /// Data końcowa Work Itema
        /// </summary>
        public DateTime WorkItemEnd { get; set; }

        /// <summary>
        /// Klucz obcy Tabeli WorkItemCollection 
        /// </summary>
        [ForeignKey("WorkItemCollectionId")]
        public int WorkItemCollectionId { get; set; }

        /// <summary>
        /// Obiekt kolekcji WorkItemów
        /// </summary>
        public virtual WorkItemCollection WorkItemCollection { get; set; }

        /// <summary>
        /// Konstruktor bez parametrów
        /// </summary>
        public WorkItem()
        {
            Color = "#FFDFD991";
        }
    }
}
