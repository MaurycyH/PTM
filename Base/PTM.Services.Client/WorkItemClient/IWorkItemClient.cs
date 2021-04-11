using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTM.Services.Client.WorkItemClient
{
    /// <summary>
    /// Interfejs klienta workItema
    /// </summary>
    public interface IWorkItemClient
    {
        /// <summary>
        /// Tworzy workItema
        /// </summary>
        /// <param name="workItem">Dane workItema do stworzenia</param>
        /// <returns>Stworzony obiekt workItema</returns>
        Task<WorkItemPublic> CreateWorkItem(WorkItemPublic workItem);

        /// <summary>
        /// Pobiera WorkItemy dla użytkownika, z konkretnego dnia
        /// </summary>
        /// <param name="userID">ID Użytkownika</param>
        /// <param name="date">Dzień z którego workitemy mają być pobrane</param>
        /// <returns></returns>
        Task<IEnumerable<WorkItemPublic>> GetAllWorkItemsFromDay(int userID, DateTime date);

        /// <summary>
        /// Pobiera wszystkie workitemy z danej kolekcji
        /// </summary>
        /// <param name="ID">ID WorkItema</param>
        /// <returns>Pobrane WorkItemy</returns>
        Task<IEnumerable<WorkItemPublic>> GetAllWorkItems(int ID);

        /// <summary>
        /// Pobiera workItema o wskazanym ID
        /// </summary>
        /// <param name="ID">ID workItema</param>
        /// <returns>WorkItem</returns>
        Task<WorkItemPublic> GetWorkItem(int ID);

        /// <summary>
        /// Aktualizuje workItema
        /// </summary>
        /// <param name="workItem">workItem do zaktualizowania</param>
        /// <returns>WorkItem</returns>
        Task<WorkItemPublic> UpdateWorkItem(WorkItemPublic workItem);

        /// <summary>
        /// Kasuje workItema
        /// </summary>
        /// <param name="ID">ID workItema do skasowania</param>
        /// <returns>WorkItem</returns>
        Task DeleteWorkItem(int ID);
    }
}
