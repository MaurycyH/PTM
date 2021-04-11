using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.Logic.Authentication
{
    /// <summary>
    /// Fabryka dostawców autentykacji
    /// </summary>
    internal class AuthenticationFactory
    {
        /// <summary>
        /// Tworzy dostawcę autentykacji
        /// </summary>
        /// <param name="authProvider">Typ dostawcy autentykacji</param>
        /// <returns>Dostawca autentykacji</returns>
        public virtual IOAuthProvider CreateProvider(AuthenticationProvider authProvider)
        {
            IOAuthProvider provider = null;

            switch (authProvider)
            {
                case AuthenticationProvider.Google:
                    provider = new GoogleAuthentication();
                    break;
                case AuthenticationProvider.Microsoft:
                    provider = new MicrosoftAuthentication();
                    break;
                default:
                    break;
            }

            return provider;
        }
    }
}
