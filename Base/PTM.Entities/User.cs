using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PTM.Entities
{
    /// <summary>
    /// Kolumny tabeli User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Klucz Główny 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Imie usera
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Nazwisko usera
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Kod konta MS/Google usera
        /// </summary>
        [Required]
        public string OAuthID { get; set; }

        /// <summary>
        /// Navigation property dla klucza jeden do wielu z Tabelą TaskBoards
        /// </summary>
        public virtual ICollection<TaskBoard> TaskBoards { get; set; }

    }
}
