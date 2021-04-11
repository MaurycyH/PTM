using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract.Common;

namespace PTM.Logic.Authentication
{
    /// <summary>
    /// Autentykuje aplikacje po przez wykorzystanie google.
    /// </summary>
    internal class GoogleAuthentication : IOAuthProvider
    {
        private const string clientID = "*TOKEN*";
        private const string clientSecret = "*TOKEN*";
        private const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        private UserPublic User;

        /// <summary>
        /// Manager poświadczeń
        /// </summary>
        private CredentialsManager CredentialsManager { get; }
    
        /// <summary>
        /// Tworzy klasę autentykacji z domyślnym Managerem Poświadczeń (Google)
        /// </summary>
        public GoogleAuthentication()
        {
            this.CredentialsManager = new CredentialsManager(AuthenticationProvider.Google);
        }

        /// <inheritdoc/>
        public async Task<UserPublic> AuthenticateUserAsync(CancellationToken cancellationToken)
        {
            await AuthenticateWithGoogle().ConfigureAwait(false);
            return User;
        }

        /// <inheritdoc/>
        public async Task<UserPublic> RefreshUserAsync(string refreshToken, CancellationToken cancellationToken)
        {
            await AuthenticateWithRefreshToken(refreshToken).ConfigureAwait(false);
            return User;
        }

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        public string RandomDataBase64url(uint length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[length];
                rng.GetBytes(bytes);
                return Base64urlencodeNoPadding(bytes);
            }
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        public byte[] Sha256(string inputStirng)
        {
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(inputStirng);
                return sha256.ComputeHash(bytes);
            }
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        public string Base64urlencodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-", StringComparison.InvariantCultureIgnoreCase);
            base64 = base64.Replace("/", "_", StringComparison.InvariantCultureIgnoreCase);
            // Strips padding.
            base64 = base64.Replace("=", "", StringComparison.InvariantCultureIgnoreCase);

            return base64;
        }

        /// <summary>
        /// Loguje użytkownika przez konto Google (OAuth 2)
        /// </summary>
        private async Task AuthenticateWithGoogle()
        {
            // Generates state and PKCE values.
            string state = RandomDataBase64url(32);
            string code_verifier = RandomDataBase64url(32);
            string code_challenge = Base64urlencodeNoPadding(Sha256(code_verifier));
            const string code_challenge_method = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectURI = "http://localhost:8080/";

            // Creates an HttpListener to listen for requests on that redirect URI.
            using (HttpListener http = new HttpListener())
            {
                http.Prefixes.Add(redirectURI);
                http.Start();

                // Creates the OAuth 2.0 authorization request.
                string authorizationRequest = string.Format(CultureInfo.InvariantCulture, "{0}?response_type=code&scope=profile%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}&access_type=offline&approval_prompt=force",
                    authorizationEndpoint,
                    System.Uri.EscapeDataString(redirectURI),
                    clientID,
                    state,
                    code_challenge,
                    code_challenge_method);

                // Opens request in the browser.
                Process.Start(new ProcessStartInfo(authorizationRequest) { UseShellExecute = true });

                // Waits for the OAuth authorization response.
                HttpListenerContext context = await http.GetContextAsync().ConfigureAwait(false);

                // Sends an HTTP response to the browser.
                HttpListenerResponse response = context.Response;
                string responseString = string.Format(CultureInfo.InvariantCulture, "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>");
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream responseOutput = response.OutputStream;
                Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
                {
                    responseOutput.Close();
                    http.Stop();
                });

                // Checks for errors.
                if (context.Request.QueryString.Get("error") != null)
                {
                    return;
                }
                if (context.Request.QueryString.Get("code") == null
                    || context.Request.QueryString.Get("state") == null)
                {
                    return;
                }

                // extracts the code
                string code = context.Request.QueryString.Get("code");
                string incoming_state = context.Request.QueryString.Get("state");

                // Compares the receieved state to the expected value, to ensure that
                // this app made the request which resulted in authorization.
                if (incoming_state != state)
                {
                    return;
                }

                // Starts the code exchange at the Token Endpoint.
                await PerformCodeExchange(code, code_verifier, redirectURI).ConfigureAwait(false);
                return;
            }
        }

        /// <summary>
        /// pobiera Token użytkownika
        /// </summary>
        private async Task PerformCodeExchange(string code, string code_verifier, string redirectURI)
        {
            // builds the  request
            Uri tokenRequestURI = new Uri("https://www.googleapis.com/oauth2/v4/token");
            string tokenRequestBody = string.Format(CultureInfo.InvariantCulture, "code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier,
                clientSecret
                );

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = _byteVersion.Length;
            Stream stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length).ConfigureAwait(false);
            stream.Close();

            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync().ConfigureAwait(false);
                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    // reads response body
                    string responseText = await reader.ReadToEndAsync().ConfigureAwait(false);

                    // converts to dictionary
                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    string AccessToken = tokenEndpointDecoded["access_token"];
                    CredentialsManager.SaveToken(tokenEndpointDecoded["refresh_token"]);
                    await UserinfoCall(AccessToken).ConfigureAwait(false);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // reads response body
                            string responseText = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// pobiera access Token używając refresh tokenu
        /// </summary>
        private async Task AuthenticateWithRefreshToken(string Token)
        {
            // builds the  request
            Uri tokenRequestURI = new Uri("https://www.googleapis.com/oauth2/v4/token");
            string tokenRequestBody = $"client_id={clientID}&client_secret={clientSecret}&grant_type=refresh_token&refresh_token={Token}";

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = _byteVersion.Length;
            Stream stream = tokenRequest.GetRequestStream();
            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length).ConfigureAwait(false);
            stream.Close();

            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync().ConfigureAwait(false);
                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    // reads response body
                    string responseText = await reader.ReadToEndAsync().ConfigureAwait(false);

                    // converts to dictionary
                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    string access_token = tokenEndpointDecoded["access_token"];
                    await UserinfoCall(access_token).ConfigureAwait(false);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // reads response body
                            string responseText = await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Tworzy usera na podstawie otrzymanego tokenu
        /// </summary>
        private async Task UserinfoCall(string AccessToken)
        {
            // builds the  request
            Uri userinfoRequestURI = new Uri("https://www.googleapis.com/oauth2/v3/userinfo");
            string userinfoResponseText = "";
            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestURI);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format(CultureInfo.InvariantCulture, "Authorization: Bearer {0}", AccessToken));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync().ConfigureAwait(false);
            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                // reads response body
                userinfoResponseText = await userinfoResponseReader.ReadToEndAsync().ConfigureAwait(false);
            }
            User = new UserAccountFactory().Create(AuthenticationProvider.Google, userinfoResponseText);
            GoogleAvatarDownloader googleAvatarDownloader = new GoogleAvatarDownloader(userinfoResponseText);
            await googleAvatarDownloader.GetAvatarAsync(CancellationToken.None).ConfigureAwait(false);
            return;
        }
    }
}
