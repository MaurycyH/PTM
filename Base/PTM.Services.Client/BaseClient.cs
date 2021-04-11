using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PTM.Services.Client
{
    public abstract class BaseClient
    {
        /// <summary>
        /// Tworzy obiekt gotowy HTTP Client. Przydatny do stworzenia requesta.
        /// </summary>
        /// <returns>Obiekt HTTPClient.</returns>
        protected HttpClient CreateClient()
        {
            HttpClient httpClient = new HttpClient()
            {
                // TODO zmienić na dynamiczne wczytywanie z settingsów programu
                BaseAddress = new Uri("https://localhost:44313")
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }
}
