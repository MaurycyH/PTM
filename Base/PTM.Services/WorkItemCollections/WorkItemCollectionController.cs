using Microsoft.AspNetCore.Mvc;
using PTM.Logic;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.WorkItemCollections
{
    [ApiController]
    [Route("WorkItemCollections")]
    public class WorkItemCollectionController : ControllerBase
    {
        private IWorkItemCollectionsRepository mRepository;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="repository">Repozytorium Work Item collection</param>
        public WorkItemCollectionController(IWorkItemCollectionsRepository repository)
        {
            Ensure.ParamNotNull(repository, nameof(repository));

            mRepository = repository;
        }

        /// <summary>
        /// Tworzy taskboard
        /// </summary>
        /// <param name="collection">Obiekt taskboardu do stworzenia</param>
        [HttpPost]
        public virtual IActionResult CreateWorkItemCollection([FromBody] WorkItemCollectionPublic collection)
        {
            Ensure.ParamNotNull(collection, nameof(collection));

            WorkItemCollectionPublic createdWorkItemCollection = mRepository.CreateWorkItemCollection(collection);

            return base.Created($"/WorkItemCollections/{createdWorkItemCollection.ID}", createdWorkItemCollection);
        }

        /// <summary>
        /// Pobiera wszystkie WorkItem kolekcje dla taskboardu
        /// </summary>
        /// <param name="ID">ID TaskBoardu</param>
        /// <returns>WorkitemCollections</returns>
        [HttpGet("GetAll/{ID}")]
        public IActionResult GetAllCollections(int ID)
        {
            if (ID <= 0)
            {
                return base.BadRequest("ID has to be grater than 0!");
            }

            IEnumerable<WorkItemCollectionPublic> workItemCollections = mRepository.GetAllWorkItemCollections(ID);

            return base.Ok(workItemCollections);
        }

        /// <summary>
        /// Aktualizuje Kolekcję workItemów
        /// </summary>
        /// <param name="workItemCollection">Obiekt Kolekcji do zaktualizowania</param>
        [HttpPut]
        public virtual IActionResult UpdateCollection([FromBody] WorkItemCollectionPublic workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));

            WorkItemCollectionPublic updatedCollection = mRepository.UpdateWorkItemCollection(workItemCollection);

            if (updatedCollection == null)
            {
                return base.NotFound();
            }

            return base.Created($"/workItemCollections/{updatedCollection.ID}", updatedCollection);
        }

        /// <summary>
        /// Kasuje Kolekcję workItemów
        /// </summary>
        /// <param name="ID">ID Kolekcji workItemów do usunięcia</param>
        [HttpDelete("{ID}")]
        public virtual IActionResult DeleteCollection(int ID)
        {
            if (ID <= 0)
            {
                return base.Problem("Parameter has to be grater than 0");
            }

            mRepository.DeleteWorkItemCollection(ID);

            return base.Ok($"/users/{ID}");
        }
    }
}
