using PTM.PublicDataModel;
using System.Collections.Generic;

namespace PTM.Services.Users
{
    /// <summary>
    /// Repozytorium Userów
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Tworzy usera
        /// </summary>
        /// <param name="user">Dane usera do utworzenia</param>
        /// <returns>User</returns>
        UserPublic CreateUser(UserPublic user);

        /// <summary>
        /// Pobiera usera o wskazanym ID
        /// </summary>
        /// <param name="ID">ID Useru do pobrania</param>
        /// <returns>User</returns>
        UserPublic GetUser(int ID);

        /// <summary>
        /// Pobiera usera o wskazanym ID
        /// </summary>
        /// <param name="ID">ID Useru do pobrania</param>
        /// <returns>User</returns>
        UserPublic GetUserOAuth(string OAuthID);

        /// <summary>
        /// Aktualizuje usera
        /// </summary>
        /// <param name="user">user do zaktualizowania</param>
        /// <returns>User</returns>
        UserPublic UpdateUser(UserPublic user);

        /// <summary>
        /// Kasuje usera
        /// </summary>
        /// <param name="ID">ID usera</param>
        void DeleteUser(int ID);
    }
}
