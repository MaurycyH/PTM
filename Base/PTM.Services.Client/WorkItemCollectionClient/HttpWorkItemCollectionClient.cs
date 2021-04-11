using Newtonsoft.Json;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.Client.WorkItemCollectionClient
{
    /// <inheritdoc/>
    public class HttpWorkItemCollectionClient : BaseClient, IWorkItemCollectionClient
    {
        public async Task<WorkItemCollectionPublic> CreateWorkItemCollection(WorkItemCollectionPublic workItemCollection)
        {
            Ensure.ParamNotNull(workItemCollection, nameof(workItemCollection));

            WorkItemCollectionPublic taskBoardResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(workItemCollection);
                Uri postUri = new Uri(httpClient.BaseAddress, "/WorkItemCollections/");

                HttpResponseMessage response = await httpClient.PostAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    taskBoardResponse = JsonConvert.DeserializeObject<WorkItemCollectionPublic>(json);
                }
            }

            return taskBoardResponse;
        }

        /// <inheritdoc/>
        public async Task DeleteWorkItemCollection(int ID)
        {
            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(string.Format("/WorkItemCollections/{0}", ID));
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WorkItemCollectionPublic>> GetAllWorkItemCollections(int ID)
        {
            List<WorkItemCollectionPublic> workItemCollections = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(string.Format("/WorkItemCollections/GetAll/{0}", ID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    workItemCollections = JsonConvert.DeserializeObject<List<WorkItemCollectionPublic>>(json);
                }
            }

            return workItemCollections;
        }

        /// <inheritdoc/>
        public Task<WorkItemCollectionPublic> GetWorkItemCollection(int ID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<WorkItemCollectionPublic> UpdateWorkItemCollection(WorkItemCollectionPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemCollectionPublic UserResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(workItem);
                Uri postUri = new Uri(httpClient.BaseAddress, "/WorkItemCollections/");

                HttpResponseMessage response = await httpClient.PutAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    UserResponse = JsonConvert.DeserializeObject<WorkItemCollectionPublic>(json);
                }
            }

            return UserResponse;
        }
    }
}
