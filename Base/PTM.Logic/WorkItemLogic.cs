using Microsoft.EntityFrameworkCore;
using PTM.Entities;
using PTM.PublicDataModel;
using PTM.Services.Client.WorkItemClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Logic
{
    public class WorkItemLogic
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="context">Kontekst DB</param>
        public WorkItemLogic(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mDBContext = context;
        }

        /// <summary>
        /// Tworzy WorkItem
        /// </summary>
        /// <param name="workItem">WorkItem do stworzenia</param>
        /// <returns>Stworzony obiekt WorkItema</returns>
        public WorkItem CreateWorkItem(WorkItem workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            if (workItem.ID != 0)
            {
                throw new ArgumentException($"ID can't be set on entity. The provided ID is {workItem.ID}");
            }

            if (!mDBContext.WorkItemCollections.Any(wic => wic.ID == workItem.WorkItemCollectionId))
            {
                throw new ArgumentException($"The specified WorkItemCollectionID doesn't exist - {workItem.WorkItemCollectionId}");
            }

            if (!CheckDateTimeConflicts(workItem))
            {
                throw new ArgumentException($"WorkItem {workItem.Name} conflicts with an existing workitem");
            }

            return mDBContext.WorkItems.Add(workItem).Entity;
        }

        /// <summary>
        /// Pobiera WorkItem z bazy
        /// </summary>
        /// <param name="ID">ID WorkItema do pobrania</param>
        /// <returns>WorkItem</returns>
        public WorkItem GetWorkItem(int ID)
        {
            if (ID <= 0)
            {
                throw new ArgumentException($"Parameter have to be grater than 0. The provided value is {ID}.");
            }

            return mDBContext.WorkItems.FirstOrDefault(wi => wi.ID == ID);
        }

        /// <summary>
        /// Pobiera wszystkie workItemy dla danego użytkownika z danego dnia.
        /// </summary>
        /// <param name="ID">ID Usera</param>
        /// <param name="date">Data z której mają być pobrane itemy</param>
        /// <returns>Pobrane WorkItemy</returns>
        public async Task<IEnumerable<WorkItem>> GetAllWorkItemFromDayFromUser(int ID, DateTime date)
        {
            List<WorkItem> workItems = new List<WorkItem>();
            date = date.Date;

            List<WorkItem> result = await mDBContext.WorkItems
                .Where(w => w.WorkItemEnd.Date >= date && w.WorkItemStart.Date <= date)
                .Where(u => u.WorkItemCollection.TaskBoard.User.ID == ID).ToListAsync().ConfigureAwait(false);

            workItems.AddRange(result);

            return workItems;
        }

        /// <summary>
        /// Pobiera wszystkie workitemy z danej kolekcji
        /// </summary>
        /// <param name="ID">ID WorkItema</param>
        /// <returns>Pobrane WorkItemy</returns>
        public IEnumerable<WorkItem> GetAllWorkItems(int ID)
        {
            if (ID <= 0)
            {
                throw new AggregateException($"Parameter have to be grater than 0. The provided value is {ID}.");
            }

            return mDBContext.WorkItems.Where(w => w.WorkItemCollectionId == ID);
        }

        /// <summary>
        /// Aktualizuje WorkItem
        /// </summary>
        /// <param name="workItem">Obiekt zaktualizowanego WorkItema</param>
        /// <returns>Zaktualizowana encja WorkItema</returns>
        public WorkItem UpdateWorkItem(WorkItem workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            if (workItem.ID <= 0)
            {
                throw new ArgumentException($"Parameter {nameof(workItem)} ID's have to be grater than 0. The provided value is {workItem.ID}.");
            }

            WorkItem result = mDBContext.WorkItems.FirstOrDefault(wi => wi.ID == workItem.ID);

            if (result == null)
            {
                throw new ArgumentException($"WorkItem with ID {workItem.ID} does not exist.");
            }

            if (!CheckDateTimeConflicts(workItem))
            {
                return result;
            }

            result.Name = workItem.Name;
            result.Color = workItem.Color;
            result.Description = workItem.Description;
            result.WorkItemCollectionId = workItem.WorkItemCollectionId;
            result.WorkItemEnd = workItem.WorkItemEnd;
            result.WorkItemStart = workItem.WorkItemStart;

            return result;
        }

        /// <summary>
        /// Kasuje WorkItem
        /// </summary>
        /// <param name="ID">ID work itema do skasowania</param>
        public void DeleteWorkItem(int ID)
        {
            if (ID <= 0)
            {
                throw new AggregateException($"Parameter have to be grater than 0. The provided value is {ID}.");
            }

            WorkItem workItem = new WorkItem() { ID = ID };

            try
            {
                mDBContext.WorkItems.Attach(workItem);
            }
            catch (InvalidOperationException)
            {
                // Może się zdarzyć, że już takie entity śledzimy w ramach danego kontekstu
                // TODO zapisać logerem
            }

            mDBContext.WorkItems.Remove(workItem);
        }

        private bool CheckDateTimeConflicts(WorkItem workItem)
        {
            User user = mDBContext.WorkItemCollections.Find(workItem.WorkItemCollectionId).TaskBoard.User;

            List<WorkItem> result = mDBContext.WorkItems
                .Where(w => w.WorkItemStart < workItem.WorkItemEnd && w.WorkItemEnd > workItem.WorkItemStart)
                .Where(u => u.WorkItemCollection.TaskBoard.User.ID == user.ID).ToList();

            result.Remove(result.FirstOrDefault(wi => wi.ID == workItem.ID));

            if (result.Count > 0)
            {
                return false;
            }
            else return true;
        }
    }
}
