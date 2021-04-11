using PTM.Entities;
using PTM.Logic;
using PTM.Logic.ModelConverters;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.WorkItems
{
    public class WorkItemRepository : IWorkItemRepository
    {
        private IDatabaseContext mDBContext;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="context"></param>
        public WorkItemRepository(IDatabaseContext context)
        {
            Ensure.ParamNotNull(context, nameof(context));

            mDBContext = context;
        }

        /// <inheritdoc/>
        public WorkItemPublic CreateWorkItem(WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemLogic workItemLogic = new WorkItemLogic(mDBContext);
            WorkItemConverter converter = new WorkItemConverter(mDBContext);

            WorkItem result = workItemLogic.CreateWorkItem(converter.Convert(workItem));
            mDBContext.SaveChanges();

            return converter.Convert(result);
        }

        /// <inheritdoc/>
        public WorkItemPublic GetWorkItem(int ID)
        {
            WorkItemLogic workItemLogic = new WorkItemLogic(mDBContext);
            WorkItemConverter converter = new WorkItemConverter(mDBContext);

            return converter.Convert(workItemLogic.GetWorkItem(ID)); ;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WorkItemPublic>> GetAllWorkItemFromUserFromDay(int ID, DateTime date)
        {
            List<WorkItemPublic> workItems = new List<WorkItemPublic>();
            WorkItemLogic workItemLogic = new WorkItemLogic(mDBContext);
            WorkItemConverter converter = new WorkItemConverter(mDBContext);

            IEnumerable<WorkItem> result = await workItemLogic.GetAllWorkItemFromDayFromUser(ID, date).ConfigureAwait(false);

            foreach (WorkItem workItem in result)
            {
                workItems.Add(converter.Convert(workItem));
            }

            return workItems;
        }

        /// <inheritdoc/>
        public WorkItemPublic UpdateWorkItem(WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemLogic workItemLogic = new WorkItemLogic(mDBContext);
            WorkItemConverter converter = new WorkItemConverter(mDBContext);

            WorkItem result = workItemLogic.UpdateWorkItem(converter.Convert(workItem));
            mDBContext.SaveChanges();

            return converter.Convert(result);
        }

        /// <inheritdoc/>
        public void DeleteWorkItem(int ID)
        {
            WorkItemLogic workItemLogic = new WorkItemLogic(mDBContext);
            workItemLogic.DeleteWorkItem(ID);
            mDBContext.SaveChanges();
        }

        /// <inheritdoc/>
        public IEnumerable<WorkItemPublic> GetAllWorkItems(int ID)
        {
            WorkItemLogic workItemLogic = new WorkItemLogic(mDBContext);
            WorkItemConverter converter = new WorkItemConverter(mDBContext);

            IEnumerable<WorkItem> result = workItemLogic.GetAllWorkItems(ID);

            foreach (WorkItem workItem in result)
            {
                yield return converter.Convert(workItem);
            }
        }
    }
}
