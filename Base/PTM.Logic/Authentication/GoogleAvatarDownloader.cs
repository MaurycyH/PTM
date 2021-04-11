using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Logic.Authentication
{

    /// <summary>
    /// Klasa odpowiadajaca za implementajce interfejsu IAvatarDownloader dla Autetykacji Google'a
    /// </summary>
    public class GoogleAvatarDownloader : IAvatarDownloader
    {
        private SettingsManager mSettingsManager = new SettingsManager();
        private string mTokenResponse;

        /// <summary>
        /// Konstrukor przekazujący odpowiedz z google zawierajaca adres url obrazka
        /// </summary>
        /// <param name="tokenResponse">Token zwrotny z Google'a</param>
        public GoogleAvatarDownloader(string tokenResponse)
        {
            //Google zawsze zwraca obraz dlatego tylko upewniam sie ze token nie jest pusty poprzez Ensure.ParamNotNull
            Ensure.ParamNotNull(tokenResponse, nameof(tokenResponse));
            mTokenResponse = tokenResponse;
        }
        public async Task GetAvatarAsync(CancellationToken cancellationToken)
        {
            JObject jsonParsed = JObject.Parse(mTokenResponse);
            string photoUrl = (string)jsonParsed.SelectToken("picture");
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(photoUrl);
                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (System.Drawing.Image googleAvatar = System.Drawing.Image.FromStream(mem))
                    {
                        ResizeImageHelper.ResizeImage(googleAvatar, 32, 32).Save(Path.Combine(mSettingsManager.PathToAppData, "PTM2020", "UserAvatar.png"), ImageFormat.Png);
                    }
                }
            }
        }
    }
}
