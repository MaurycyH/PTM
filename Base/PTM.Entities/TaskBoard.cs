using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PTM.Entities
{
    /// <summary>
    /// Kolumny tabeli TaskBoard
    /// </summary>
    public class TaskBoard
    {

        /// <summary>
        /// Klucz Główny
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Nazwa kontenera
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        [ForeignKey("UserID")]
        public int UserID { get; set; }

        /// <summary>
        /// Navigation property dla klucza jeden do wielu z Tabelą User
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Navigation property dla klucza jeden do wielu z Tabelą WorkItemCollection
        /// </summary>
        public virtual ICollection<WorkItemCollection> WorkItemCollections { get; set; }
    }
}
