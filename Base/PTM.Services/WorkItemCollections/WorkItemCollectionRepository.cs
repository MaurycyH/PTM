using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Tesseract.Common;

namespace PTM.Services.WorkItemCollections
{
    public class WorkItemCollectionRepository : IWorkItemCollectionsRepository
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="databaseContext">Kontekst bazdy danych</param>
        public WorkItemCollectionRepository(IDatabaseContext databaseContext)
        {
            Ensure.ParamNotNull(databaseContext, nameof(databaseContext));

            mDBContext = databaseContext;
        }
        public WorkItemCollectionPublic CreateWorkItemCollection(WorkItemCollectionPublic workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));
            WorkItemCollectionLogic wicLogic = new WorkItemCollectionLogic(mDBContext);
            WorkItemCollectionConverter converter = new WorkItemCollectionConverter(mDBContext);

            WorkItemCollection result = wicLogic.CreateWorkItemCollection(converter.Convert(workItemCollection));

            mDBContext.SaveChanges();

            return converter.Convert(result);
        }

        public IEnumerable<WorkItemCollectionPublic> GetAllWorkItemCollections(int ID)
        {
            List<WorkItemCollection> collections = mDBContext.WorkItemCollections.Where(wic => wic.TaskBoardId == ID).ToList();

            WorkItemCollectionConverter converter = new WorkItemCollectionConverter(mDBContext);

            foreach (WorkItemCollection collection in collections)
            {
                yield return converter.Convert(collection);
            }
        }

        public void DeleteWorkItemCollection(int ID)
        {
            WorkItemCollectionLogic logic = new WorkItemCollectionLogic(mDBContext);

            logic.DeleteWorkItemCollection(ID);

            mDBContext.SaveChanges();
        }

        public WorkItemCollectionPublic GetWorkItemCollection(int ID)
        {
            throw new NotImplementedException();
        }

        public WorkItemCollectionPublic UpdateWorkItemCollection(WorkItemCollectionPublic workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));
            WorkItemCollectionLogic wicLogic = new WorkItemCollectionLogic(mDBContext);
            WorkItemCollectionConverter converter = new WorkItemCollectionConverter(mDBContext);

            WorkItemCollection updatedCollection = wicLogic.UpdateWorkItemCollection(converter.Convert(workItemCollection));

            mDBContext.SaveChanges();

            return converter.Convert(updatedCollection);
        }

        IEnumerable<WorkItemCollectionPublic> IWorkItemCollectionsRepository.GetAllWorkItemCollections(int ID)
        {
            List<WorkItemCollection> collections = mDBContext.WorkItemCollections.Where(wic => wic.TaskBoardId == ID).ToList();

            WorkItemCollectionConverter converter = new WorkItemCollectionConverter(mDBContext);

            foreach (WorkItemCollection collection in collections)
            {
                yield return converter.Convert(collection);
            }
        }
    }
}
