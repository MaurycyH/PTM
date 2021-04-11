using Microsoft.Identity.Client;
using Newtonsoft.Json;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Graph.Auth;
using Microsoft.Graph;
using System.Drawing.Imaging;
using System.Drawing;

namespace PTM.Logic.Authentication
{
    /// <summary>
    /// Klasa odpowiada za zalogowanie uzytkownika poprzez OAuth2 przy pomocy konta Microsoftu
    /// </summary>
    public class MicrosoftAuthentication : IOAuthProvider
    {
        private ConfigurationReader mConfigurationReader = new ConfigurationReader();
        private TokenCacheHelper mTokencacheHelper;

        //Set the API Endpoint to Graph 'me' endpoint. 
        private Uri mGraphAPIEndpoint = new Uri("https://graph.microsoft.com/v1.0/me");

        //Set the scope for API call to user.read
        private string[] mScopes = new string[] { "user.read" };
        private string mClientId;

        // Note: Tenant is important for the quickstart.
        private  string mTenant = "common";
        private  string mInstance = "https://login.microsoftonline.com/";
        private  IPublicClientApplication mClientApp;
        private UserPublic mUser;

        /// <summary>
        /// Manager poświadczeń
        /// </summary>
        private CredentialsManager mCredentialsManager { get; }

        /// <summary>
        /// Tworzy klasę autentykacji z domyślnym Managerem Poświadczeń (Microsoft)
        /// </summary>
        public MicrosoftAuthentication()
        {
            mClientId = mConfigurationReader.Setting<string>("MicrosoftClientId");
            mTokencacheHelper = new TokenCacheHelper();

            this.mCredentialsManager = new CredentialsManager(AuthenticationProvider.Microsoft);
            mClientApp = PublicClientApplicationBuilder.Create(mClientId)
           .WithAuthority($"{mInstance}{mTenant}")
           .WithDefaultRedirectUri()
           .Build();

            // Odpowiada za szybkie logowanie za pomoca cache tokena
            mTokencacheHelper.EnableSerialization(mClientApp.UserTokenCache);
        }
        /// <inheritdoc/>
        public async Task<UserPublic> AuthenticateUserAsync(CancellationToken cancellationToken)
        {
            await AuthenticateWithMicrosoft(cancellationToken).ConfigureAwait(false);
            return mUser;
        }

        public async Task<UserPublic> RefreshUserAsync(string refreshToken, CancellationToken cancellationToken)
        {
            await AuthenticateWithMicrosoft(cancellationToken).ConfigureAwait(false);
            return mUser;
        }

        /// <summary>
        /// Call AcquireToken - to acquire a token requiring user to sign-in
        /// </summary>
        private async Task AuthenticateWithMicrosoft(CancellationToken cancellationToken)
        {
            AuthenticationResult authResult = null;
            IEnumerable<IAccount> accounts = await mClientApp.GetAccountsAsync().ConfigureAwait(false);
            IAccount firstAccount = accounts.FirstOrDefault();

            //AcquireTokenSilent pobiera token z pliku cache, i jesli jest tam zapisane jakies konto to je loguje a jak nie to looguje po raz pierwszy przez strone
            try
            {
                authResult = await mClientApp.AcquireTokenSilent(mScopes, firstAccount).ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (MsalUiRequiredException ex)
            {
                try
                {
                    authResult = await mClientApp.AcquireTokenInteractive(mScopes)
                        .WithAccount(accounts.FirstOrDefault())
                        .WithPrompt(Microsoft.Identity.Client.Prompt.SelectAccount) 
                        .ExecuteAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (MsalException msalex)
                {
                    throw;
                    //TODO: zalogowac przypadek z msalex
                }
            }
            catch (MsalClientException ex)
            {
                throw;
                //TODO: zalogowac przypadek z msalex
                
            }

            // Tutaj TokenResponse przechowuje informacje o uzytkowniku z tokenu
            if (authResult != null)
            {
                string tokenResponse = await GetHttpContentWithToken(mGraphAPIEndpoint, authResult.AccessToken, cancellationToken).ConfigureAwait(false);
                MicrosoftAvatarDownloader microsoftAvatarDownloader = new MicrosoftAvatarDownloader(mClientApp, mScopes);
                mUser = new UserAccountFactory().Create(AuthenticationProvider.Microsoft, tokenResponse);
                await microsoftAvatarDownloader.GetAvatarAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public async Task<string> GetHttpContentWithToken(Uri url, string token, CancellationToken cancellationToken)
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                using (System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url))
                {
                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return content;
                }
            }
            catch (Exception)
            {
                throw;
            }
            ///finally dla usuniecia warningu
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
