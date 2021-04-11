using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PTM.Services.Client.WorkItemCollectionClient
{
    /// <summary>
    /// Interfejs klienta workItema
    /// </summary>
    public interface IWorkItemCollectionClient
    {
        /// <summary>
        /// Tworzy workItem kolekcję
        /// </summary>
        /// <param name="workItemCollection">Dane kolekcji do stworzenia</param>
        /// <returns>Stworzony obiekt kolekcji</returns>
        Task<WorkItemCollectionPublic> CreateWorkItemCollection(WorkItemCollectionPublic workItemCollection);

        /// <summary>
        /// Pobiera wszystkie kolekcje dla taskboardu o wskazanym ID
        /// </summary>
        /// <param name="ID">ID taskboardu</param>
        /// <returns>WorkItemCollection</returns>
        Task<IEnumerable<WorkItemCollectionPublic>> GetAllWorkItemCollections(int ID);

        /// <summary>
        /// Pobiera kolekcję o wskazanym ID
        /// </summary>
        /// <param name="ID">ID kolekcji</param>
        /// <returns>WorkItemCollection</returns>
        Task<WorkItemCollectionPublic> GetWorkItemCollection(int ID);

        /// <summary>
        /// Aktualizuje workItema
        /// </summary>
        /// <param name="workItem">workItem do zaktualizowania</param>
        /// <returns>WorkItemCollection</returns>
        Task<WorkItemCollectionPublic> UpdateWorkItemCollection(WorkItemCollectionPublic workItem);

        /// <summary>
        /// Kasuje workItema
        /// </summary>
        /// <param name="ID">ID workItema do skasowania</param>
        /// <returns>WorkItemCollection</returns>
        Task DeleteWorkItemCollection(int ID);
    }
}
