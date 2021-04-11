using PTM.Entities;
using PTM.PublicDataModel;
using System.Threading.Tasks;

namespace PTM.Services.Client.UserClient
{
    /// <summary>
    /// Interfejs klienta usera
    /// </summary>
    public interface IUserClient
    {
        /// <summary>
         /// Tworzy usera
         /// </summary>
         /// <param name="user">Dane usera do stworzenia</param>
         /// <returns>Stworzony obiekt usera</returns>
        Task<UserPublic> CreateUser(UserPublic user);

        /// <summary>
        /// Pobiera usera o wskazanym ID
        /// </summary>
        /// <param name="ID">ID usera</param>
        /// <returns>User</returns>
        Task<UserPublic> GetUser(int ID);

        /// <summary>
        /// Pobiera usera o wskazanym kodzie konta MS/Google
        /// </summary>
        /// <param name="OAuthID">kod konta usera</param>
        /// <returns>User</returns>
        Task<UserPublic> GetUserOAuth(string OAuthID);

        /// <summary>
        /// Aktualizuje usera
        /// </summary>
        /// <param name="user">user do zaktualizowania</param>
        /// <returns>User</returns>
        Task<UserPublic> UpdateUser(UserPublic user);

        /// <summary>
        /// Kasuje usera
        /// </summary>
        /// <param name="ID">ID usera do skasowania</param>
        /// <returns>User</returns>
        Task DeleteUser(int ID);
    }
}
