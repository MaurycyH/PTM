using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTM.Logic.Authentication
{
    /// <summary>
    /// Dostarcza metody autentykacji przez podanego providera
    /// </summary>
    internal interface IOAuthProvider
    {
        /// <summary>
        /// Autentykuje użytkownika
        /// </summary>
        /// <param name="cancellationToken">Token anulowania</param>
        Task<UserPublic> AuthenticateUserAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Loguje użytkownika przez refresh token Google i zwraca konto
        /// </summary>
        /// <param name="refreshToken">Token odświeżenia</param>
        /// <param name="cancellationToken">Token anulowania</param>
        /// <returns>Odświeżone konto usera</returns>
        Task<UserPublic> RefreshUserAsync(string refreshToken, CancellationToken cancellationToken);

    }
}
