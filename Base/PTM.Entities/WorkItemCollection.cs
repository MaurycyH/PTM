using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PTM.Entities
{
    /// <summary>
    /// Kolumny tabeli WorkItemCollection
    /// </summary>
    [Table("WorkItemCollection")]
    public class WorkItemCollection
    {
        /// <summary>
        /// Klucz główny
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Nazwa kolekcji WorkItemów
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Klucz obcy Tablicy TaskBoard
        /// </summary>
        [ForeignKey("TaskBoardId")]
        public int TaskBoardId { get; set; }
        public virtual TaskBoard TaskBoard { get; set; }

        /// <summary>
        /// Navigation Property dla klucza jeden do wielu z Tabelą WorkItem
        /// </summary>
        public virtual ICollection<WorkItem> WorkItems { get; set; }
    }
}
