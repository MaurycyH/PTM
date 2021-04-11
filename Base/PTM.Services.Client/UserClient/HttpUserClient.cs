using Newtonsoft.Json;
using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Services.Client.UserClient
{
    /// <summary>
    /// Klient HTTP dla Userów
    /// </summary>
    public class HttpUserClient : BaseClient, IUserClient
    {
        /// <inheritdoc/>
        public async Task<UserPublic> CreateUser(UserPublic User)
        {
            Ensure.ParamNotNull(User, nameof(User));

            UserPublic UserResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(User);
                Uri postUri = new Uri(httpClient.BaseAddress, "/users/");

                HttpResponseMessage response = await httpClient.PostAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    UserResponse = JsonConvert.DeserializeObject<UserPublic>(json);
                }
            }

            return UserResponse;
        }

        /// <inheritdoc/>
        public async Task<UserPublic> GetUser(int ID)
        {
            UserPublic User = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(string.Format("/users/{0}", ID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    User = JsonConvert.DeserializeObject<UserPublic>(json);
                }
            }

            return User;
        }

        /// <inheritdoc/>
        public async Task<UserPublic> GetUserOAuth(string OAuthID)
        {
            UserPublic User = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(string.Format("/users/OAuth/{0}", OAuthID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    User = JsonConvert.DeserializeObject<UserPublic>(json);
                }
            }

            return User;
        }

        /// <inheritdoc/>
        public async Task<UserPublic> UpdateUser(UserPublic user)
        {
            Ensure.ParamNotNull(user, nameof(user));

            UserPublic UserResponse = null;

            using (HttpClient httpClient = base.CreateClient())
            {
                string jsonString = JsonConvert.SerializeObject(user);
                Uri postUri = new Uri(httpClient.BaseAddress, "/users/");

                HttpResponseMessage response = await httpClient.PutAsync(postUri, new StringContent(jsonString, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    UserResponse = JsonConvert.DeserializeObject<UserPublic>(json);
                }
            }

            return UserResponse;
        }

        /// <inheritdoc/>
        public async Task DeleteUser(int ID)
        {
            using (HttpClient httpClient = base.CreateClient())
            {
                HttpResponseMessage response = await httpClient.DeleteAsync(string.Format("/users/{0}", ID));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
