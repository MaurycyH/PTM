using PTM.PublicDataModel;
using System.Threading;
using System.Threading.Tasks;

namespace PTM.Logic.Authentication
{
    /// <summary>
    /// Klasa procesująca request autentykacji
    /// </summary>
    public class AuthenticationProcessor
    {
        private IOAuthProvider mProvider = null;

        /// <summary>
        /// Autentykuje użytkownika po przez wskazanego providera
        /// </summary>
        /// <param name="provider">Dostawca autentykacji</param>
        /// <returns>Konto użytkownika. Null jeśli nie udało się uwierzytelnić</returns>
        public async Task<UserPublic> AuthenticateAsync(AuthenticationProvider provider, CancellationToken cancellationToken)
        {
            AuthenticationFactory factory = new AuthenticationFactory();
            mProvider = factory.CreateProvider(provider);

            CredentialsManager credentialsManager = new CredentialsManager(provider);
            string refreshToken = credentialsManager.LoadToken();

            if (!string.IsNullOrEmpty(refreshToken))
            {
                return await mProvider.RefreshUserAsync(refreshToken, cancellationToken).ConfigureAwait(false);
            }

            return await mProvider.AuthenticateUserAsync(cancellationToken).ConfigureAwait(false);
        }


    }
}
