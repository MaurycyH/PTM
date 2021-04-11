using Newtonsoft.Json.Linq;
using PTM.PublicDataModel;
using System;

namespace PTM.Logic.Authentication
{
    public class UserAccountFactory
    {
        /// <summary>
        /// Tworzy nowego usera w oparciu o json'a zwrotnego z autentykatora, w przypadku błędu zrobic wyjatek
        /// </summary>
        /// <param name="provider">Nazwa providera autentykacji</param>
        /// <param name="jsonResponse">Token zawierajacy dane do stworzenia uzytkownika</param>
        /// <returns>Zwraca stworzone konto użytkownika</returns>
        public UserPublic Create(AuthenticationProvider provider, string jsonResponse)
        {
            try
            {
                JObject jsonParsed = JObject.Parse(jsonResponse);
                UserPublic userAccount = new UserPublic();
                switch (provider)
                {
                    case AuthenticationProvider.Google:
                        userAccount.FirstName = (string)jsonParsed["given_name"];
                        userAccount.LastName = (string)jsonParsed["family_name"];
                        userAccount.OAuthID = (string)jsonParsed["sub"];
                        break;
                    case AuthenticationProvider.Microsoft:
                        userAccount.FirstName = (string)jsonParsed["givenName"];
                        userAccount.LastName = (string)jsonParsed["surname"];
                        userAccount.OAuthID = (string)jsonParsed["id"];
                        break;
                    default:
                        throw new ArgumentException("Problem ze stworzeniem uzytkownika", nameof(provider));
                }

                return userAccount;
            }
            catch
            {
                throw;
                //TODO: Zalogowac przypadek

            }
        }
    }
}
