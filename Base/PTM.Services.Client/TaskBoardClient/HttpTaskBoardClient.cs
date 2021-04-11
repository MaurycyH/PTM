using Newtonsoft.Json;
using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.Client.TaskBoardClient
{
    /// <summary>
    /// Klient HTTP dla taskboardów
    /// </summary>
    public class HttpTaskBoardClient : BaseClient, ITaskBoardClient
    {
        /// <inheritdoc/>
        public async Task<TaskBoardPublic> GetTaskboard(int ID)
        {
            TaskBoardPublic taskboard = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(string.Format("/taskboards/{0}", ID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    taskboard = JsonConvert.DeserializeObject<TaskBoardPublic>(json);
                }
            }

            return taskboard;
        }

        public async Task<ICollection<TaskBoardPublic>> GetAllTaskboards(int userID)
        {
            List<TaskBoardPublic> taskboard = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(string.Format("/taskboards/GetAll/{0}", userID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    taskboard = JsonConvert.DeserializeObject<List<TaskBoardPublic>>(json);
                }
            }

            return taskboard;
        }

        /// <inheritdoc/>
        public async Task<TaskBoardPublic> CreateTaskBoard(TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            TaskBoardPublic taskBoardResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(taskBoard);
                Uri postUri = new Uri(httpClient.BaseAddress, "/taskboards/");

                HttpResponseMessage response = await httpClient.PostAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    taskBoardResponse = JsonConvert.DeserializeObject<TaskBoardPublic>(json);
                }
            }

            return taskBoardResponse;
        }

        /// <inheritdoc/>
        public async Task<TaskBoardPublic> UpdateTaskBoard(TaskBoardPublic taskBoard)
        {
            Ensure.ParamNotNull(taskBoard, nameof(taskBoard));

            TaskBoardPublic taskBoardResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(taskBoard);
                Uri postUri = new Uri(httpClient.BaseAddress, "/taskboards/");

                HttpResponseMessage response = await httpClient.PutAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    taskBoardResponse = JsonConvert.DeserializeObject<TaskBoardPublic>(json);
                }
            }

            return taskBoardResponse;
        }

        /// <inheritdoc/>
        public async Task DeleteTaskBoard(int ID)
        {
            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(string.Format("/taskboards/{0}", ID));
            }
        }
    }
}
