using Microsoft.AspNetCore.Mvc;
using PTM.PublicDataModel;
using System;
using Tesseract.Common;

namespace PTM.Services.Users
{
    [Route("users")]
    public class UserController : Controller
    {
        IUserRepository mRepository;

        /// <summary>
        /// Domyślny ctor.
        /// </summary>
        /// <param name="repository">Repozytorium</param>
        public UserController(IUserRepository repository)
        {
            Ensure.ParamNotNull(repository, nameof(repository));

            mRepository = repository;
        }

        /// <summary>
        /// Tworzy usera
        /// </summary>
        /// <param name="user">Obiekt usera do stworzenia</param>
        [HttpPost]
        public virtual IActionResult CreateUser([FromBody] UserPublic user)
        {
            Ensure.ParamNotNull(user, nameof(user));

            UserPublic createdUser = mRepository.CreateUser(user);

            if (createdUser == null)
            {
                return base.NotFound();
            }

            return base.Created($"/users/{createdUser.ID}", createdUser);
        }

        /// <summary>
        /// Pobiera Usera o podanym ID
        /// </summary>
        /// <param name="ID">ID Usera</param>
        /// <returns>User</returns>
        [HttpGet("{ID}")]
        public IActionResult GetUser(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return base.Problem("Parameter has to be grater than 0");
                }

                UserPublic user = mRepository.GetUser(ID);

                if(user == null)
                {
                    return base.NotFound();
                }

                return base.Ok(user);
            }
            catch (Exception ex)
            {
                return base.Problem(ex.Message);
            }
        }

        /// <summary>
        /// Pobiera Usera o podanym koncie MS/Google
        /// </summary>
        /// <param name="OAuthID">kod konta Usera</param>
        /// <returns>User</returns>
        [HttpGet("OAuth/{OAuthID}")]
        public IActionResult GetUserOAuth(string OAuthID)
        {
            try
            {
                UserPublic user = mRepository.GetUserOAuth(OAuthID);

                if (user == null)
                {
                    return base.NotFound();
                }

                return base.Ok(user);
            }
            catch (Exception ex)
            {
                return base.Problem(ex.Message);
            }
        }

        /// <summary>
        /// Aktualizuje usera
        /// </summary>
        /// <param name="user">Obiekt Usera do zaktualizowania</param>
        [HttpPut]
        public virtual IActionResult UpdateUser([FromBody] UserPublic user)
        {
            Ensure.ParamNotNull(user, nameof(user));

            UserPublic updatedUser = mRepository.UpdateUser(user);

            if (updatedUser == null)
            {
                return base.NotFound();
            }

            return base.Created($"/users/{updatedUser.ID}", updatedUser);
        }

        /// <summary>
        /// Kasuje usera
        /// </summary>
        /// <param name="ID">ID usera do usunięcia</param>
        [HttpDelete("{ID}")]
        public virtual IActionResult DeleteUser(int ID)
        {
            if (ID <= 0)
            {
                return base.Problem("Parameter has to be grater than 0");
            }

            mRepository.DeleteUser(ID);

            return base.Ok($"/users/{ID}");
        }
    }
}
