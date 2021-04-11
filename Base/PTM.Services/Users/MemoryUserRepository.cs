using PTM.Entities;
using PTM.PublicDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Tesseract.Common;

namespace PTM.Services.Users
{
    public class MemoryUserRepository : IUserRepository
    {
        protected ICollection<UserPublic> mUsers;

        /// <summary>
        /// Domysłny ctor. Inicjalizuje kolekcję Userów.
        /// </summary>
        public MemoryUserRepository()
        {
            if (mUsers == null)
            {
                mUsers = new List<UserPublic>();
            }
        }

        /// <summary>
        /// Ctor pozwalający przekazać konkretną listę obiektów.
        /// </summary>
        /// <param name="Users">Lista obietków</param>
        public MemoryUserRepository(ICollection<UserPublic> Users)
        {
            Ensure.ParamNotNull(Users, nameof(Users));

            mUsers = Users;
        }

        /// <inheritdoc/>
        public UserPublic CreateUser(UserPublic User)
        {
            Ensure.ParamNotNull(User, nameof(User));

            mUsers.Add(User);

            return User;
        }

        public void DeleteUser(int ID)
        {
            mUsers.Remove(mUsers.Where(u => u.ID == ID).FirstOrDefault());
        }

        /// <inheritdoc/>
        public UserPublic GetUser(int ID)
        {
            UserPublic user = mUsers.FirstOrDefault(u => u.ID == ID);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public UserPublic GetUserOAuth(string OAuthID)
        {
            UserPublic user = mUsers.FirstOrDefault(u => u.OAuthID == OAuthID);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public UserPublic UpdateUser(UserPublic user)
        {
            UserPublic oldUser = mUsers.FirstOrDefault(u => u.ID == user.ID);

            if (oldUser == null)
            {
                return null;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            return oldUser;
        }
    }
}
