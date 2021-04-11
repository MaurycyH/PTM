using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Common;
using Microsoft.Identity.Client;
using Microsoft.Graph.Auth;

namespace PTM.Logic.Authentication
{
    /// <summary>
    /// Klasa implementujaca Interfejs IAvatarDownloader i odpowiada za pobranie obrazka z Microsoftu za pomoca SDK
    /// </summary>
    public class MicrosoftAvatarDownloader : IAvatarDownloader
    {
        private SettingsManager mSettingsManager = new SettingsManager();
        GraphServiceClient graphClient;

        /// <summary>
        /// Konstruktor który inicjalizuje podstawowe obiekty dla Graphów
        /// </summary>
        /// <param name="ClientApp">Obiekt z bibloteki MSAL, ktory informuje o rodzaju aplikacji oraz metodzie autentykacji</param>
        /// <param name="Scopes">Parametry które pobierzemy od uzytkownika</param>
        public MicrosoftAvatarDownloader(IPublicClientApplication ClientApp, string[] Scopes)
        {
            InteractiveAuthenticationProvider interactiveAuthenticationProvider = new InteractiveAuthenticationProvider(ClientApp, Scopes);
            graphClient = new GraphServiceClient(interactiveAuthenticationProvider);
            graphClient.BaseUrl = "https://graph.microsoft.com/beta";
        }
        /// <inheritdoc/>
        public async Task GetAvatarAsync(CancellationToken cancellationToken)
        {
            Ensure.ParamNotNull(graphClient, nameof(graphClient));
            try
            {
                // GET /me
                Stream photoresponse = await graphClient.Me.Photo.Content.Request().GetAsync(cancellationToken).ConfigureAwait(false);

                if (photoresponse != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        photoresponse.CopyTo(ms);
                        System.Drawing.Image microsoftAvatar = System.Drawing.Image.FromStream(ms);
                        ResizeImageHelper.ResizeImage(microsoftAvatar, 32, 32).Save(Path.Combine(mSettingsManager.PathToAppData, "PTM2020", "UserAvatar.png"), ImageFormat.Png);

                    }
                }
                else
                {
                    throw new ArgumentException("Photo response is null", nameof(Microsoft));
                }
            }
            catch (ServiceException ex)
            {
                // Jesli uzytkownik nie posiada avatara to usuwa poprzedni zapamietany zeby nie zostal wczytany do programu zły avatar
                System.IO.File.Delete(mSettingsManager.PathToAppData + "\\PTM2020" + "\\UserAvatar.png");
            }
        }
    }
}
