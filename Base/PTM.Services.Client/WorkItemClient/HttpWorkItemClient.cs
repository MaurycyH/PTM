using Newtonsoft.Json;
using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.Client.WorkItemClient
{
    public class HttpWorkItemClient : BaseClient, IWorkItemClient
    {
        /// <inheritdoc/>
        public async Task<WorkItemPublic> CreateWorkItem(WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemPublic workItemResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(workItem);
                Uri postUri = new Uri(httpClient.BaseAddress, "/WorkItems/");

                HttpResponseMessage response = await httpClient.PostAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    workItemResponse = JsonConvert.DeserializeObject<WorkItemPublic>(json);
                }
            }

            return workItemResponse;
        }

        /// <inheritdoc/>
        public Task DeleteWorkItem(int ID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WorkItemPublic>> GetAllWorkItemsFromDay(int userID, DateTime date)
        {
            List<WorkItemPublic> workItems = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(date);
                Uri postUri = new Uri(httpClient.BaseAddress, "/workItems/GetUserDay/" + userID);

                HttpResponseMessage response = await httpClient.PostAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    workItems = JsonConvert.DeserializeObject<List<WorkItemPublic>>(json);
                }
            }

            return workItems;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<WorkItemPublic>> GetAllWorkItems(int ID)
        {
            List<WorkItemPublic> workItems = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(string.Format("/WorkItems/GetAll/{0}", ID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    workItems = JsonConvert.DeserializeObject<List<WorkItemPublic>>(json);
                }
            }

            return workItems;
        }

        /// <inheritdoc/>
        public Task<WorkItemPublic> GetWorkItem(int ID)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<WorkItemPublic> UpdateWorkItem(WorkItemPublic workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItemPublic UserResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(workItem);
                Uri postUri = new Uri(httpClient.BaseAddress, "/workItems/");

                HttpResponseMessage response = await httpClient.PutAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    UserResponse = JsonConvert.DeserializeObject<WorkItemPublic>(json);
                }
            }

            return UserResponse;
        }

        /// <inheritdoc/>
        public async Task<WorkItem> CheckDateTimeConflicts(WorkItem workItem)
        {
            Ensure.ParamNotNull(workItem, nameof(workItem));

            WorkItem UserResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(workItem);
                Uri postUri = new Uri(httpClient.BaseAddress, "/workItems/ConflictCheck");

                HttpResponseMessage response = await httpClient.PutAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    UserResponse = JsonConvert.DeserializeObject<WorkItem>(json);
                }
            }

            return null;
        }
    }
}
