using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTM.Services.WorkItems
{
    /// <summary>
    /// Interfejs pozwalający na pobieranie WorkItemów dla kolekcji
    /// </summary>
    public interface IWorkItemRepository
    {
        /// <summary>
        /// Tworzy WorkItem w powiązanym taskboardzie
        /// </summary>
        /// <param name="workItem">Work item do stworzenia</param>
        /// <returns>Stworzony WorkItem</returns>
        public WorkItemPublic CreateWorkItem(WorkItemPublic workItem);

        /// <summary>
        /// Pobiera wskazany WorkItem
        /// </summary>
        /// <param name="ID">ID WorkItema</param>
        /// <returns>Pobrany WorkItem</returns>
        public WorkItemPublic GetWorkItem(int ID);

        /// <summary>
        /// Pobiera wszystkie workItemy z kolekcji
        /// </summary>
        /// <param name="ID">ID kolekcji</param>
        /// <returns>Pobrane WorkItemy</returns>
        public IEnumerable<WorkItemPublic> GetAllWorkItems(int ID);

        /// <summary>
        /// Pobiera wszystkie workItemy dla danego użytkownika z danego dnia.
        /// </summary>
        /// <param name="ID">ID WorkItema</param>
        /// <param name="DayOffset">Przesunięcie daty względem obecnego dnia</param>
        /// <returns>Pobrane WorkItemy</returns>
        public Task<IEnumerable<WorkItemPublic>> GetAllWorkItemFromUserFromDay(int ID, DateTime date);

        /// <summary>
        /// Aktualizuje WorkItem
        /// </summary>
        /// <param name="workItem">Work Item do zaktualizowania</param>
        /// <returns>Zaktualizowany WorkItem</returns>
        public WorkItemPublic UpdateWorkItem(WorkItemPublic workItem);

        /// <summary>
        /// Kasuje wskazany WorkItem;
        /// </summary>
        /// <param name="ID">ID WorkItema do usunięcia</param>
        public void DeleteWorkItem(int ID);
    }
}
