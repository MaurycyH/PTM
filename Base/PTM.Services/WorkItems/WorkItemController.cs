using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PTM.PublicDataModel;
using Tesseract.Common;

namespace PTM.Services.WorkItems
{
    [ApiController]
    [Route("workitems")]
    public class WorkItemController : ControllerBase
    {
        private IWorkItemRepository mRepository;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="repository">Repozytorium WorkItemów</param>
        public WorkItemController(IWorkItemRepository repository)
        {
            Ensure.ParamNotNull(repository, nameof(repository));

            mRepository = repository;
        }

        /// <summary>
        /// Tworzy WorkItem
        /// </summary>
        /// <param name="workItem">WorkItem do stworzenia</param>
        /// <returns>Stworzony WorkItem</returns>
        [HttpPost]
        public virtual IActionResult CreateWorkItem([FromBody] WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            // workItem musi mieć ustawionego rodzica
            if (workItem.WorkItemCollectionID <= 0)
            {
                return base.BadRequest(workItem);
            }

            try
            {
                WorkItemPublic workItemPublic = mRepository.CreateWorkItem(workItem);
                return base.Created($"/workitems/{workItemPublic.ID}", workItemPublic);
            }
            catch
            {
                return base.BadRequest(workItem);
            }
        }

        /// <summary>
        /// Pobiera WorkItem
        /// </summary>
        /// <param name="ID">ID workitema</param>
        /// <returns>Workitem</returns>
        [HttpGet("{ID}")]
        public IActionResult GetWorkItem(int ID)
        {
            if (ID <= 0)
            {
                return base.BadRequest("ID have to be grater than 0!");
            }

            WorkItemPublic workItem = mRepository.GetWorkItem(ID);

            if (workItem == null)
            {
                base.NotFound(workItem);
            }

            return base.Ok(workItem);
        }

        /// <summary>
        /// Pobiera WorkItemy dla użytkownika, z konkretnego dnia
        /// </summary>
        /// <param name="ID">ID Użytkownika</param>
        /// <param name="date">Dzień z którego workitemy mają być pobrane</param>
        /// <returns></returns>
        [HttpPost("GetUserDay/{ID}")]
        public async Task<IActionResult> GetAllWorkItems(int ID, [FromBody] DateTime date)
        {
            if (ID <= 0)
            {
                return base.BadRequest("ID has to be grater than 0!");
            }

            IEnumerable<WorkItemPublic> workItem = await mRepository.GetAllWorkItemFromUserFromDay(ID, date);

            return base.Ok(workItem);
        }

        /// <summary>
        /// Pobiera WorkItem
        /// </summary>
        /// <param name="ID">ID workitema</param>
        /// <returns>Workitem</returns>
        [HttpGet("GetAll/{ID}")]
        public IActionResult GetAllWorkItems(int ID)
        {
            if (ID <= 0)
            {
                return base.BadRequest("ID has to be grater than 0!");
            }
            IEnumerable<WorkItemPublic> workItem = mRepository.GetAllWorkItems(ID);
            return base.Ok(workItem);
        }

        /// <summary>
        /// Aktualizuje workitem
        /// </summary>
        /// <param name="workItem">WorkItem do zaktualizowania</param>
        /// <returns>Zaktualizowany WorkItem</returns>
        [HttpPut]
        public IActionResult UpdateWorkItem([FromBody] WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            if (workItem.ID <= 0)
            {
                return base.BadRequest(workItem);
            }

            WorkItemPublic updatedWorkItem = mRepository.UpdateWorkItem(workItem);

            if (updatedWorkItem == null)
            {
                return base.NotFound(workItem);
            }

            return base.Created($"/workitems/{updatedWorkItem.ID}", updatedWorkItem);
        }

        /// <summary>
        /// Kasuje workitem
        /// </summary>
        /// <param name="ID">ID workitema do skasowania</param>
        [HttpDelete]
        public IActionResult DeleteWorkItem(int ID)
        {
            if (ID <= 0)
            {
                return base.BadRequest(ID);
            }

            mRepository.DeleteWorkItem(ID);

            return base.Ok();
        }
    }
}
