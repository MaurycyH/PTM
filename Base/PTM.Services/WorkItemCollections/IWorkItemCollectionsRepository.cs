using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTM.Services.WorkItemCollections
{
    /// <summary>
    /// Repozytorium WorkItemCollections
    /// </summary>
    public interface IWorkItemCollectionsRepository
    {
        /// <summary>
        /// Tworzy kolekcje WorkItemów
        /// </summary>
        /// <param name="workItemCollection">Kolekcja workitemów</param>
        /// <returns>Stworzona kolekcja WorkItemów</returns>
        public WorkItemCollectionPublic CreateWorkItemCollection(WorkItemCollectionPublic workItemCollection);

        /// <summary>
        /// Pobiera wskazaną kolekcje
        /// </summary>
        /// <param name="ID">ID kolekcji do pobrania</param>
        /// <returns>Pobrana kolekcja workitemów</returns>
        public WorkItemCollectionPublic GetWorkItemCollection(int ID);

        /// <summary>
        /// Pobiera wszystkie WorkItem kolekcje dla taskboardu
        /// </summary>
        /// <param name="ID">ID TaskBoardu</param>
        /// <returns>WorkitemCollections</returns>
        public IEnumerable<WorkItemCollectionPublic> GetAllWorkItemCollections(int ID);

        /// <summary>
        /// Aktualizuje wskazaną kolekcję
        /// </summary>
        /// <param name="workItemCollection">Kolekcja do zaktualizowania</param>
        /// <returns>Zaktualizowana kolekcja</returns>
        public WorkItemCollectionPublic UpdateWorkItemCollection(WorkItemCollectionPublic workItemCollection);

        /// <summary>
        /// Usuwa wskazaną kolekcję
        /// </summary>
        /// <param name="ID">ID kolekcji do usunięcia</param>
        public void DeleteWorkItemCollection(int ID);
    }
}
